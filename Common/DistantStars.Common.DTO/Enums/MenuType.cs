using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DistantStars.Common.DTO.Enums
{
    public enum MenuType
    {
        [Description("其他菜单")]
        None,
        [Description("系统菜单")]
        System,
        [Description("主页菜单")]
        Home,
    }
}
