using System.Collections.Generic;

namespace DistantStars.Common.DTO.Dtos
{
    public class MenuInfoDto
    {
        public int MenuId { get; set; }

        public string MenuHeader { get; set; }

        public string MenuIcon { get; set; }

        public string TargetView { get; set; }

        public string MenuType { get; set; }

        public int ParentId { get; set; }

        public int Index { get; set; }
        
        public int State { get; set; }

        public string StateName { get; set; }
        public IEnumerable<RoleInfoDto> Roles { get; set; }
    }
}
