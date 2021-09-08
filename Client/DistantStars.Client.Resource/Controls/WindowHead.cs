using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DistantStars.Client.Resource.Controls
{
    public class WindowHead : ContentControl
    {
        private Button _min;
        private ToggleButton _normal;
        private Button _close;
        static WindowHead()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowHead), new FrameworkPropertyMetadata(typeof(WindowHead)));

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _min = (Button)Template.FindName("Min", this);
            _normal = (ToggleButton)Template.FindName("Normal", this);
            _close = (Button)Template.FindName("Close", this);
            Loaded += WindowHead_Loaded;
        }

        private void WindowHead_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= WindowHead_Loaded;
            e.Handled = true;
            if (Window.GetWindow(this) is Window window)
            {
                _min.Click += WindowButton_OnClick;
                _normal.Click += WindowButton_OnClick;
                _close.Click += WindowButton_OnClick;
                window.StateChanged += (s, args) =>
                {
                    _normal.IsChecked = window.WindowState == WindowState.Maximized;
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
            if (HiddenNormal) return;
            if (Window.GetWindow(this) is Window window)
            {
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(WindowHead), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty HiddenMinProperty = DependencyProperty.Register(
            "HiddenMin", typeof(bool), typeof(WindowHead), new PropertyMetadata(default(bool)));

        public bool HiddenMin
        {
            get { return (bool)GetValue(HiddenMinProperty); }
            set { SetValue(HiddenMinProperty, value); }
        }

        public static readonly DependencyProperty HiddenNormalProperty = DependencyProperty.Register(
            "HiddenNormal", typeof(bool), typeof(WindowHead), new PropertyMetadata(default(bool)));


        public bool HiddenNormal
        {
            get { return (bool)GetValue(HiddenNormalProperty); }
            set { SetValue(HiddenNormalProperty, value); }
        }


    }
}
