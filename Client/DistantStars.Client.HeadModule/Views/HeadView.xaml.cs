using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DistantStars.Client.HeadModule.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class HeadView : UserControl
    {
        public HeadView()
        {
            InitializeComponent();
            Loaded += HeadView_Loaded;
        }

        private void HeadView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= HeadView_Loaded;
            e.Handled = true;
            if (Window.GetWindow(this) is Window window)
            {
                Min.Click += WindowButton_OnClick;
                Normal.Click += WindowButton_OnClick;
                Close.Click += WindowButton_OnClick;
                window.StateChanged += (s, args) =>
                {
                    Normal.IsChecked = window.WindowState == WindowState.Maximized;
                };
            }
        }

        private void WindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is ButtonBase buttonBase && Window.GetWindow(buttonBase) is Window window)
            {
                switch (buttonBase.Name)
                {
                    case "Min":
                        window.WindowState = WindowState.Minimized;
                        break;
                    case "Normal":
                        if (buttonBase is ToggleButton toggleButton)
                        {
                            window.WindowState = toggleButton.IsChecked == true ? WindowState.Maximized : WindowState.Normal;
                        }
                        break;
                    case "Close":
                        window.Close();
                        break;
                }
            }
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnMouseLeftButtonDown(e);
            if (Window.GetWindow(this) is Window window)
            {
                window.DragMove();
            }
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnMouseDoubleClick(e);
            if (Window.GetWindow(this) is Window window)
            {
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }

    }
}
