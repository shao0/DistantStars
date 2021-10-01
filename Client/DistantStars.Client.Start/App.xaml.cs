using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DistantStars.Client.BLL.Systems;
using DistantStars.Client.Common.Tools;
using DistantStars.Client.Common.Tools.Interfaces;
using DistantStars.Client.ContentModule;
using DistantStars.Client.DAL.Systems;
using DistantStars.Client.HeadModule;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.IDAL.Systems;
using DistantStars.Client.Resource.Helpers;
using DistantStars.Client.Start.Views;
using DistantStars.Client.ToolModule;
using Prism.Ioc;
using Prism.Modularity;

namespace DistantStars.Client.Start
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// UI线程未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出      
                Current.MainWindow.Show($"UI线程异常:{e.Exception.Message}");
            }
            catch (Exception ex)
            {
                //此时程序出现严重异常，将强制结束退出
                MessageBox.Show("UI线程发生致命错误！");
            }

        }
        /// <summary>
        /// 非UI线程未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (e.IsTerminating)
            {
                stringBuilder.Append("非UI线程发生致命错误");
            }
            stringBuilder.Append("非UI线程异常：");
            if (e.ExceptionObject is Exception exception)
            {
                stringBuilder.Append(exception.Message);
            }
            else
            {
                stringBuilder.Append(e.ExceptionObject);
            }
            Current.MainWindow.Show(stringBuilder.ToString());
        }
        /// <summary>
        /// Task线程内未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //task线程内未处理捕获
            Current.MainWindow.Show($"Task线程异常:{e.Exception.Message}");
            e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }

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
            moduleCatalog.AddModule<ToolModule.ToolModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
