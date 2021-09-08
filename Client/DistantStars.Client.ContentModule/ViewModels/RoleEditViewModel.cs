using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DistantStars.Client.Common;
using DistantStars.Client.Common.Events;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Model.Enums;
using DistantStars.Client.Model.Models;
using Prism.Events;
using Prism.Regions;

namespace DistantStars.Client.ContentModule.ViewModels
{
    public class RoleEditViewModel : EditViewModelBase
    {
        private readonly IRoleBLL _role;
        private readonly IMenuBLL _menu;
        private readonly IEventAggregator _ea;
        private List<MenuInfoModel> AllMenus { get; set; }
        public ObservableCollection<MenuInfoModel> MenuTree { get; set; } = new ObservableCollection<MenuInfoModel>();

        public RoleEditViewModel(IRoleBLL role, IMenuBLL menu, IRegionManager region, IEventAggregator ea) : base(region)
        {
            _role = role;
            _menu = menu;
            _ea = ea;
        }

        
        public override async Task LoadedData()
        {
            var menusTask = _menu.GetAllMenusAsync();
            IEnumerable<MenuInfoModel> roleMenus = null;
            if (ModelState == EditState.Modify)
            {
                if (!(ModelInfo is RoleInfoModel roleInfo)) return;
                roleMenus = await _menu.GetMenusByRoleIdAsync(roleInfo.RoleId);
            }
            AllMenus = (await menusTask).ToList();
            foreach (var model in AllMenus)
            {
                var parent = AllMenus.Find(m => m.MenuId == model.ParentId);
                if (model.ParentId == 0)
                    MenuTree.Add(model);
                parent?.Children.Add(model);
                if (roleMenus == null) continue;
                foreach (var dto in roleMenus)
                {
                    if (!model.Checked && dto.MenuId == model.MenuId) model.Checked = true;
                }
            }

        }

        public override async void Save()
        {
            if (!(ModelInfo is RoleInfoModel roleInfo)) return;
            //var message = View.Show("正在保存...", ShowEnum.ShowLoading);
            roleInfo.Menus = AllMenus.Where(menu => menu.Checked);
            if (ModelState == EditState.Modify)
            {
                var model = await _role.UpdateRoleAsync(roleInfo);
                if (model.RoleId == Global.CurrentUserInfo.RoleId)
                {
                    _ea.GetEvent<CurrentRoleUpdateEvent>().Publish(roleInfo);
                }
            }
            else if (ModelState == EditState.Add)
            {
                await _role.AddRoleAsync(roleInfo);
            }
            //message.Close();
            //View.Show("保存成功");
            GoBack();
        }

    }
}
