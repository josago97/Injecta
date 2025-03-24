using System;

namespace Injecta
{
    public class InjectException : Exception
    {
        internal InjectException(string message) : base(message) { }
    }
}
