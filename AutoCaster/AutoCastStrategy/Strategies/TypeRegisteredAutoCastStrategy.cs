using System;
using AutoCaster.AutoCastStrategy.Strategies.Interface;

namespace AutoCaster.AutoCastStrategy.Strategies
{
    public class TypeRegisteredAutoCastStrategy : IAutoCastStrategy
    {
        public object AutoCast(AutoCaster context)
        {
            try
            {
                object castedObject;
                return context
                    .TryCastElement(context.CastToType, context.ObjectToCast, out castedObject) ?
                    castedObject : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}