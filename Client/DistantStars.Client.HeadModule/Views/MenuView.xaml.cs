using System.Windows;
using System.Windows.Controls;

namespace DistantStars.Client.HeadModule.Views
{
    /// <summary>
    /// MenuView.xaml 的交互逻辑
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void MenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            RootMenu.PlacementTarget = MenuButton;
            RootMenu.IsOpen = true;
        }
        
    }
}
