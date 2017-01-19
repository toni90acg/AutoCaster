using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoCaster.AutoCastStrategy;
using AutoCaster.Exceptions;
using AutoCaster.Interfaces;
using AutoCaster.Utils;

namespace AutoCaster
{
    public class AutoCaster : IAutoCaster
    {
        internal readonly Dictionary<Type, Func<object, object>> OptionsToCast;
        internal object ObjectToCast;
        internal Type CastToType;

        #region Constructor

        public AutoCaster()
        {
            OptionsToCast = new Dictionary<Type, Func<object, object>>
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
            ObjectToCast = objectToCast;
            CastToType = castToType;

            castedObject = new AutoCastStrategyHandler(this)
                .GetAutoCastStrategy(castToType)
                .AutoCast(this);

            return castedObject != null;
        }

        [Obsolete("Better to use TryAutoCast")]
        public bool TryAutoCastWithoutStrategies(object objectToCast, Type castToType, out object castedObject)
        {
            if (castToType == objectToCast.GetType())
            {
                castedObject = objectToCast;
                return true;
            }
            try
            {
                if (OptionsToCast.ContainsKey(castToType))
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
            if (TypeIsRegistered(typeToCast))
            {
                throw new AutoCasterTypeRegisteredException($"The type: {typeToCast.Name}, has already been registered");
            }

            OptionsToCast.Add(typeToCast, mappingFunc);

            return this;
        }

        public IAutoCaster RegisterCastMappingForced(Type typeToCast, Func<object, object> mappingFunc)
        {
            if (TypeIsRegistered(typeToCast))
            {
                OptionsToCast.Remove(typeToCast);
            }
            OptionsToCast.Add(typeToCast, mappingFunc);

            return this;
        }

        public IAutoCaster RegisterCastMapping<T>(Func<object, object> mappingFunc)
        {
            return RegisterCastMapping(typeof(T), mappingFunc);
        }

        public IAutoCaster RegisterCastMappingForced<T>(Func<object, object> mappingFunc)
        {
            return RegisterCastMappingForced(typeof(T), mappingFunc);
        }

        public IAutoCaster UnregisterCastMapping(Type typeToCast)
        {
            if (TypeIsRegistered(typeToCast))
            {
                OptionsToCast.Remove(typeToCast);
            }
            return this;
        }

        public IAutoCaster UnregisterCastMapping<T>()
        {
            return UnregisterCastMapping(typeof(T));
        }

        #endregion

        #region Getters

        public IList<Type> GetListOfTypesRegistered()
        {
            return OptionsToCast.Select(k => k.Key).ToList();
        }

        public Func<object, object> GetFuncForType(Type type)
        {
            return OptionsToCast
                .Where(t => t.Key == type)
                .Select(f => f.Value)
                .FirstOrDefault();
        }

        #endregion

        #endregion

        #region Private Methods

        internal bool TryCastElement(Type typeToCast, object objectToCast, out object castedObject)
        {
            castedObject = OptionsToCast
                .Where(type => type.Key == typeToCast)
                .Select(func => func.Value)
                .FirstOrDefault()?
                .Invoke(objectToCast);

            return castedObject != null;
        }

        private bool TypeIsRegistered(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return OptionsToCast.ContainsKey(type);
        }

        internal bool TryAutoMap(Type typeToCast, object objectToMap, out object castedObject)
        {
            castedObject = Activator.CreateInstance(typeToCast);

            var mappingElement = new MappingElementProperties(objectToMap, castedObject);

            foreach (var propertyInfoPair in mappingElement.PropertiesWithSameName)
            {
                var valueCasted = CreateCastedObjectByPropertyInfo(propertyInfoPair, objectToMap);

                var propertyCastedObject = propertyInfoPair.Value;
                propertyCastedObject.SetValue(castedObject, valueCasted);
            }
            return true;
        }

        private object CreateCastedObjectByPropertyInfo(KeyValuePair<PropertyInfo, PropertyInfo> propertyInfoPair, object objectToMap)
        {
            var propertyValueObjectToMap = propertyInfoPair.Key.GetValue(objectToMap);
            var propertyTypeCastedObject = propertyInfoPair.Value.PropertyType;

            object valueCasted;
            TryAutoCast(propertyValueObjectToMap, propertyTypeCastedObject, out valueCasted);

            return valueCasted;
        }

        #endregion
    }
}