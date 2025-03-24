using System;
using UnityEngine;

namespace Injecta
{
    [DefaultExecutionOrder(ORDER_EXECUTION)]
    public class ContextBase : MonoBehaviour
    {
        public const int ORDER_EXECUTION = -1000;

        protected void ResolveBindings(Container container, object[] instances)
        {
            Array.ForEach(instances, container.Resolve);
        }

        protected void ResolveBindings(Container container, object instance)
        {
            container.Resolve(instance);
        }
    }
}
