using System;
using System.Windows.Media.Imaging;
using DistantStars.Client.Common.Helpers;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace DistantStars.Client.Start.Models
{
    public class LoginInfoRecord : BindableBase
    {
        #region string UserAccount 用户名称
        /// <summary>
        /// 用户名称 字段
        /// </summary>
        private string _userAccount = "admin";
        /// <summary>
        /// 用户名称 属性
        /// </summary>
        public string UserAccount
        {
            get => _userAccount;
            set => SetProperty(ref _userAccount, value);
        }
        #endregion

        #region string Password 密码
        /// <summary>
        /// 密码 字段
        /// </summary>
        private string _Password = "123456";
        /// <summary>
        /// 密码 属性
        /// </summary>
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);

        }
        #endregion

        #region bool AutoLogin 自动登陆
        /// <summary>
        /// 自动登陆字段
        /// </summary>
        private bool _AutoLogin;
        /// <summary>
        /// 自动登陆属性
        /// </summary>
        public bool AutoLogin
        {
            get => _AutoLogin;
            set
            {
                if (value) RememberPassword = value;
                SetProperty(ref _AutoLogin, value);
            }
        }

        #endregion

        #region bool RememberPassword 记住密码
        /// <summary>
        /// 记住密码字段
        /// </summary>
        private bool _RememberPassword;


        /// <summary>
        /// 记住密码属性
        /// </summary>
        public bool RememberPassword
        {
            get => _RememberPassword;
            set
            {
                if (AutoLogin) value = AutoLogin;
                SetProperty(ref _RememberPassword, value);
            }
        }

        #endregion

        public string UserIconMd5 { get; set; }

        #region BitmapImage UserIcon 用户头像

        /// <summary>
        /// 用户头像属性
        /// </summary>
        private BitmapImage _UserIcon;
        /// <summary>
        /// 用户头像属性
        /// </summary>
        [JsonIgnore]
        public BitmapImage UserIcon
        {
            get => _UserIcon ?? (_UserIcon = UserIconPath.ConvertBitmapImage());
            set => SetProperty(ref _UserIcon, value);
        }

        private string _userIconPath = $"{AppDomain.CurrentDomain.BaseDirectory}/../../Icon.ico";
        public string UserIconPath
        {
            get => _userIconPath;
            set
            {
                _userIconPath = value;
                RaisePropertyChanged(nameof(UserIcon));
            }
        }
        #endregion
    }
}
