using System;
using System.Collections.Generic;

namespace AutoCaster.Interfaces
{
    public interface IAutoCaster
    {
        #region AutoCast

        /// <summary>
        /// Casts everything. But is generally used to cast simple objects like:
        /// string, int, double, etc...
        /// </summary>
        /// <param name="typeToCast"> Type that we want to get</param>
        /// <param name="objectToCast"> object that we want to cast</param>
        /// <param name="castedObject"> the output casted object</param>
        /// <returns> Returns if the AutoCaster succeded in casting the object</returns>
        bool TryAutoCast(object objectToCast, Type typeToCast, out object castedObject);

        /// <summary>
        /// Cast classes (or maps objects)
        /// </summary>
        /// <typeparam name="T"> This is the Type that we want to get </typeparam>
        /// <param name="objectToCast"> This is the object that we want to cast</param>
        /// <returns> Returns the casted object</returns>
        T AutoCast<T>(object objectToCast) where T : class;

        #endregion

        #region Registration and Unregistration

        /// <summary>
        /// Registers a mapping func (a func with an specific mapping) with an specific type
        /// </summary>
        /// <param name="typeToCast">The type that will be registered</param>
        /// <param name="mappingFunc">The specific mapping</param>
        IAutoCaster RegisterCastMapping(Type typeToCast, Func<object, object> mappingFunc);

        /// <summary>
        /// Register a mapping func with an specific type overwriting any past mapping func
        /// </summary>
        /// <param name="typeToCast">The type that will be registered</param>
        /// <param name="mappingFunc">The specific mapping</param>
        IAutoCaster RegisterCastMappingForced(Type typeToCast, Func<object, object> mappingFunc);

        /// <summary>
        /// Registers a mapping func (a func with an specific mapping) with an specific type
        /// </summary>
        /// <typeparam name="T">The type that will be registered</typeparam>
        /// <param name="mappingFunc">The specific mapping</param>
        IAutoCaster RegisterCastMapping<T>(Func<object, object> mappingFunc);

        /// <summary>
        /// Register a mapping func with an specific type overwriting any past mapping func
        /// </summary>
        /// <typeparam name="T">The type that will be registered</typeparam>
        /// <param name="mappingFunc">The specific mapping</param>
        IAutoCaster RegisterCastMappingForced<T>(Func<object, object> mappingFunc);

        /// <summary>
        /// Unegisters a mapping func (a func with an specific mapping) with an specific type
        /// </summary>
        /// <param name="typeToCast">The type that will be unregistered</param>
        IAutoCaster UnregisterCastMapping(Type typeToCast);

        /// <summary>
        /// Unegisters a mapping func (a func with an specific mapping) with an specific type
        /// </summary>
        /// <typeparam name="T">The type that will be unregistered</typeparam>
        /// <returns></returns>
        IAutoCaster UnregisterCastMapping<T>();

        #endregion

        #region Getters

        /// <summary>
        /// Gets the list of all the registered types
        /// </summary>
        /// <returns></returns>
        IList<Type> GetListOfTypesRegistered();

        /// <summary>
        /// Get the specific mapping func for a registered type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The mapping func of a registered type</returns>
        Func<object, object> GetFuncForType(Type type);

        #endregion
    }
}
