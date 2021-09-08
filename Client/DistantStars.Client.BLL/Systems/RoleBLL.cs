using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.IDAL.Systems;
using DistantStars.Client.Model.Models;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.BLL.Systems
{
    public class RoleBLL : IRoleBLL
    {
        private readonly IRoleDAL _role;

        public RoleBLL(IRoleDAL role)
        {
            _role = role;
        }
        public async Task<IEnumerable<RoleInfoModel>> GetAllRolesAsync(RoleParameter parameter = null)
        {
            var allRoles = await _role.GetAllRolesAsync(parameter);
            return allRoles.Select(r => r.Map<RoleInfoDto, RoleInfoModel>());
        }

        public async Task<RoleInfoModel> UpdateRoleAsync(RoleInfoModel role)
        {
            var dto = role.Map<RoleInfoModel, RoleInfoDto>();
            dto.Menus = role.Menus.Select(menu => menu.Map<MenuInfoModel, MenuInfoDto>());
            var roleDto = await _role.UpdateRoleAsync(dto);
            return roleDto.Map<RoleInfoDto, RoleInfoModel>();
        }

        public async Task<RoleInfoModel> AddRoleAsync(RoleInfoModel role)
        {
            var dto = role.Map<RoleInfoModel, RoleInfoDto>();
            dto.Menus = role.Menus.Select(menu => menu.Map<MenuInfoModel, MenuInfoDto>());
            var roleDto = await _role.AddRoleAsync(dto);
            return roleDto.Map<RoleInfoDto, RoleInfoModel>();
        }

        public async Task DeleteAsync(int id)
        {
            await _role.DeleteAsync(id);
        }
    }
}
