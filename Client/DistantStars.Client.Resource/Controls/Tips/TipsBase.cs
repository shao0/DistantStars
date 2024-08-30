using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using DistantStars.Client.Resource.Helpers;
using DistantStars.Client.Resource.Proxy;
using HandyControl.Interactivity;

namespace DistantStars.Client.Resource.Controls.Tips
{
    public abstract class TipsBase : ContentControl, IMessage
    {

        static TipsBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TipsBase), new FrameworkPropertyMetadata(typeof(TipsBase)));
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(TipsBase), new PropertyMetadata(default(string)));

        public string Message
        {
            get => (string) GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static Window GetActiveWindow() => Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) ?? Application.Current.MainWindow;


        public static AdornerDecorator WindowAdornerDecorator => GetActiveWindow().FindVisualChild<AdornerDecorator>();

        public abstract void Close();


    }
}
