﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DistantStars.Client.Common;
using DistantStars.Client.Common.Events;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model.Models.Systems;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace DistantStars.Client.HeadModule.ViewModels
{
    public class HeadViewModel : BindableBase
    {
        private readonly IFileBLL _file;
        private readonly IEventAggregator _ea;
        private FrameworkElement View;
        public HeadViewModel(IFileBLL file, IEventAggregator ea)
        {
            _file = file;
            _ea = ea;
            //HeadIco = GlobalEntity.CurrentUserInfo.UserIcon.Base64StringToBitmapImage();
        }

        #region BitmapImage HeadIco 属性名称
        /// <summary>
        /// 属性名称字段
        /// </summary>
        private BitmapImage _HeadIco;
        /// <summary>
        /// 属性名称属性
        /// </summary>
        public BitmapImage HeadIco
        {
            get => _HeadIco;
            set => SetProperty(ref _HeadIco, value);
        }
        #endregion

        #region string UserAccount 用户名称
        /// <summary>
        /// 用户名称字段
        /// </summary>
        private string _UserName;
        /// <summary>
        /// 用户名称属性
        /// </summary>
        public string UserName
        {
            get => _UserName;
            set => SetProperty(ref _UserName, value);
        }
        #endregion

        #region LoadedCommand 加载命令
        /// <summary>
        /// 加载命令
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand<object>(Loaded);

        public void Loaded(object obj)
        {
            if (obj is FrameworkElement view)
            {
                View = view;
                _ea.GetEvent<CurrentUserUpdateEvent>().Subscribe(LoadedUserInfo);
                LoadedUserInfo(Global.CurrentUserInfo);
            }
        }

        private void LoadedUserInfo(UserInfoModel obj)
        {
            //var message = View.Show("正在加载...",ShowEnum.ShowLoading);
            UserName = obj.UserName;
            HeadIco = obj.UserIconPath.ConvertBitmapImage();
            //message.Close();
        }

        #endregion



    }
}
