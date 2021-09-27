using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using DistantStars.Client.Resource.Helpers;
using DistantStars.Client.Resource.Proxy;

namespace DistantStars.Client.Resource.Controls.Tips
{
    public class MessageControl : TipsBase
    {
        private readonly int _outSeconds = 3;
        private DispatcherTimer _outTimer;
        private int _tickCount = 0;
        static MessageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageControl), new FrameworkPropertyMetadata(typeof(MessageControl)));
        }
        public MessageControl()
        {
            StartTimer();
        }

        public MessageControl(string msg) : this()
        {
            Message = msg;
        }

        public MessageControl(string msg, int outSeconds) : this(msg)
        {
            _outSeconds = outSeconds;
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
                var x = HorizontalAlignment == HorizontalAlignment.Right ? Width : -Width;
                RenderTransform.BindingDoubleAnimation(TranslateTransform.XProperty, x, completed: () =>
                {
                    MessagePanel.Children.Remove(this);
                });
            }));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var x = HorizontalAlignment == HorizontalAlignment.Right ? Width : -Width;
            var transform = new TranslateTransform
            {
                X = x
            };
            RenderTransform = transform;
            transform.BindingDoubleAnimation(TranslateTransform.XProperty, 0);
        }


        public static IMessage ShowMessage(FrameworkElement element, string msg, int outTime = 3)
        {
            IMessage message = null;
            element.Dispatcher.Invoke(() =>
            {
                //if (GetMessagePanel(GetMessageWindow(element)) is Grid grid)
                //{
                //    var messageControl = new MessageControl(msg, outTime);
                //    message = messageControl;
                //    if (grid.ColumnDefinitions.Count > 0)
                //        Grid.SetColumnSpan(messageControl, grid.ColumnDefinitions.Count);
                //    if (grid.RowDefinitions.Count > 0)
                //        Grid.SetRowSpan(messageControl, grid.RowDefinitions.Count);
                //    Panel.SetZIndex(messageControl, 100);
                //    grid.Children.Insert(0, messageControl);
                //}
                var messageControl = new MessageControl(msg, outTime);
                MessagePanel.Children.Insert(0, messageControl);
                message = messageControl;
            });

            return message;
        }
    }
}
