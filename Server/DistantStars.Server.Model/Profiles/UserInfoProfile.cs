using AutoMapper;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Server.Model.Models;

namespace DistantStars.Server.Model.Profiles
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfo, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfo>();
        }
    }
}
