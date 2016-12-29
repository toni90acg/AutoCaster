using System;

namespace AutoCaster.Exceptions
{
    public class AutoCasterTypeRegisteredException : Exception
    {
        public AutoCasterTypeRegisteredException(string message) : base(message) { }
    }
}