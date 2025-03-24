using System;

namespace Injecta
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
    }
}
