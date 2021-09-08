using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;
using DistantStars.Server.Model.Models;

namespace DistantStars.Server.IService.Systems
{
    public interface IUserService:IServiceBase
    {
        Task<IEnumerable<UserInfo>> GetAllUsers(UserParameter parameter);
        Task<UserInfoDto> UpdateUserAsync(UserInfo userInfo);
    }
}
