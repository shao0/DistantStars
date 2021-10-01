using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DistantStars.Client.Common;
using DistantStars.Client.Common.Events;
using DistantStars.Client.Common.ViewModels;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model;
using DistantStars.Client.Model.Enums;
using DistantStars.Client.Model.Models.Systems;
using DistantStars.Client.Resource.Data.Enum;
using DistantStars.Client.Resource.Fonts;
using DistantStars.Client.Resource.Helpers;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace DistantStars.Client.ContentModule.ViewModels
{
    public class MenuEditViewModel : EditViewModelBase
    {
        private readonly IMenuBLL _menu;
        private readonly IEventAggregator _ea;

        public MenuEditViewModel(IRegionManager region, IMenuBLL menu, IEventAggregator ea) : base(region)
        {
            _menu = menu;
            _ea = ea;
        }

        public ObservableCollection<MenuInfoModel> Menus { get; set; } = new ObservableCollection<MenuInfoModel>();


        #region bool ShowIcon 展示所有图标
        /// <summary>
        /// 展示所有图标字段
        /// </summary>
        private bool _ShowIcon;
        /// <summary>
        /// 展示所有图标属性
        /// </summary>
        public bool ShowIcon
        {
            get => _ShowIcon;
            set => SetProperty(ref _ShowIcon, value);
        }
        #endregion

        public override async Task LoadedData()
        {
            if (!(ModelInfo is MenuInfoModel menuInfo)) return;
            var message = _View.Loading("正在加载...");
            Menus.Clear();
            Menus.Add(new MenuInfoModel { MenuHeader = "一级菜单" });
            var allMenus = await _menu.GetAllMenusAsync();
            foreach (var model in allMenus.Where(m => m.MenuId != menuInfo.MenuId))
            {
                Menus.Add(model);
            }
            message.Close();
        }

        public override async void Save()
        {
            if (!(ModelInfo is MenuInfoModel menuInfo)) return;
            var message = _View.Loading("正在保存...");
            if (ModelState == EditState.Modify)
            {
                await _menu.UpdateMenuAsync(menuInfo);
            }
            else if (ModelState == EditState.Add)
            {
                await _menu.AddMenuAsync(menuInfo);
            }
            _ea.GetEvent<CurrentUserMenuUpdateEvent>().Publish(Global.CurrentUserInfo.Id);

            message.Close();
            _View.Show("保存成功",ShowType.Success);
            GoBack();
        }



        #region ClickIconCommand 点击图标命令
        /// <summary>
        /// 点击图标命令
        /// </summary>
        public ICommand ClickIconCommand => new DelegateCommand<object>(ClickIcon);

        private void ClickIcon(object obj)
        {
            if (obj is string v && ModelInfo is MenuInfoModel menuInfo)
            {
                menuInfo.MenuIcon = v.UnicodeToFontValueConvert();
                ShowIcon = false;
            }
        }

        #endregion


    }
}
