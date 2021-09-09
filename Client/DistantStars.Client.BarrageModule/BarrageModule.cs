using DistantStars.Client.BarrageModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace DistantStars.Client.BarrageModule
{
    public class BarrageModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BarrageView>();
        }
    }
}