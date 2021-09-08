using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Client.Common;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.IDAL.Systems;
using DistantStars.Client.Model.Models;
using DistantStars.Common.DTO.Dtos;
using DistantStars.Common.DTO.Enums;
using DistantStars.Common.DTO.Parameters;

namespace DistantStars.Client.BLL.Systems
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserDAL _userDal;
        private readonly IFileDAL _fileDal;

        public UserBLL(IUserDAL userDal, IFileDAL fileDal)
        {
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
            _fileDal = fileDal ?? throw new ArgumentNullException(nameof(fileDal));
        }

        public async Task<bool> LoginAsync(string userAccount, string password)
        {
            LoginParameter parameter = new LoginParameter { UserAccount = userAccount, Password = password };
            var dto = await _userDal.LoginAsync(parameter);
            if (dto != null)
            {
                var model = dto.Map<UserInfoDto, UserInfoModel>();
                model.Menus = dto.Menus.Select(m => m.Map<MenuInfoDto, MenuInfoModel>());
                Global.CurrentUserInfo = model;
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<UserInfoModel>> GetAllUsersAsync(ParameterBase parameter = null)
        {
            var allUsers = await _userDal.GetAllUsersAsync(parameter);
            return allUsers.Select(dto => dto.Map<UserInfoDto, UserInfoModel>()).ToList();
        }


        public async Task<UserInfoModel> UpdateUserAsync(UserInfoModel model)
        {
            var md5 = await UploadImage(model.UserIconPath);
            if (!string.IsNullOrWhiteSpace(md5)) model.UserIcon = md5;
            var dto = model.Map<UserInfoModel, UserInfoDto>();
            return (await _userDal.UpdateUserAsync(dto)).Map<UserInfoDto, UserInfoModel>();
        }

        public async Task<UserInfoModel> SignUpAsync(UserInfoModel model)
        {
            model.UserIcon = await UploadImage(model.UserIconPath);
            var dto = model.Map<UserInfoModel, UserInfoDto>();
            return (await _userDal.SignUpAsync(dto)).Map<UserInfoDto, UserInfoModel>();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userDal.DeleteUserAsync(id);
        }

        async Task<string> UploadImage(string filePath)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
            {
                var fileDto = await _fileDal.UploadFileAsync(new FileParameter { FileType = FileType.Image }, filePath);
                if (fileDto != null) result = fileDto.MD5;
            }
            return result;
        }

    }
}
