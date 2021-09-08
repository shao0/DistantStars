using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Client.Model.Models;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.IBLL.Systems
{
    public interface IRoleBLL
    {
        Task<IEnumerable<RoleInfoModel>> GetAllRolesAsync(RoleParameter parameter = null);
        Task<RoleInfoModel> UpdateRoleAsync(RoleInfoModel role);
        Task<RoleInfoModel> AddRoleAsync(RoleInfoModel role);
        Task DeleteAsync(int id);
    }
}
