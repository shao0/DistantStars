using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DistantStars.Client.Common.Tools.Interfaces;
using DistantStars.Client.IDAL.Systems;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;
using Newtonsoft.Json;

namespace DistantStars.Client.DAL.Systems
{
    public class RoleDAL : WebDataAccess, IRoleDAL
    {
        public RoleDAL(IConfig config) : base(config)
        {
        }
        public async Task<IEnumerable<RoleInfoDto>> GetAllRolesAsync(RoleParameter parameter = null)
        {
            var uri = "Role/GetAllRoles";
            uri = parameter != null ? $"Role/GetAllRoles{parameter.GetUriParameter()}" : uri;
            var json = await GetStringAsync(uri);
            return JsonConvert.DeserializeObject<IEnumerable<RoleInfoDto>>(json);
        }

        public async Task<RoleInfoDto> UpdateRoleAsync(RoleInfoDto role)
        {
            var content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");
            var json = await PostStringAsync("Role/UpdateRole", content);
            return JsonConvert.DeserializeObject<RoleInfoDto>(json);
        }

        public async Task<RoleInfoDto> AddRoleAsync(RoleInfoDto role)
        {
            var content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");
            var json = await PostStringAsync("Role/AddRole", content);
            return JsonConvert.DeserializeObject<RoleInfoDto>(json);
        }

        public async Task DeleteAsync(int id)
        {
            await GetStringAsync($"Role/Delete/{id}");
        }

    }
}
