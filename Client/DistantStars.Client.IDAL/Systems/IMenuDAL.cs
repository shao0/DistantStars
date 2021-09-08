using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.IDAL.Systems
{
    public interface IMenuDAL
    {
        Task<IEnumerable<MenuInfoDto>> GetAllMenusAsync(ParameterBase parameter = null);

        Task<IEnumerable<MenuInfoDto>> GetMenusByRoleIdAsync(int id);

        Task<IEnumerable<MenuInfoDto>> GetMenusByUserIdAsync(int id);

        Task<MenuInfoDto> AddMenuAsync(MenuInfoDto menu);

        Task<MenuInfoDto> UpdateMenuAsync(MenuInfoDto menu);

        Task DeleteMenuAsync(int id);
    }
}
