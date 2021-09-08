using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DistantStars.Client.Common;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model;
using DistantStars.Client.Model.Events;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Common.DTO.Enums;
using DistantStars.Common.DTO.Parameters;
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

                if (Global.CurrentUserInfo != null)
                    LoadedUserInfo(Global.CurrentUserInfo);
            }
        }

        private async void LoadedUserInfo(UserInfoModel obj)
        {
            //var message = View.Show("正在加载...",ShowEnum.ShowLoading);
            UserName = obj.UserName;
            var bytes = await _file.DownloadFileAsync(new FileParameter { FileType = FileType.Image, MD5 = obj.UserIcon });
            if (bytes.Length > 0)
                HeadIco = bytes.ConvertBitmapImage();
            //message.Close();
        }

        #endregion



    }
}
