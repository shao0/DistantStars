using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Server.DBContext;
using DistantStars.Server.IService.Systems;
using DistantStars.Server.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace DistantStars.Server.Service.Systems
{
    public class MenuService : ServiceBase, IMenuService
    {
        public MenuService(EFCoreContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MenuInfo>> GetAllMenusAsync()
        {
            return await _context.MenuInfo.Where(menu => menu.State == 1).ToListAsync();
        }

        public async Task<IEnumerable<MenuInfo>> GetMenusByUserIdAsync(int userId)
        {
            var roleId = _context.UserInfo.First(user => user.Id == userId).RoleId;
            var query = _context.MenuInfo
                .Where(menu => menu.State == 1)
                .Join(_context.RoleMenu
                        .Where(roleMenu => roleId == roleMenu.RoleId),
                 menu => menu.MenuId,
                 roleMenu => roleMenu.MenuId,
                 (menu, roleMenu) => menu);

            return await query.Distinct().ToListAsync();
        }

        public async Task<IEnumerable<MenuInfo>> GetMenusByRoleIdAsync(int roleId)
        {
            var roleIds = _context.RoleInfo
                .Where(role => role.RoleId == roleId && role.State == 1)
                .Select(role => role.RoleId);

            var query = _context.MenuInfo
                .Where(menu => menu.State == 1)
                .Join(_context.RoleMenu
                        .Where(roleMenu => roleIds
                            .Contains(roleMenu.RoleId)),
                    menu => menu.MenuId,
                    roleMenu => roleMenu.MenuId,
                    (menu, roleMenu) => menu);
            return await query.Distinct().ToListAsync();
        }


        public async Task<MenuInfo> AddMenuAsync(MenuInfo menu)
        {
            if (_context.Set<MenuInfo>().Any())
                menu.Index = _context.MenuInfo.Max(menu => menu.Index) + 1;
            menu.State = 1;
            return await InsertAsync(menu);
        }
    }
}
