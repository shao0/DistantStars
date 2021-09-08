using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistantStars.Server.Model.Models
{
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string UserIcon { get; set; }
        
        public int RoleId { get; set; }

        public RoleInfo RoleInfo { get; set; }

    }
}
