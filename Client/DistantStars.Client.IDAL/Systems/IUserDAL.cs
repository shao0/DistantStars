using DistantStars.Common.DTO.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.IDAL.Systems
{
    public interface IUserDAL
    {
        Task<UserInfoDto> LoginAsync(LoginParameter parameter);
        
        Task<IEnumerable<UserInfoDto>> GetAllUsersAsync(ParameterBase parameter = null);
        
        Task<UserInfoDto> UpdateUserAsync(UserInfoDto dto);

        Task<UserInfoDto> SignUpAsync(UserInfoDto userInfo);

        Task DeleteUserAsync(int id);
    }
}
