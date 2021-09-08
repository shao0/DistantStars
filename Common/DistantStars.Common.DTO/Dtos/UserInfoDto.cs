using System.Collections.Generic;

namespace DistantStars.Common.DTO.Dtos
{
    public class UserInfoDto
    {
        public int Id { get; set; }

        public string UserAccount { get; set; }
        
        public string UserName { get; set; }

        public bool ModifyPassword { get; set; }
        public string UserPassword { get; set; }

        public string UserIcon { get; set; }

        public int RoleId { get; set; }

        public IEnumerable<MenuInfoDto> Menus { get; set; }

    }
}
