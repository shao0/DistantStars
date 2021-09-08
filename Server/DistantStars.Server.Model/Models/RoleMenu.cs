using System.ComponentModel.DataAnnotations.Schema;

namespace DistantStars.Server.Model.Models
{
    public class RoleMenu
    {
        public int RoleId { get; set; }

        public int MenuId { get; set; }
        
        [ForeignKey("RoleId")]
        public RoleInfo Role { get; set; }

        [ForeignKey("MenuId")]
        public MenuInfo Menu{ get; set; }

    }
}
