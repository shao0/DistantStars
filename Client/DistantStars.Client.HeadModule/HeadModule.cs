using DistantStars.Client.BLL.Systems;
using DistantStars.Client.Common;
using DistantStars.Client.DAL.Systems;
using DistantStars.Client.HeadModule.Views;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.IDAL.Systems;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace DistantStars.Client.HeadModule
{
    public class HeadModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.MainHeader, typeof(HeadView));
            regionManager.RegisterViewWithRegion(RegionNames.HeaderMenu, typeof(MenuView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IMenuBLL, MenuBLL>();
            containerRegistry.Register<IMenuDAL, MenuDAL>();
        }
    }
}