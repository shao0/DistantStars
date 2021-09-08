using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.IDAL.Systems
{
    public interface IRoleDAL
    {
        Task<IEnumerable<RoleInfoDto>> GetAllRolesAsync(RoleParameter parameter = null);
        Task<RoleInfoDto> UpdateRoleAsync(RoleInfoDto role);
        Task<RoleInfoDto> AddRoleAsync(RoleInfoDto role);
        Task DeleteAsync(int id);
    }
}
