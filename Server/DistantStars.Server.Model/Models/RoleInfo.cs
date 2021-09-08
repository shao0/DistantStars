using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantStars.Server.Model.Models
{
   public class RoleInfo
    {
        public RoleInfo()
        {
            RoleMenus = new List<RoleMenu>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        [DefaultValue(1)]
        public int State { get; set; }

        public IEnumerable<RoleMenu> RoleMenus { get; set; }

    }
}
