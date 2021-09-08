using DistantStars.Server.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistantStars.Server.IService.Systems
{
    public interface IMenuService : IServiceBase
    {
        /// <summary>
        /// 查询所以菜单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MenuInfo>> GetAllMenusAsync();
        /// <summary>
        /// 根据用户id查询菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<MenuInfo>> GetMenusByUserIdAsync(int userId);
        /// <summary>
        /// 根据角色id查询菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<MenuInfo>> GetMenusByRoleIdAsync(int roleId);

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="menu"></param>
        Task<MenuInfo> AddMenuAsync(MenuInfo menu);

    }
}
