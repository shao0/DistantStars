using DistantStars.Client.Common;
using DistantStars.Client.GameModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace DistantStars.Client.GameModule
{
    public class GameModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.GameTool, typeof(GameToolView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<GameView>();
        }
    }
}