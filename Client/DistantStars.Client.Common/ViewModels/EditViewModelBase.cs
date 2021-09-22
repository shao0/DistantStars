using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DistantStars.Client.Model.Enums;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DistantStars.Client.Common.ViewModels
{
    public abstract class EditViewModelBase : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        protected FrameworkElement View;

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
            }
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
            _regionManager.RequestNavigate(View.GetType().Name.Replace("EditView", "ContentRegion"), View.GetType().Name.Replace("Edit", string.Empty));
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
        public virtual async void OnNavigatedTo(NavigationContext navigationContext)
        {
            //var message = Application.Current.MainWindow.Show("正在加载...", ShowEnum.ShowLoading);
            ModelState = navigationContext.Parameters.GetValue<EditState>("ModelState");
            ModelInfo = navigationContext.Parameters.GetValue<BindableBase>("Model");
            await LoadedData();
            //message.Close();
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
