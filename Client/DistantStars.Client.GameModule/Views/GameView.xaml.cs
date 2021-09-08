using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DistantStars.Client.GameModule.Views
{
    /// <summary>
    /// GameView.xaml 的交互逻辑
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
            //Loaded += GameView_Loaded;
        }

        private void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Game;component/Resources/Images/爱宠02.png")));
        }
    }
}
