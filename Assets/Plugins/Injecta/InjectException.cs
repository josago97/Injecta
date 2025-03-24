using System;

namespace Injecta
{
    public class InjectException : Exception
    {
        public InjectException(string message) : base(message) { }
    }
}
