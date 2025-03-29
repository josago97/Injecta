using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Injecta
{
    public class Container
    {
        private static readonly Type OBJECT_TYPE = typeof(object);

        private Dictionary<Type, object> _bindings;
        private Container _parent;

        public Container() : this(null) { }

        public Container(Container parent)
        {
            _parent = parent;
            _bindings = new Dictionary<Type, object>();

            BindInstance(this);
        }

        public void BindInstances(params object[] instances)
        {
            Array.ForEach(instances, BindInstance);
        }

        public void BindInstance<T>(T instance)
        {
            _bindings[typeof(T)] = instance;
        }

        public bool TryGetBinding(Type type, out object value)
        {
            value = null;
            bool hasBinding = false;
            Container container = this;

            do
            {
                hasBinding = container._bindings.TryGetValue(type, out value);
                container = container._parent;
            }
            while (!hasBinding && container != null);

            return hasBinding;
        }

        public void Resolve(object instance)
        {
            ResolveFields(instance);
            ResolveProperties(instance);
            ResolveMethods(instance);
        }
        private void ResolveFields(object instance)
        {
            FieldInfo[] fields = GetInjectMembers<FieldInfo>(instance, type => type.GetFields);

            foreach (FieldInfo field in fields)
            {
                if (TryGetBinding(field.FieldType, out object value))
                    field.SetValue(instance, value);
            }
        }

        private void ResolveProperties(object instance)
        {
            PropertyInfo[] properties = GetInjectMembers<PropertyInfo>(instance, type => type.GetProperties);

            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite && TryGetBinding(property.PropertyType, out object value))
                    property.SetValue(instance, value);
            }
        }

        private void ResolveMethods(object instance)
        {
            MethodInfo[] methods = GetInjectMembers<MethodInfo>(instance, type => type.GetMethods);

            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();
                object[] args = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo param = parameters[i];
                    TryGetBinding(param.ParameterType, out object value);
                    args[i] = value;
                }

                method.Invoke(instance, args);
            }
        }

        private T[] GetInjectMembers<T>(object instance, Func<Type, Func<BindingFlags, T[]>> func) where T : MemberInfo
        {
            const BindingFlags baseBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            const BindingFlags bindingFlags = BindingFlags.Public | baseBindingFlags;

            Type type = instance.GetType();
            List<T> members = func(type)(bindingFlags).Where(HasInjectAttribute).ToList();

            while ((type = type.BaseType) != null && type != OBJECT_TYPE)
            {
                IEnumerable<T> privateMembers = func(type)(bindingFlags)
                    .Where(member => IsPrivateMember(member) && HasInjectAttribute(member));

                members.AddRange(privateMembers);
            }

            return members.ToArray();
        }

        private bool HasInjectAttribute(MemberInfo memberInfo)
        {
            return memberInfo.IsDefined(typeof(InjectAttribute));
        }

        private bool IsPrivateMember(MemberInfo member)
        {
            return member is FieldInfo field && field.IsPrivate ||
                   member is MethodBase method && method.IsPrivate ||
                   member is PropertyInfo property && property.CanWrite && property.SetMethod.IsPrivate;
        }
    }
}
