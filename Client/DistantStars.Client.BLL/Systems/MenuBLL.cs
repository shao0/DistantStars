using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.IDAL.Systems;
using DistantStars.Client.Model.Models;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.BLL.Systems
{
    public class MenuBLL : IMenuBLL
    {
        private readonly IMenuDAL _menu;

        public MenuBLL(IMenuDAL menu)
        {
            _menu = menu;
        }
        public async Task<IEnumerable<MenuInfoModel>> GetAllMenusAsync(ParameterBase parameter = null)
        {
            return (await _menu.GetAllMenusAsync(parameter)).Select(dto => dto.Map<MenuInfoDto, MenuInfoModel>());
        }

        public async Task<IEnumerable<MenuInfoModel>> GetMenusByRoleIdAsync(int id)
        {
            return (await _menu.GetMenusByRoleIdAsync(id)).Select(dto => dto.Map<MenuInfoDto, MenuInfoModel>());
        }

        public async Task<IEnumerable<MenuInfoModel>> GetMenusByUserIdAsync(int id)
        {
            return (await _menu.GetMenusByUserIdAsync(id)).Select(dto => dto.Map<MenuInfoDto, MenuInfoModel>());
        }

        public async Task<MenuInfoModel> AddMenuAsync(MenuInfoModel menu)
        {
            MenuInfoDto dto = menu.Map<MenuInfoModel, MenuInfoDto>();
            return (await _menu.AddMenuAsync(dto)).Map<MenuInfoDto, MenuInfoModel>();
        }

        public async Task<MenuInfoModel> UpdateMenuAsync(MenuInfoModel menu)
        {
            MenuInfoDto dto = menu.Map<MenuInfoModel, MenuInfoDto>();
            return (await _menu.UpdateMenuAsync(dto)).Map<MenuInfoDto, MenuInfoModel>();
        }

        public async Task DeleteMenuAsync(int id)
        {
            await _menu.DeleteMenuAsync(id);
        }
    }
}
