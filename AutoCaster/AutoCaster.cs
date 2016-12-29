using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoCaster.Exceptions;
using AutoCaster.Interfaces;

namespace AutoCaster
{
    public class AutoCaster : IAutoCaster
    {
        private readonly Dictionary<Type, Func<object, object>> _optionsToCast;

        #region Constructor
        public AutoCaster()
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

        #region AUTOCAST

        public T AutoCast<T>(object objectToCast) where T : class
        {
            object castedOject;
            return TryAutoCast(objectToCast, typeof(T), out castedOject) ? castedOject as T : null;
        }

        public bool TryAutoCast(object objectToCast, Type castToType, out object castedObject)
        {
            if (castToType == objectToCast.GetType())
            {
                castedObject = objectToCast;
                return true;
            }
            try
            {
                if (_optionsToCast.ContainsKey(castToType))
                {
                    if (TryCastElement(castToType, objectToCast, out castedObject))
                    {
                        return true;
                    }
                }
                if (TryAutoMap(castToType, objectToCast, out castedObject))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                castedObject = null;
                return false;
            }
            castedObject = null;
            return false;
        }

        #endregion

        #region Register and Unregister
        
        public IAutoCaster RegisterCastMapping(Type typeToCast, Func<object, object> mappingFunc)
        {
            TypeIsRegistered(typeToCast);
            if (!TypeIsRegistered(typeToCast))
            {
                _optionsToCast.Add(typeToCast, mappingFunc);
            }
            return this;
        }

        public IAutoCaster RegisterCastMapping<T>(Func<object, object> mappingFunc)
        {
            return RegisterCastMapping(typeof(T), mappingFunc);
        }

        public IAutoCaster UnegisterCastMapping(Type typeToCast)
        {
            if (TypeIsRegistered(typeToCast))
            {
                _optionsToCast.Remove(typeToCast);
            }
            return this;
        }

        public IAutoCaster UnregisterCastMapping<T>()
        {
            return UnegisterCastMapping(typeof(T));
        }

        #endregion

        #region Getters

        public IList<Type> GetListOfTypesRegistered()
        {
            return _optionsToCast.Select(k => k.Key).ToList();
        }

        public Func<object, object> GetFuncForType(Type type)
        {
            return _optionsToCast
                .Where(t => t.Key == type)
                .Select(f => f.Value).FirstOrDefault();
        }

        #endregion

        #endregion

        #region Private Methods
        private bool TryCastElement(Type typeToCast, object objectToCast, out object castedObject)
        {
            var castingFunc = _optionsToCast
                .Where(t => t.Key == typeToCast)
                .Select(v => v.Value).FirstOrDefault();

            var castedElement = castingFunc?.Invoke(objectToCast);

            if (castedElement == null)
            {
                throw new AutoCasterInvalidCastingException("Imposible to cast");
            }

            castedObject = castedElement;
            return true;
        }

        private bool TypeIsRegistered(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return _optionsToCast.ContainsKey(type);
        }

        private bool TryAutoMap(Type typeToCast, object objectToMap, out object castedObject)
        {
            var target = Activator.CreateInstance(typeToCast);

            var propertyInfosObjectToCast = objectToMap.GetType().GetProperties();
            var propertyInfosCastedObject = target.GetType().GetProperties();

            foreach (var propertyCastedObject in propertyInfosCastedObject)
            {
                var propertiesWithSameName = propertyInfosObjectToCast
                    .FirstOrDefault(o => o.Name == propertyCastedObject.Name);

                if (propertiesWithSameName == null) continue;

                var value = propertiesWithSameName.GetValue(objectToMap);
                var propertyType = propertyCastedObject.PropertyType;
                object valueCasted;
                TryAutoCast(value, propertyType, out valueCasted);
                propertyCastedObject.SetValue(target, valueCasted);
            }

            castedObject = target;
            return true;
        }
        #endregion
    }
}
