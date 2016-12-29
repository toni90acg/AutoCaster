using System;

namespace AutoCaster.Exceptions
{
    public class AutoCasterInvalidCastingException : Exception
    {
        public AutoCasterInvalidCastingException(string message) : base(message){}
    }
}
