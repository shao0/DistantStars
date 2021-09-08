using System.Collections.Generic;

namespace DistantStars.Common.DTO.Dtos
{
   public class RoleInfoDto
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int State { get; set; }
        public string StateName { get; set; }
        public IEnumerable<MenuInfoDto> Menus { get; set; }
    }
}
