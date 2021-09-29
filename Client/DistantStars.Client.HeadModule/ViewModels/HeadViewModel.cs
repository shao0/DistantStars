using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DistantStars.Client.Common;
using DistantStars.Client.Common.Events;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.Common.ViewModels;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Client.Resource.Helpers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace DistantStars.Client.HeadModule.ViewModels
{
    public class HeadViewModel : ViewModelBase
    {
        private readonly IEventAggregator _ea;
        public HeadViewModel( IEventAggregator ea)
        {
            _ea = ea;
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

        public override void LoadedContinue()
        {
            base.LoadedContinue();
            _ea.GetEvent<CurrentUserUpdateEvent>().Subscribe(LoadedUserInfo);
            LoadedUserInfo(Global.CurrentUserInfo);
        }

        private void LoadedUserInfo(UserInfoModel obj)
        {
            var message = _View.Show("正在加载头像...", ShowEnum.ShowLoading);
            UserName = obj.UserName;
            HeadIco = obj.UserIconPath.ConvertBitmapImage();
            message.Close();
        }

        #endregion



    }
}
