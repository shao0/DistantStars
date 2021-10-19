using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using DistantStars.Client.Resource.Data.Enum;
using DistantStars.Client.Resource.Data.Info;
using DistantStars.Client.Resource.Helpers;
using DistantStars.Client.Resource.Proxy;

namespace DistantStars.Client.Resource.Controls.Tips
{
    public class MessageControl : TipsBase
    {
        private readonly int _outSeconds = 3;
        private DispatcherTimer _outTimer;
        private int _tickCount = 0;

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(Geometry), typeof(MessageControl), new PropertyMetadata(default(Geometry)));
        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register(
            "IconBrush", typeof(Brush), typeof(MessageControl), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// 图标颜色
        /// </summary>
        public Brush IconBrush
        {
            get { return (Brush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }
        private static Panel _MessagePanel;

        private static Panel MessagePanel => _MessagePanel ?? CreatePanel();

        private static Panel CreatePanel()
        {
            var layer = WindowAdornerDecorator.AdornerLayer;
            if (layer != null)
            {
                _MessagePanel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Top
                };

                var scrollViewer = new ScrollViewer
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    Content = _MessagePanel
                };

                var tipsAdorner = new TipsAdorner(layer)
                {
                    Child = scrollViewer
                };

                layer.Add(tipsAdorner);

                return _MessagePanel;
            }


            return null;
        }


        static MessageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageControl), new FrameworkPropertyMetadata(typeof(MessageControl)));
        }

        public MessageControl(ShowMessageInfo info)
        {
            Message = info.ShowMessage;
            _outSeconds = info.WaitTime;
            Icon = $"{info.ShowType}Geometry".GetResource<Geometry>();
            IconBrush = $"{info.ShowType}Brush".GetResource<Brush>();
        }


        private void StartTimer()
        {
            _outTimer = new DispatcherTimer();
            _outTimer.Interval = TimeSpan.FromSeconds(1);
            _outTimer.Tick += OutTimer_Tick;
            _outTimer.Start();
        }


        private void OutTimer_Tick(object sender, EventArgs e)
        {
            if (IsMouseOver)
            {
                _tickCount = 0;
                return;
            }
            _tickCount++;
            if (_tickCount >= _outSeconds)
            {
                Close();
            }
        }

        public override void Close()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _outTimer?.Stop();
                var x = HorizontalAlignment == HorizontalAlignment.Right ? MaxWidth : -MaxWidth;
                RenderTransform.BindingDoubleAnimation(TranslateTransform.XProperty, x, completed: () =>
                {
                    MessagePanel.Children.Remove(this);
                });
            }));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var x = HorizontalAlignment == HorizontalAlignment.Right ? MaxWidth : -MaxWidth;
            var transform = new TranslateTransform
            {
                X = x
            };
            RenderTransform = transform;
            transform.BindingDoubleAnimation(TranslateTransform.XProperty, 0, beginTime: 500);
            StartTimer();
        }


        public static IMessage ShowMessage(FrameworkElement element, ShowMessageInfo info)
        {
            IMessage message = null;
            element.Dispatcher.Invoke(() =>
            {
                var messageControl = new MessageControl(info);
                MessagePanel.Children.Insert(0, messageControl);
                message = messageControl;
            });

            return message;
        }
    }
}
