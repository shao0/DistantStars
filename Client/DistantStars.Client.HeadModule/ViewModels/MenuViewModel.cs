using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DistantStars.Client.Common;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model;
using DistantStars.Client.Model.Events;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Client.Resource.Fonts;
using DistantStars.Common.DTO.Enums;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace DistantStars.Client.HeadModule.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly IRegionManager _region;
        private readonly IEventAggregator _ea;
        private readonly IMenuBLL _menu;

        private List<MenuInfoModel> Menus { get; set; }

        public MenuViewModel(IRegionManager region, IEventAggregator ea, IMenuBLL menu)
        {
            if (Global.CurrentUserInfo == null) return;
            _region = region;
            _ea = ea;
            _menu = menu;
            Menus = Global.CurrentUserInfo.Menus.ToList();
        }


        #region MenuInfoModel RootMenu 根菜单
        /// <summary>
        /// 根菜单字段
        /// </summary>
        private MenuInfoModel _RootMenu;
        /// <summary>
        /// 根菜单属性
        /// </summary>
        public MenuInfoModel RootMenu
        {
            get => _RootMenu;
            set => SetProperty(ref _RootMenu, value);
        }
        #endregion



        #region bool Show 展示
        /// <summary>
        /// 展示字段
        /// </summary>
        private bool _Show;
        /// <summary>
        /// 展示属性
        /// </summary>
        public bool Show
        {
            get => _Show;
            set => SetProperty(ref _Show, value);
        }
        #endregion


        private void CreateMenuTree()
        {
            RootMenu?.Clear();
            RootMenu = new MenuInfoModel { MenuHeader = "菜单", MenuIcon = IconValues.RootMenuIcon };
            foreach (var model in Menus)
            {
                var parent = Menus.FirstOrDefault(m => m.MenuId == model.ParentId);
                if (model.ParentId == 0)
                    RootMenu.Children.Add(model);
                parent?.Children.Add(model);
                if (model.MenuType == MenuType.Home)
                {
                    Click(model);
                }
            }
        }

        #region LoadedCommand 加载命令
        /// <summary>
        /// 加载命令
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand<object>(Loaded);

        private void Loaded(object obj)
        {
            if (obj is FrameworkElement view)
            {
                _ea.GetEvent<CurrentUserMenuUpdateEvent>().Subscribe(UpdateRole);
                CreateMenuTree();
            }
        }

        private async void UpdateRole(int Id)
        {
            Menus.Clear();
            var menus = await _menu.GetMenusByUserIdAsync(Id);
            foreach (var model in menus)
            {
                model.Clear();
                Menus.Add(model);
            }
            CreateMenuTree();
        }

        #endregion

        #region ClickMenuCommand 选择菜单
        /// <summary>
        /// 选择菜单命令
        /// </summary>
        public ICommand ClickMenuCommand => new DelegateCommand<object>(Click);

        private void Click(object obj)
        {
            if (obj is MenuInfoModel menu)
            {
                if ((menu.Children == null || menu.Children.Count == 0) && !string.IsNullOrWhiteSpace(menu.TargetView))
                {
                    var parameters = new NavigationParameters();
                    parameters.Add("Title", menu.MenuHeader);
                    parameters.Add("MenuType", menu.MenuType);
                    _region.RequestNavigate(RegionNames.MainContent, menu.TargetView, parameters);
                }
                Show = false;
            }
        }
        #endregion
    }
}
