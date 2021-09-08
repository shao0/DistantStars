using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Server.Model.Models;

namespace DistantStars.Server.Model.Profiles
{
    public class RoleInfoProfile : Profile
    {
        public RoleInfoProfile()
        {
            CreateMap<RoleInfo, RoleInfoDto>().ForMember(dest => dest.StateName, member => member.MapFrom(str => str.State == 1 ? "可用" : "禁用"));
            CreateMap<RoleInfoDto, RoleInfo>().ForMember(
                dest => dest.RoleMenus,
                member => member.MapFrom(dto => dto.Menus.Select(menu => new RoleMenu
                {
                    MenuId = menu.MenuId,
                    RoleId = dto.RoleId,
                })));
        }
    }
}
