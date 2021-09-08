using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model.Enums;
using DistantStars.Client.Model.Models;
using Prism.Commands;
using Prism.Regions;

namespace DistantStars.Client.ContentModule.ViewModels
{
    public class MenuEditViewModel : EditViewModelBase
    {
        private readonly IMenuBLL _menu;
        public MenuEditViewModel(IRegionManager region, IMenuBLL menu) : base(region)
        {
            _menu = menu;
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
            Menus.Clear();
            Menus.Add(new MenuInfoModel { MenuHeader = "一级菜单" });
            var allMenus = await _menu.GetAllMenusAsync();
            foreach (var model in allMenus.Where(m => m.MenuId != menuInfo.MenuId))
            {
                Menus.Add(model);
            }
        }

        public override async void Save()
        {
            if (!(ModelInfo is MenuInfoModel menuInfo)) return;
            //var message = View.Show("正在保存...", ShowEnum.ShowLoading);
            if (ModelState == EditState.Modify)
            {
                await _menu.UpdateMenuAsync(menuInfo);
            }
            else if (ModelState == EditState.Add)
            {
                await _menu.AddMenuAsync(menuInfo);
            }
            //message.Close();
            //View.Show("保存成功");
            GoBack();
        }



        #region ClickIconCommand 点击图标命令
        /// <summary>
        /// 点击图标命令
        /// </summary>
        public ICommand ClickIconCommand => new DelegateCommand<object>(ClickIcon);

        private void ClickIcon(object obj)
        {
            if (obj is string v&& ModelInfo is MenuInfoModel menuInfo)
            {
                menuInfo.MenuIcon = v;
                ShowIcon = false;
            }
        }

        #endregion


    }
}
