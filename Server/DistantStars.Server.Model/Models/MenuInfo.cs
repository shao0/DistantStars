using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantStars.Server.Model.Models
{
    public class MenuInfo
    {
        public MenuInfo()
        {
            RoleMenus = new List<RoleMenu>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuId { get; set; }

        public string MenuHeader { get; set; }

        public string MenuIcon { get; set; }

        public string TargetView { get; set; }

        public string MenuType { get; set; }

        public int ParentId { get; set; }

        public int Index { get; set; }
        
        [DefaultValue(1)]
        public int State { get; set; }

        public IEnumerable<RoleMenu> RoleMenus { get; set; }
    }
}
