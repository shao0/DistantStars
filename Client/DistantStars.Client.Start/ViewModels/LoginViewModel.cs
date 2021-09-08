using System;
using System.Windows;
using System.Windows.Input;
using DistantStars.Client.IBLL.Systems;
using Prism.Commands;
using Prism.Mvvm;

namespace DistantStars.Client.Start.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IUserBLL _userBll;
        private FrameworkElement _view;
        public LoginViewModel(IUserBLL userBll)
        {
            _userBll = userBll;
        }
        #region string UserName 用户名称
        /// <summary>
        /// 用户名称 字段
        /// </summary>
        private string _UserName = "admin";
        /// <summary>
        /// 用户名称 属性
        /// </summary>
        public string UserName
        {
            get => _UserName;
            set => SetProperty(ref _UserName, value);
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

        #region string ErrorMessage 错误信息
        /// <summary>
        /// 错误信息字段
        /// </summary>
        private string _ErrorMessage;
        /// <summary>
        /// 错误信息属性
        /// </summary>
        public string ErrorMessage
        {
            get => _ErrorMessage;
            set => SetProperty(ref _ErrorMessage, value);
        }
        #endregion

        #region LoadedCommand 加载命令
        /// <summary>
        /// 加载命令
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand<object>(Loaded);

        private void Loaded(object obj)
        {
            if (obj is FrameworkElement view)
            {
                _view = view;
            }
        }

        #endregion


        #region LoginCommand 登录命令
        /// <summary>
        /// 登录命令
        /// </summary>
        public ICommand LoginCommand => new DelegateCommand<object>(Login);

        private async void Login(object obj)
        {
            //IMessage message = null;
            try
            {
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    throw new Exception("用户不能为空!");
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    throw new Exception("密码不能为空!");
                }

                if (obj is Window w)
                {
                    //message = _view.Show("登录中...", ShowEnum.ShowLoading);

                    if (await _userBll.LoginAsync(UserName, Password))
                    {
                        //message.Message = "登录成功";
                        w.DialogResult = true;
                    }
                    else
                    {
                        throw new Exception("用户或密码错误!");
                    }
                }
            }
            catch (Exception e)
            {
                //_view.Show(e.Message);
            }
            //finally
            //{
            //    message?.Close();
            //}
        }

        #endregion




    }
}
