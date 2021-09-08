using System.ComponentModel.DataAnnotations.Schema;

namespace DistantStars.Server.Model.Models
{
    [Table("user_role")]
    public class UserRole
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }

        public UserInfo UserInfo { get; set; }

        public RoleInfo RoleInfo { get; set; }
    }
}
