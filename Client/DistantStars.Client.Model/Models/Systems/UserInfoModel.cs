using System.Collections.Generic;
using Prism.Mvvm;

namespace DistantStars.Client.Model.Models.Systems
{
    public class UserInfoModel : BindableBase
    {
        public int Id { get; set; }

        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string UserIcon { get; set; }


        #region int RoleId 角色id
        /// <summary>
        /// 角色id字段
        /// </summary>
        private int _RoleId;
        /// <summary>
        /// 角色id属性
        /// </summary>
        public int RoleId
        {
            get => _RoleId;
            set => SetProperty(ref _RoleId, value);
        }
        #endregion


        #region bool ModifyPassword 修改密码
        /// <summary>
        /// 修改密码字段
        /// </summary>
        private bool _ModifyPassword;
        /// <summary>
        /// 修改密码属性
        /// </summary>
        public bool ModifyPassword
        {
            get => _ModifyPassword;
            set => SetProperty(ref _ModifyPassword, value);
        }
        #endregion

        #region string UserIconPath 头像
        /// <summary>
        /// 头像字段
        /// </summary>
        private string _UserIconPath;
        /// <summary>
        /// 头像属性
        /// </summary>
        public string UserIconPath
        {
            get => _UserIconPath;
            set => SetProperty(ref _UserIconPath, value);
        }
        #endregion


        public IEnumerable<MenuInfoModel> Menus { get; set; }
    }
}
