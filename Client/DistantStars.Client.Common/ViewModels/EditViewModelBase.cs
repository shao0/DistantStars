using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DistantStars.Client.Model.Enums;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DistantStars.Client.Common.ViewModels
{
    public abstract class EditViewModelBase : ViewModelBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;


        protected EditViewModelBase(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #region ModelState ModelState 编辑状态
        /// <summary>
        /// 编辑状态字段
        /// </summary>
        private EditState _modelState;
        /// <summary>
        /// 编辑状态属性
        /// </summary>
        public EditState ModelState
        {
            get => _modelState;
            set
            {
                _modelState = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ModelInfo ModelInfo 实例
        /// <summary>
        /// 实例字段
        /// </summary>
        private BindableBase _ModelInfo;
        /// <summary>
        /// 实例属性
        /// </summary>
        public BindableBase ModelInfo
        {
            get => _ModelInfo;
            set => SetProperty(ref _ModelInfo, value);
        }
        #endregion

        #region  加载

        public override async void LoadedContinue()
        {
            await LoadedData();
        }

        /// <summary>
        /// 加載數據
        /// </summary>
        /// <returns></returns>
        public abstract Task LoadedData();
        #endregion

        #region GoBackCommand 返回命令
        /// <summary>
        /// 返回命令
        /// </summary>
        public ICommand GoBackCommand => new DelegateCommand(GoBack);


        protected void GoBack()
        {
            _regionManager.RequestNavigate(_View.GetType().Name.Replace("EditView", "ContentRegion"), _View.GetType().Name.Replace("Edit", string.Empty));
        }

        #endregion

        #region SaveCommand 保存命令
        /// <summary>
        /// 保存命令
        /// </summary>
        public ICommand SaveCommand => new DelegateCommand(Save);
        /// <summary>
        /// SaveCommand命令調用
        /// </summary>
        public abstract void Save();
        #endregion

        /// <summary>
        /// 进入页面
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            ModelState = navigationContext.Parameters.GetValue<EditState>("ModelState");
            ModelInfo = navigationContext.Parameters.GetValue<BindableBase>("Model");
        }


        /// <summary>
        /// True则重用该View实例，False则每一次导航到该页面都会实例化一次
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
        /// <summary>
        /// 离开页面
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
