using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistantStars.Client.Common.Helpers
{
    public static class AutoMapperHelper
    {
        public static T Map<TSource,T>(this TSource source)
        {
            var result = Activator.CreateInstance<T>();
            var resultType = typeof(T);
            var resultProperties = resultType.GetProperties();
            var sourceType = source.GetType();
            foreach (var property in resultProperties)
            {
                var propertyInfo = sourceType.GetProperty(property.Name);
                if (propertyInfo != null&& property.PropertyType== propertyInfo.PropertyType)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    property.SetValue(result,propertyValue);
                }
            }
            return result;
        }
    }
}
