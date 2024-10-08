﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DistantStars.Client.Common;
using DistantStars.Client.Common.ViewModels;
using DistantStars.Client.ContentModule.Views;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model.Enums;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Client.Resource.Data.Enum;
using DistantStars.Client.Resource.Helpers;
using DistantStars.Common.DTO.Parameters;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DistantStars.Client.ContentModule.ViewModels
{
    public class RoleViewModel : ViewModelBase
    {

        private readonly IRoleBLL _role;
        private readonly IRegionManager _region;

        public RoleViewModel(IRoleBLL role, IRegionManager region)
        {
            _role = role;
            _region = region;
        }

        public ObservableCollection<RoleInfoModel> RoleInfos { get; set; } = new ObservableCollection<RoleInfoModel>();


        #region  加载

        public override async void LoadedContinue()
        {
            await LoadedData();
        }

        private async Task LoadedData(string search = null)
        {
            var message = _View.Loading("正在加载...");
            RoleParameter parameter = string.IsNullOrWhiteSpace(search) ? null : new RoleParameter { Search = search };
            RoleInfos.Clear();
            var allRoles = await _role.GetAllRolesAsync(parameter);
            foreach (var model in allRoles)
            {
                RoleInfos.Add(model);
            }
            message.Close();
        }
        #endregion

        #region EditCommand 编辑命令
        /// <summary>
        /// 编辑命令
        /// </summary>
        public ICommand EditCommand => new DelegateCommand<object>(Edit);

        private void Edit(object obj)
        {
            if (obj is RoleInfoModel role)
            {
                var parameters = new NavigationParameters();
                parameters.Add("Model", role);
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
            parameters.Add("Model", new RoleInfoModel());
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
                await LoadedData(search);
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
            if (obj is int roleId)
            {
                var message = _View.Loading("正在删除...");
                await _role.DeleteAsync(roleId);
                await LoadedData();
                message.Close();
                _View.Show("删除成功", ShowType.Success);
            }

        }
        #endregion


        private void GoEdit(NavigationParameters parameters)
        {
            _region.RequestNavigate(RegionNames.RoleContent, nameof(RoleEditView), parameters);
        }

    }
}
