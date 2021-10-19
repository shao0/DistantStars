using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DistantStars.Client.Resource.Helpers;
using DistantStars.Client.Resource.Proxy;
using HandyControl.Interactivity;

namespace DistantStars.Client.Resource.Controls.Tips
{
    public class LoadingControl : TipsBase
    {
        public TipsAdorner TipsAdorner;

        private Panel LoadingPanel;

        static LoadingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingControl), new FrameworkPropertyMetadata(typeof(LoadingControl)));
        }
        public LoadingControl()
        {
        }

        public override void OnApplyTemplate()
        {
            LoadingPanel = (Panel)Template.FindName("PART_LoadingPanel", this);
            Loaded += LoadingControl_Loaded;
        }

        public LoadingControl(string message) : this()
        {
            Message = message;
        }
        private void LoadingControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= LoadingControl_Loaded;
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

        public override void Close()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (TipsAdorner != null)
                {
                    WindowAdornerDecorator?.AdornerLayer?.Remove(TipsAdorner);
                }
            }));
        }

        public static IMessage ShowLoadingAnimation(FrameworkElement element, string msg)
        {
            IMessage message = null;
            element?.Dispatcher.Invoke(() =>
            {
                var loading = WindowAdornerDecorator.AdornerLayer.FindVisualChild<LoadingControl>();
                if (loading != null)
                {
                    loading.Message = msg;
                }
                else
                {
                    loading = new LoadingControl(msg);
                    var layer = WindowAdornerDecorator.AdornerLayer;
                    loading.TipsAdorner = new TipsAdorner(layer) { Child = loading };
                    layer.Add(loading.TipsAdorner);
                }
                message = loading;
            });
            return message;
        }
    }
}
