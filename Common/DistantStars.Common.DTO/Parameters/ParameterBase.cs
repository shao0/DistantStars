using System;

namespace DistantStars.Common.DTO.Parameters
{
    public class ParameterBase
    {
        public string Search { get; set; }


        public bool GetParameter<T>(string propertyName, out T value)
        {
            var result = false;
            value = default;
            var property = GetType().GetProperty(propertyName);
            if (property?.PropertyType == typeof(T))
            {
                value = (T)property.GetValue(this);
                result = true;
            }
            return result;
        }


        public string GetUriParameter()
        {
            var properties = GetType().GetProperties();
            var result = "?";
            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                if (value != null)
                    result += $"{property.Name}={value}&";
            }
            result = result.Substring(0,result.Length-1);
            return result;
        }
    }
}
