using System;
using System.Collections.Generic;
using System.Reflection;

namespace Injecta
{
    public class Container
    {
        private Dictionary<Type, object> _bindings;
        private Container _parent;

        public Container() : this(null)
        {
        }

        public Container(Container parent)
        {
            _parent = parent;
            _bindings = new Dictionary<Type, object>();
        }

        public void BindInstances(params object[] instances)
        {
            Array.ForEach(instances, o => _bindings[o.GetType()] = o);
        }

        public void BindInstance<T>(T instance)
        {
            _bindings[typeof(T)] = instance;
        }      

        public bool TryGetBinding(Type type, out object value)
        {
            return _bindings.TryGetValue(type, out value);
        }

        public void Resolve(object instance)
        {
            Container[] allContainers = GetAllContainers();
            ResolveFields(instance, allContainers);
            ResolveProperties(instance, allContainers);
            ResolveMethods(instance, allContainers);
        }

        private Container[] GetAllContainers()
        {
            List<Container> allContainers = new List<Container>() { this };
            Container current = this;

            while (current._parent != null)
            {
                allContainers.Add(current._parent);
                current = current._parent;
            }

            allContainers.Reverse();

            return allContainers.ToArray();
        }

        private void ResolveFields(object instance, Container[] containers)
        {
            Type type = instance.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo[] fields = Array.FindAll(type.GetFields(flags), f => f.IsDefined(typeof(InjectAttribute)));

            foreach (FieldInfo field in fields)
            {
                if (TryGetBinding(containers, field.FieldType, out object value))
                    field.SetValue(instance, value);
            }
        }

        private void ResolveProperties(object instance, Container[] containers)
        {
            Type type = instance.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            PropertyInfo[] properties = Array.FindAll(type.GetProperties(flags), f => f.IsDefined(typeof(InjectAttribute)));

            foreach (PropertyInfo property in properties)
            {
                if (TryGetBinding(containers, property.PropertyType, out object value))
                    property.SetValue(instance, value);
            }
        }

        private void ResolveMethods(object instance, Container[] containers)
        {
            Type type = instance.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            MethodInfo[] methods = Array.FindAll(type.GetMethods(flags), f => f.IsDefined(typeof(InjectAttribute)));

            Array.ForEach(methods, m =>
            {
                List<object> args = new List<object>();

                Array.ForEach(m.GetParameters(), p =>
                {
                    if (TryGetBinding(containers, p.ParameterType, out object value))
                        args.Add(value);
                    else
                        args.Add(null);
                });

                m.Invoke(instance, args.ToArray());
            });
        }

        private bool TryGetBinding(Container[] containers, Type propertyType, out object value)
        {
            value = null;
            bool hasBinding = false;

            for (int i = 0; i < containers.Length && !hasBinding; i++)
            {
                Container container = containers[i];
                hasBinding = container.TryGetBinding(propertyType, out value);
            }

            return hasBinding;
        }
    }
}
