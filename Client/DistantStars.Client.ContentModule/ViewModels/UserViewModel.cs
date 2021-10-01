using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DistantStars.Client.Common;
using DistantStars.Client.Common.ViewModels;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model.Enums;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Client.Resource.Data.Enum;
using DistantStars.Client.Resource.Helpers;
using DistantStars.Common.DTO.Parameters;
using Prism.Commands;
using Prism.Regions;

namespace DistantStars.Client.ContentModule.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IUserBLL _user;
        private readonly IRegionManager _region;

        public ObservableCollection<UserInfoModel> Users { get; set; } = new ObservableCollection<UserInfoModel>();

        public UserViewModel(IUserBLL user, IRegionManager region)
        {
            _user = user;
            _region = region;
        }

        #region  加载

        public override async void LoadedContinue()
        {
            var message = _View.Loading("正在加载...");
            await LoadedData();
            message.Close();
        }

        private async Task LoadedData(string search = null)
        {
            ParameterBase parameter = string.IsNullOrWhiteSpace(search) ? null : new ParameterBase { Search = search };
            Users.Clear();
            foreach (var model in await _user.GetAllUsersAsync(parameter))
            {
                Users.Add(model);
            }
        }
        #endregion

        #region EditCommand 编辑命令
        /// <summary>
        /// 编辑命令
        /// </summary>
        public ICommand EditCommand => new DelegateCommand<object>(Edit);

        private void Edit(object obj)
        {
            if (obj is UserInfoModel model)
            {
                var parameters = new NavigationParameters();
                parameters.Add("Model", model);
                parameters.Add("ModelState", EditState.Modify);
                GoEdit(parameters);
            }
        }

        #endregion

        #region AddCommand 新增命令
        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand AddCommand => new DelegateCommand(Add);

        private void Add()
        {
            var parameters = new NavigationParameters();
            parameters.Add("Model", new UserInfoModel());
            parameters.Add("ModelState", EditState.Add);
            GoEdit(parameters);
        }

        #endregion

        #region QueryCommand 查询命令
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand QueryCommand => new DelegateCommand<object>(Query);

        private async void Query(object obj)
        {
            if (obj is string search)
            {
                var message = _View.Loading("正在查询...");
                await LoadedData(search);
                message.Close();
            }
        }

        #endregion

        #region DeletedCommand 删除命令
        /// <summary>
        /// 删除命令
        /// </summary>
        public ICommand DeletedCommand => new DelegateCommand<object>(Deleted);

        private async void Deleted(object obj)
        {
            if (obj is int menuId)
            {
                var message = _View.Loading("正在删除...");
                await _user.DeleteUserAsync(menuId);
                await LoadedData();
                message.Close();
                _View.Show("删除成功", ShowType.Success);
            }

        }
        #endregion


        private void GoEdit(NavigationParameters parameters)
        {
            _region.RequestNavigate(RegionNames.UserContent, GetType().Name.Replace("ViewModel","EditView"), parameters);
        }
    }
}
