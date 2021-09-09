using System.Windows;
using DistantStars.Client.BLL.Systems;
using DistantStars.Client.Common.Tools;
using DistantStars.Client.Common.Tools.Interfaces;
using DistantStars.Client.ContentModule;
using DistantStars.Client.DAL.Systems;
using DistantStars.Client.HeadModule;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.IDAL.Systems;
using DistantStars.Client.Start.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace DistantStars.Client.Start
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void InitializeShell(Window shell)
        {
            if (Container.Resolve<LoginView>().ShowDialog() == true)
            {
                base.InitializeShell(shell);
            }
            else
            {
                Current?.Shutdown();
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.Register<IUserBLL, UserBLL>();
            containerRegistry.Register<IUserDAL, UserDAL>();
            containerRegistry.Register<IFileBLL, FileBLL>();
            containerRegistry.Register<IFileDAL, FileDAL>();
            containerRegistry.Register<IConfig, Config>();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<GameModule.GameModule>();
            moduleCatalog.AddModule<HeadModule.HeadModule>();
            moduleCatalog.AddModule<ContentModule.ContentModule>();
            moduleCatalog.AddModule<BarrageModule.BarrageModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
