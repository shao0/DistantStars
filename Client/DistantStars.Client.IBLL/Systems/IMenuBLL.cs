using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Client.Model.Models;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.IBLL.Systems
{
    public interface IMenuBLL
    {

        Task<IEnumerable<MenuInfoModel>> GetAllMenusAsync(ParameterBase parameter = null);

        Task<IEnumerable<MenuInfoModel>> GetMenusByRoleIdAsync(int id);

        Task<IEnumerable<MenuInfoModel>> GetMenusByUserIdAsync(int id);

        Task<MenuInfoModel> AddMenuAsync(MenuInfoModel menu);

        Task<MenuInfoModel> UpdateMenuAsync(MenuInfoModel menu);

        Task DeleteMenuAsync(int id);
    }
}
