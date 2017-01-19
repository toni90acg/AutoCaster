using System.Collections.Generic;
using System.Reflection;

namespace AutoCaster.Utils
{
    public sealed class PairPropertiesInfo : Dictionary<PropertyInfo,PropertyInfo>
    {
        /// <summary>
        /// Add two property info to the dictionary
        /// </summary>
        /// <param name="propertyWithSameName">Refered to the property with same name</param>
        /// <param name="castedObjectProperty">Refered to the property of the casted object</param>
        public void AddProperty(PropertyInfo propertyWithSameName, PropertyInfo castedObjectProperty)
        {
           Add(propertyWithSameName,castedObjectProperty); 
        }
    }
}
