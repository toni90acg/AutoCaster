using System;
using System.Collections.Generic;
using System.Linq;
using AutoCaster.Exceptions;

namespace AutoCaster
{
    public class AutoCasterSingleton
    {
        private readonly Dictionary<Type, Func<object, object>> _optionsToCast;
        private static AutoCasterSingleton _autoCasterSingleton;

        #region Constructor

        private AutoCasterSingleton()
        {
            _optionsToCast = new Dictionary<Type, Func<object, object>>
            {
                {typeof(string), o => o.ToString()},
                {typeof(int), o => Convert.ToInt32(o)},
                {typeof(double), o => Convert.ToDouble(o)},
                {typeof(bool), o => Convert.ToBoolean(o)}
            };
        }

        #endregion

        #region Public Methods

        public static AutoCasterSingleton GetAutoCasterInstance()
        {
            return _autoCasterSingleton ?? (_autoCasterSingleton = new AutoCasterSingleton());
        }

        public bool TryAutoCast(object objectToCast, Type castToType, out object castedObject)
        {
            if (castToType == objectToCast.GetType())
            {
                castedObject = objectToCast;
                return true;
            }

            if (_optionsToCast.ContainsKey(castToType))
            {
                try
                {
                    if (TryCastElement(castToType, objectToCast, out castedObject)) return true;
                }
                catch (Exception)
                {
                    castedObject = null;
                    return false;
                }
            }

            castedObject = null;
            return false;
        }

        public T AutoCast<T>(object objectToCast) where T : class
        {
            if (typeof(T) == objectToCast.GetType())
            {
                return objectToCast as T;
                
            }

            if (!_optionsToCast.ContainsKey(typeof(T))) return null;

            try
            {
                return CastElement<T>(typeof(T), objectToCast);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AutoCasterSingleton RegisterCastMapping(Type typeToCast, Func<object, object> mappingFunc)
        {
            if (_optionsToCast.ContainsKey(typeToCast))
            {
                return null;
            }
            _optionsToCast.Add(typeToCast, mappingFunc);
            return this;
        }

        #endregion

        #region Private Methods

        private bool TryCastElement(Type typeToCast, object objectToCast, out object o)
        {
            var castingFunc = _optionsToCast
                .Where(t => t.Key == typeToCast)
                .Select(v => v.Value).FirstOrDefault();

            var castedElement = castingFunc?.Invoke(objectToCast);

            if (castedElement == null)
            {
                throw new AutoCasterInvalidCastingException("Imposible to cast");
            }

            o = castedElement;
            return true;
        }

        private T CastElement<T>(Type typeToCast, object objectToCast) where T : class
        {
            var castingFunc = _optionsToCast
                .Where(t => t.Key == typeToCast)
                .Select(v => v.Value).FirstOrDefault();

            var castedElement = castingFunc?.Invoke(objectToCast) as T;

            if (castedElement == null)
            {
                throw new AutoCasterInvalidCastingException("Imposible to cast");
            }

            return castedElement;
        }

        #endregion
    }
}