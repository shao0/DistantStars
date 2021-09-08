using System.Linq;
using AutoMapper;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Server.Model.Models;

namespace DistantStars.Server.Model.Profiles
{
    public class MenuInfoProfile : Profile
    {
        public MenuInfoProfile()
        {
            CreateMap<MenuInfo, MenuInfoDto>().ForMember(dest => dest.StateName, member => member.MapFrom(str => str.State == 1 ? "可用" : "禁用"));
            CreateMap<MenuInfoDto, MenuInfo>().ForMember(
                dest => dest.RoleMenus,
                member => member.MapFrom(menu => menu.Roles.Select(role => new RoleMenu
                {
                    MenuId = menu.MenuId,
                    RoleId = role.RoleId,
                })));
        }
    }
}
