using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Parameters;
using DistantStars.Server.Model.Models;

namespace DistantStars.Server.IService.Systems
{
    public interface IRoleService : IServiceBase
    {
        Task<IEnumerable<RoleInfo>> GetRolesAsync(ParameterBase parameter);
        Task<RoleInfo> AddRoleAsync(RoleInfo role);
        Task<RoleInfo> UpdateRoleAsync(RoleInfo role);
        Task DeleteRoleAsync(int id);
    }
}
