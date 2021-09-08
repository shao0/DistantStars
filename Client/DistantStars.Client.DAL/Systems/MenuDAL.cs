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
    public class MenuDAL : WebDataAccess, IMenuDAL
    {
        public MenuDAL(IConfig config) : base(config)
        {
        }
        public async Task<IEnumerable<MenuInfoDto>> GetAllMenusAsync(ParameterBase parameter = null)
        {
            var uri = "Menu/GetAllMenus";
            uri = parameter != null ? $"Menu/GetAllMenus{parameter.GetUriParameter()}" : uri;
            var json = await GetStringAsync(uri);
            return JsonConvert.DeserializeObject<IEnumerable<MenuInfoDto>>(json);
        }

        public async Task<IEnumerable<MenuInfoDto>> GetMenusByRoleIdAsync(int id)
        {
            var json = await GetStringAsync($"Menu/GetMenusByRoleId/{id}");
            return JsonConvert.DeserializeObject<IEnumerable<MenuInfoDto>>(json);
        }

        public async Task<IEnumerable<MenuInfoDto>> GetMenusByUserIdAsync(int id)
        {
            var json = await GetStringAsync($"Menu/GetMenusByUserId/{id}");
            return JsonConvert.DeserializeObject<IEnumerable<MenuInfoDto>>(json);
        }

        public async Task<MenuInfoDto> AddMenuAsync(MenuInfoDto menu)
        {
            var content = new StringContent(JsonConvert.SerializeObject(menu), Encoding.UTF8, "application/json");
            var json = await PostStringAsync("Menu/AddMenu", content);
            return JsonConvert.DeserializeObject<MenuInfoDto>(json);
        }

        public async Task<MenuInfoDto> UpdateMenuAsync(MenuInfoDto menu)
        {
            var content = new StringContent(JsonConvert.SerializeObject(menu), Encoding.UTF8, "application/json");
            var json = await PostStringAsync("Menu/UpdateMenu", content);
            return JsonConvert.DeserializeObject<MenuInfoDto>(json);
        }

        public async Task DeleteMenuAsync(int id)
        {
            await GetStringAsync($"Menu/DeleteMenu/{id}");
        }
    }
}
