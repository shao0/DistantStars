using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistantStars.Client.ToolModule.Views;
using Prism.Ioc;

namespace DistantStars.Client.ToolModule
{
    public class ToolModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PicturePornHubView>();
        }
    }
}
