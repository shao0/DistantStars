using System.Windows;
using System.Windows.Controls;
using DistantStars.Client.Resource.Proxy;

namespace DistantStars.Client.Resource.Controls.Tips
{
    public abstract class TipsBase : ContentControl, IMessage
    {
        protected static Panel MessagePanel;
        static TipsBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TipsBase), new FrameworkPropertyMetadata(typeof(TipsBase)));
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(TipsBase), new PropertyMetadata(default(string)));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly DependencyProperty MessagePanelProperty = DependencyProperty.RegisterAttached(
            "MessagePanel", typeof(FrameworkElement), typeof(TipsBase), new PropertyMetadata(default(FrameworkElement),
                (s, e) =>
                {
                    var window = GetMessageWindow((FrameworkElement)s);

                    if (e.NewValue is FrameworkElement element)
                    {
                        SetMessagePanel(window, element);
                        element.Loaded += Element_Loaded;
                    }
                }));

        private static void Element_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Grid grid)
            {
                grid.Loaded -= Element_Loaded;
                MessagePanel = new StackPanel { HorizontalAlignment = HorizontalAlignment.Right };
                AddChildren(grid, MessagePanel);
            }
        }

        /// <summary>
        /// 获取消息容器面板
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static FrameworkElement GetMessagePanel(DependencyObject obj) => (FrameworkElement)obj.GetValue(MessagePanelProperty);

        /// <summary>
        /// 设置消息容器面板
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetMessagePanel(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(MessagePanelProperty, value);
        }

        public abstract void Close();
        /// <summary>
        /// 获取消息窗口
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static FrameworkElement GetMessageWindow(FrameworkElement element) =>
            element is Window ? element : Window.GetWindow(element);

        protected static void AddChildren(Grid grid, FrameworkElement messageControl)
        {
            if (grid.ColumnDefinitions.Count > 0)
                Grid.SetColumnSpan(messageControl, grid.ColumnDefinitions.Count);
            if (grid.RowDefinitions.Count > 0)
                Grid.SetRowSpan(messageControl, grid.RowDefinitions.Count);
            Panel.SetZIndex(messageControl, 100);
            grid.Children.Insert(0, messageControl);
        }
    }
}
