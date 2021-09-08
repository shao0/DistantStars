using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Parameters;
using DistantStars.Server.DBContext;
using DistantStars.Server.IService.Systems;
using DistantStars.Server.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace DistantStars.Server.Service.Systems
{
    public class RoleService : ServiceBase, IRoleService
    {
        public RoleService(EFCoreContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RoleInfo>> GetRolesAsync(ParameterBase parameter)
        {
            IQueryable<RoleInfo> where = _context.RoleInfo;
            if (!string.IsNullOrWhiteSpace(parameter.Search))
            {
                parameter.Search = parameter.Search.Trim();
                where = where.Where(role => role.RoleName.Contains(parameter.Search));
            }
            if (parameter.GetParameter<string>("RoleName", out var roleName) && !string.IsNullOrWhiteSpace(roleName))
            {
                where = where.Where(role => role.RoleName == roleName);
            }
            return await where.ToListAsync();
        }

        public async Task<RoleInfo> AddRoleAsync(RoleInfo role)
        {
            role.State = 1;
            await _context.RoleInfo.AddAsync(role);
            await _context.RoleMenu.AddRangeAsync(role.RoleMenus);
            await CommitAsync();
            return role;
        }

        public async Task<RoleInfo> UpdateRoleAsync(RoleInfo role)
        {
            foreach (var roleMenu in _context.RoleMenu.Where(r => r.RoleId == role.RoleId))
            {
                _context.Entry(roleMenu).State = EntityState.Deleted;

            }
            foreach (var roleMenu in role.RoleMenus)
            {
                _context.Entry(roleMenu).State = EntityState.Added;
            }
            _context.RoleInfo.Update(role);
            await CommitAsync();
            return role;
        }

        public async Task DeleteRoleAsync(int id)
        {
            foreach (var roleMenu in _context.RoleMenu.Where(r => r.RoleId == id))
            {
                _context.Entry(roleMenu).State = EntityState.Deleted;
            }
            var role = await FindAsync<RoleInfo>(id);
            _context.Entry(role).State = EntityState.Deleted;
            await CommitAsync();
        }
    }
}
