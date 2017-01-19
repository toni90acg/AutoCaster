using System.Linq;
using System.Reflection;

namespace AutoCaster.Utils
{
    public class MappingElementProperties
    {
        public object ObjectToMap;
        public PropertyInfo[] ObjectToCastProperties { get; set; }
        public PropertyInfo[] CastedObjectProperties { get; set; }
        public PairPropertiesInfo PropertiesWithSameName { get; set; }

        public MappingElementProperties(object objectToMap, object castedObject)
        {
            PropertiesWithSameName = new PairPropertiesInfo();
            ObjectToMap = objectToMap;
            ObjectToCastProperties = objectToMap.GetType().GetProperties();
            CastedObjectProperties = castedObject.GetType().GetProperties();

            FindPropertiesWithSameName();
        }

        private void FindPropertiesWithSameName()
        {
            foreach (var castedObjectProperty in CastedObjectProperties)
            {
                var propertyWithSameName = ObjectToCastProperties
                    .FirstOrDefault(objectToCastProperty => castedObjectProperty.Name == objectToCastProperty.Name);

                if (propertyWithSameName == null) continue;

                PropertiesWithSameName
                    .AddProperty(propertyWithSameName,
                        castedObjectProperty);
            }
        }
    }
}