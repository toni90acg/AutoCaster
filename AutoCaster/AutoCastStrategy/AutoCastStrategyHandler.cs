using System;
using System.Collections.Generic;
using System.Linq;
using AutoCaster.AutoCastStrategy.Strategies;
using AutoCaster.AutoCastStrategy.Strategies.Interface;

namespace AutoCaster.AutoCastStrategy
{
    public class AutoCastStrategyHandler
    {
        private readonly
            Dictionary<Func<Type, bool>, IAutoCastStrategy> _dictionaryStrategies;

        private readonly AutoCaster _context;

        public AutoCastStrategyHandler(AutoCaster context)
        {
            _context = context;
            _dictionaryStrategies = new Dictionary<Func<Type, bool>, IAutoCastStrategy>
            {
                {
                    type => _context.ObjectToCast.GetType() == type,
                    new SameTypeAutoCastStrategy()
                },
                {
                    type => _context.OptionsToCast.ContainsKey(type),
                    new TypeRegisteredAutoCastStrategy()
                },
                {
                    type => !_context.OptionsToCast.ContainsKey(type),
                    new TypeNonRegisteredAutoCastStrategy()
                }
            };
        }

        public IAutoCastStrategy GetAutoCastStrategy(Type type)
        {
            return _dictionaryStrategies
                .Where(k => k.Key.Invoke(type))
                .Select(k => k.Value)
                .FirstOrDefault();
        }
    }
}