using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Client.Model.Models;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.IBLL.Systems
{
    public interface IUserBLL
    {
        Task<bool> LoginAsync(string userAccount, string password);

        Task<IEnumerable<UserInfoModel>> GetAllUsersAsync(ParameterBase parameter = null);

        Task<UserInfoModel> UpdateUserAsync(UserInfoModel dto);

        Task<UserInfoModel> SignUpAsync(UserInfoModel userInfo);

        Task DeleteUserAsync(int id);
    }
}
