using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;
using DistantStars.Server.DBContext;
using DistantStars.Server.IService.Systems;
using DistantStars.Server.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace DistantStars.Server.Service.Systems
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(EFCoreContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsers(UserParameter parameter)
        {
            IQueryable<UserInfo> query = _context.UserInfo;
            if (!string.IsNullOrWhiteSpace(parameter.Search))
            {
                parameter.Search = parameter.Search.Trim();
                var b = int.TryParse(parameter.Search, out var id);
                var userInfos = query
                    .Where(u => u.UserName.Contains(parameter.Search)
                                || u.UserAccount.Contains(parameter.Search)
                                || (b && u.Id == id));
                return await userInfos.ToListAsync();
            }
            return await query
                .Where(u =>
                    (parameter.Id <= 0 || u.Id == parameter.Id)
                    && (string.IsNullOrWhiteSpace(parameter.UserName) || u.UserName == parameter.UserName)
                    && (string.IsNullOrWhiteSpace(parameter.UserAccount) || u.UserAccount == parameter.UserAccount))
                .ToListAsync();
        }

        public Task<UserInfoDto> UpdateUserAsync(UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(userInfo.UserPassword))
            {
                //_context.UserInfo.
            }

            return null;
        }
    }
}
