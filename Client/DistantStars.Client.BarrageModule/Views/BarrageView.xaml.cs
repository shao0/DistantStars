using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DistantStars.Client.BarrageModule.Views
{
    /// <summary>
    /// Interaction logic for BarrageView.xaml
    /// </summary>
    public partial class BarrageView : UserControl
    {
        public BarrageView()
        {
            InitializeComponent();
        }
        private void Toggle_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                var storyboard = toggle.IsChecked == false
                    ? (Storyboard)FindResource("HiddenExtend")
                    : (Storyboard)FindResource("ShowExtend");
                if (storyboard != null)
                    toggle.BeginStoryboard(storyboard);
            }
        }

        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            if (sender is MediaElement media)
            {
                media.Position = TimeSpan.FromMilliseconds(1);
                media.Play();
            }
        }
    }
}
