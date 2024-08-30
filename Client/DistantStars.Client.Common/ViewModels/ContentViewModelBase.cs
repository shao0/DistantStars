using System.Windows.Input;
using DistantStars.Common.DTO.Enums;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DistantStars.Client.Common.ViewModels
{
    public abstract class ContentViewModelBase : ViewModelBase, INavigationAware
    {
        protected readonly IRegionManager _region;

        protected ContentViewModelBase(IRegionManager region)
        {
            _region = region;
        }
        #region string Title 文字
        /// <summary>
        /// 文字字段
        /// </summary>
        protected string _title = "标题";
        /// <summary>
        /// 文字属性
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        #endregion

        #region MenuType MenuType 菜单属性
        /// <summary>
        /// 菜单属性字段
        /// </summary>
        private MenuType _MenuType;
        /// <summary>
        /// 菜单属性属性
        /// </summary>
        public MenuType MenuType
        {
            get => _MenuType;
            set => SetProperty(ref _MenuType, value);
        }
        #endregion

        #region CloseCommand 关闭命令
        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand => new DelegateCommand<object>(Close);

        private void Close(object obj)
        {
            Close();
            _region.Regions[RegionNames.MainContent].Remove(obj);
        }
        /// <summary>
        /// 关闭释放
        /// </summary>
        public abstract void Close();

        #endregion



        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = navigationContext.Parameters.GetValue<string>("Title");
            _MenuType = navigationContext.Parameters.GetValue<MenuType>("MenuType");
            var name = GetType().Name;
            var regionName = $"{name.Replace("MainViewModel", string.Empty)}ContentRegion";
            if (!_region.Regions.ContainsRegionWithName(regionName)) return;
            var sourceName = name.Replace("Main", string.Empty).Replace("ViewModel", "View");
            _region.RequestNavigate(regionName, sourceName);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.ToString() != _View?.GetType().Name) Close();
        }
    }
}
