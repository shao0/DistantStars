using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DistantStars.Client.Resource.Controls
{
    /// <summary>
    /// LoadingUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingUserControl
    {
        public LoadingUserControl()
        {
            InitializeComponent();
            Loaded += LoadingUserControl_Loaded;
        }

        public LoadingUserControl(string message) : this()
        {
            Message = message;
        }
        private void LoadingUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= LoadingUserControl_Loaded;
            CreateLoadedAnimation();
        }
        private void CreateLoadedAnimation()
        {
            var width = 20;
            var ellipseWidth = width / 2;
            var move = width - ellipseWidth + 2;
            var count = 10;
            var e = 0.1;
            for (int i = 0; i < count; i++)
            {
                var ellipse = new Ellipse();
                ellipse.Height = ellipse.Width = ellipseWidth;
                ellipse.Fill = Brushes.DodgerBlue;
                var border = new Border();
                border.Child = ellipse;
                border.Height = border.Width = width;
                var translate = new TranslateTransform();
                border.RenderTransform = translate;
                border.RenderTransformOrigin = Point.Parse("0.5,0.5");
                border.VerticalAlignment = VerticalAlignment.Center;
                border.HorizontalAlignment = HorizontalAlignment.Center;
                DoubleAnimationUsingKeyFrames doubleAnimation = new DoubleAnimationUsingKeyFrames();
                doubleAnimation.KeyFrames = new DoubleKeyFrameCollection();
                var keyFrame = new LinearDoubleKeyFrame();
                keyFrame.KeyTime = TimeSpan.Parse($"0:0:{i * e}");
                keyFrame.Value = 0;
                doubleAnimation.KeyFrames.Add(keyFrame);

                keyFrame = new LinearDoubleKeyFrame();
                keyFrame.KeyTime = TimeSpan.Parse($"0:0:{(i + 1) * e}");
                keyFrame.Value = move;
                doubleAnimation.KeyFrames.Add(keyFrame);

                keyFrame = new LinearDoubleKeyFrame();
                keyFrame.KeyTime = TimeSpan.Parse($"0:0:{(2 * count - (i + 1)) * e}");
                keyFrame.Value = move;
                doubleAnimation.KeyFrames.Add(keyFrame);

                keyFrame = new LinearDoubleKeyFrame();
                keyFrame.KeyTime = TimeSpan.Parse($"0:0:{(2 * count - i) * e}");
                keyFrame.Value = 0;
                doubleAnimation.KeyFrames.Add(keyFrame);

                keyFrame = new LinearDoubleKeyFrame();
                keyFrame.KeyTime = TimeSpan.Parse($"0:0:{2 * count * e}");
                keyFrame.Value = 0;
                doubleAnimation.KeyFrames.Add(keyFrame);
                doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                translate.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
                LoadingPanel.Children.Add(border);
            }
        }
        public void Close()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var window = Window.GetWindow(this);
                if (window == null) return;
                Panel panel = (Panel)GetLoadingPanel(window);
                panel.Children.Clear();
                panel.Visibility = Visibility.Collapsed;
            }));
        }

        #region FrameworkElement LoadingPael 加载面板

        /// <summary>
        /// 属性名称 附加依赖属性获取值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static FrameworkElement GetLoadingPanel(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(LoadingPanelProperty);
        }

        /// <summary>
        /// 属性名称 附加依赖属性设置值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetLoadingPanel(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(LoadingPanelProperty, value);
        }

        /// <summary>
        /// 属性名称 附加依赖属性
        /// </summary>
        public static readonly DependencyProperty LoadingPanelProperty = DependencyProperty.RegisterAttached(
            "LoadingPanel", typeof(FrameworkElement), typeof(LoadingUserControl), new PropertyMetadata(default(FrameworkElement), (s, e) =>
            {
                var window = Window.GetWindow(s);
                SetLoadingPanel(window, (FrameworkElement)e.NewValue);
            }));

        #endregion

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(LoadingUserControl), new PropertyMetadata(default(string)));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        //public static IMessage ShowLoadingAnimation(FrameworkElement element, string msg)
        //{
        //    IMessage message = null;
        //    element?.Dispatcher.Invoke(() =>
        //    {
        //        var window = Window.GetWindow(element);
        //        Panel panel = (Panel)GetLoadingPanel(window);
        //        panel.Visibility = Visibility.Visible;
        //        LoadingUserControl loadingUserControl;
        //        if (panel.Children.Count > 0 && panel.Children[0] is LoadingUserControl loadingControl)
        //        {
        //            loadingUserControl = loadingControl;
        //            loadingUserControl.Message = msg;
        //        }
        //        else
        //        {
        //            loadingUserControl = new LoadingUserControl(msg);
        //            panel.Children.Add(loadingUserControl);
        //        }
        //        message = loadingUserControl;
        //    });
        //    return message;
        //}
    }
}
