using DistantStars.Client.BLL.Systems;
using DistantStars.Client.ContentModule.Views;
using DistantStars.Client.DAL.Systems;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.IDAL.Systems;
using Prism.Ioc;
using Prism.Modularity;

namespace DistantStars.Client.ContentModule
{
    public class ContentModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IRoleBLL, RoleBLL>();
            containerRegistry.Register<IRoleDAL, RoleDAL>();
            containerRegistry.RegisterForNavigation<RoleMainView>();
            containerRegistry.RegisterForNavigation<RoleView>();
            containerRegistry.RegisterForNavigation<RoleEditView>();
            containerRegistry.RegisterForNavigation<MenuMainView>();
            containerRegistry.RegisterForNavigation<MenuView>();
            containerRegistry.RegisterForNavigation<MenuEditView>();
            containerRegistry.RegisterForNavigation<UserMainView>();
            containerRegistry.RegisterForNavigation<UserView>();
            containerRegistry.RegisterForNavigation<UserEditView>();
        }
    }
}