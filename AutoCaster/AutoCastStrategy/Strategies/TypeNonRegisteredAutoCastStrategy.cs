using System;
using AutoCaster.AutoCastStrategy.Strategies.Interface;

namespace AutoCaster.AutoCastStrategy.Strategies
{
    public class TypeNonRegisteredAutoCastStrategy : IAutoCastStrategy
    {
        public object AutoCast(AutoCaster context)
        {
            try
            {
                object castedObject;
                return context
                    .TryAutoMap(context.CastToType, context.ObjectToCast, out castedObject) ? 
                    castedObject : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}