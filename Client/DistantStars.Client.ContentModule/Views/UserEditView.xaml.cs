using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DistantStars.Client.ContentModule.Views
{
    /// <summary>
    /// MenuEditView.xaml 的交互逻辑
    /// </summary>
    public partial class UserEditView : UserControl
    {
        public UserEditView()
        {
            InitializeComponent();
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox check)
            {
                var storyboard = check.IsChecked == true ? FindResource("ShowStoryboard") : FindResource("HiddenStoryboard");
                if (storyboard is Storyboard animation)
                {
                    PasswordTextBox.BeginStoryboard(animation);
                }
            }
        }
    }
}
