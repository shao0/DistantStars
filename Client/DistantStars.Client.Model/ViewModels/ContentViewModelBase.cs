using System.Windows.Input;
using DistantStars.Client.Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DistantStars.Client.Model.ViewModels
{
    public class ContentViewModelBase : BindableBase, INavigationAware
    {
        protected readonly IRegionManager _region;

        public ContentViewModelBase(IRegionManager region)
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

        #region CloseCommand 关闭命令
        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand => new DelegateCommand<object>(Close);

        private void Close(object obj)
        {
            _region.Regions[RegionNames.MainContent].Remove(obj);
        }

        #endregion


        #region LoadedCommand 加载命令
        /// <summary>
        /// 加载命令
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand<object>(Loaded);

        private void Loaded(object obj)
        {
            if (obj != null)
            {
                _region.RequestNavigate($"{obj.GetType().Name.Replace("MainView", string.Empty)}ContentRegion", obj.GetType().Name.Replace("Main", string.Empty));
            }
        }

        #endregion

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = navigationContext.Parameters.GetValue<string>("Title");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {

            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
