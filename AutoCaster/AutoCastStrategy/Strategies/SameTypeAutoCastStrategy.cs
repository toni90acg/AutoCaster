using AutoCaster.AutoCastStrategy.Strategies.Interface;

namespace AutoCaster.AutoCastStrategy.Strategies
{
    public class SameTypeAutoCastStrategy : IAutoCastStrategy
    {
        public object AutoCast(AutoCaster context)
        {
            return context.ObjectToCast;
        }
    }
}