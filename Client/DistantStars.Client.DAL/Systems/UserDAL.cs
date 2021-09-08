using DistantStars.Client.IDAL.Systems;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DistantStars.Client.Common.Tools.Interfaces;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;
using Newtonsoft.Json;

namespace DistantStars.Client.DAL.Systems
{
    public class UserDAL : WebDataAccess, IUserDAL
    {
        public UserDAL(IConfig config) : base(config)
        {
        }

        public async Task<UserInfoDto> LoginAsync(LoginParameter parameter)
        {
            var content = new StringContent(JsonConvert.SerializeObject(parameter), Encoding.UTF8, "application/json");
            var json = await PostStringAsync("User/Login", content);
            return JsonConvert.DeserializeObject<UserInfoDto>(json);
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync(ParameterBase parameter = null)
        {
            var uri = "User/GetAllUsers";
            if (parameter != null)
            {
                uri = $"{uri}{parameter.GetUriParameter()}";
            }
            var json = await GetStringAsync(uri);
            return JsonConvert.DeserializeObject<IEnumerable<UserInfoDto>>(json);
        }

        public async Task<UserInfoDto> UpdateUserAsync(UserInfoDto dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var json = await PostStringAsync("User/UpdateUser", content);
            return JsonConvert.DeserializeObject<UserInfoDto>(json);
        }

        public async Task<UserInfoDto> SignUpAsync(UserInfoDto dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var json = await PostStringAsync("User/SignUp", content);
            return JsonConvert.DeserializeObject<UserInfoDto>(json);
        }

        public async Task DeleteUserAsync(int id)
        {
            await GetStringAsync($"DeleteUser/{id}");
        }
    }
}
