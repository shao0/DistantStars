using AutoMapper;
using AutoMapper.Configuration;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Server.Model.Models;

namespace DistantStars.Server.Model.Profiles
{
    public class FileInfoModelProfile : Profile
    {

        public FileInfoModelProfile()
        {
            
            CreateMap<FileInfoModel, FileInfoDto>();
            CreateMap<FileInfoDto, FileInfoModel>();
        }
    }
}
