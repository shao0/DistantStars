using DistantStars.Client.GameModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace DistantStars.Client.GameModule
{
    public class GameModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<GameView>();
        }
    }
}