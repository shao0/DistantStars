using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DistantStars.Client.Common.Events;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.Common.ViewModels;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model.Enums;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Common.DTO.Enums;
using DistantStars.Common.DTO.Parameters;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace DistantStars.Client.ContentModule.ViewModels
{
    public class UserEditViewModel : EditViewModelBase
    {
        private readonly IUserBLL _user;
        private readonly IRoleBLL _role;
        private readonly IFileBLL _file;
        private readonly IEventAggregator _ea;
        public UserEditViewModel(IRegionManager region, IUserBLL user, IRoleBLL role, IFileBLL file, IEventAggregator ea) : base(region)
        {
            _user = user;
            _role = role;
            _file = file;
            _ea = ea;
        }

        public ObservableCollection<RoleInfoModel> Roles { get; set; } = new ObservableCollection<RoleInfoModel>();

        #region string Password 密码
        /// <summary>
        /// 密码字段
        /// </summary>
        private string _Password;
        /// <summary>
        /// 密码属性
        /// </summary>
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);
        }
        #endregion

        #region BitmapImage HeadImage 头像图片
        /// <summary>
        /// 头像图片字段
        /// </summary>
        private BitmapImage _HeadImage;
        /// <summary>
        /// 头像图片属性
        /// </summary>
        public BitmapImage HeadImage
        {
            get => _HeadImage;
            set => SetProperty(ref _HeadImage, value);
        }
        #endregion


        public override async Task LoadedData()
        {
            var rolesTask = _role.GetAllRolesAsync();
            if (ModelState ==EditState.Modify)
            {
                if (!(ModelInfo is UserInfoModel userInfo)) return;
                var bytes = await _file.DownloadFileAsync(new FileParameter { FileType = FileType.Image, MD5 = userInfo.UserIcon });
                HeadImage = bytes.ConvertBitmapImage();
            }
            Roles.Clear();
            foreach (var role in await rolesTask)
            {
                Roles.Add(role);
            }
        }


        public override async void Save()
        {
            if (!(ModelInfo is UserInfoModel userInfo)) return;
            //var message = View.Show("正在保存...", ShowEnum.ShowLoading);
            if (ModelState == EditState.Modify)
            {
                if (userInfo.ModifyPassword) userInfo.UserPassword = Password;
                var model = await _user.UpdateUserAsync(userInfo);
                if (model != null)
                {
                    _ea.GetEvent<CurrentUserUpdateEvent>().Publish(userInfo);
                }
            }
            else if (ModelState == EditState.Add)
            {
                userInfo.UserPassword = Password;
                await _user.SignUpAsync(userInfo);
            }
            //message.Close();
            //View.Show("保存成功");
            GoBack();
        }


        #region SelectedCommand 选择图片命令
        /// <summary>
        /// 选择图片命令
        /// </summary>
        public ICommand SelectedCommand => new DelegateCommand(Selected);

        private void Selected()
        {
            if (!(ModelInfo is UserInfoModel userInfo)) return;
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                userInfo.UserIconPath = fileDialog.FileName;
                HeadImage = userInfo.UserIconPath.ConvertBitmapImage();
            }
        }

        #endregion


    }
}
