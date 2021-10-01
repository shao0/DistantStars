using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DistantStars.Client.Resource.Fonts
{
    public static class IconValues
    {
        /// <summary>
        /// 根菜单
        /// </summary>
        public static string RootMenuIcon = "e603";

        private static readonly string[] _Icons = {
            //菜单
            RootMenuIcon,
            //账号设置
            "e700",
            //角色设置
            "e634",
            //菜单设置
            "e65f",
            //系统设置
            "e69c",
            //用户设置
            "e7ea",
        };

        public static string[] Icons => _Icons.Select(f => f.FontValueToUnicodeConvert()).ToArray();
        /// <summary>
        /// 字体值转Unicode
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string FontValueToUnicodeConvert(this string source)
        {
            return ((char)int.Parse(source, NumberStyles.HexNumber)).ToString();
        }

        /// <summary>
        /// Unicode转字体值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnicodeToFontValueConvert(this string value)
        {
            return ((int)value.ToCharArray()[0]).ToString("x");
        }

    }
}
