using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Common.DTO.Enums;

namespace DistantStars.Client.Common
{
    public static class Global
    {
        public static UserInfoModel CurrentUserInfo { get; set; }
        public static Dictionary<string, object> MenuTypes { get; set; } = new Dictionary<string, object>();

        static Global()
        {
            InitialMenuType();
        }
        static void InitialMenuType()
        {
            var type = typeof(MenuType);
            FieldInfo fieldInfo;
            foreach (var enumValue in type.GetEnumValues())
            {
                string name = Enum.GetName(type, enumValue);
                fieldInfo = type.GetField(name);
                var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute), false);
                MenuTypes.Add(attribute.Description, enumValue);
            }
        }
    }
}
