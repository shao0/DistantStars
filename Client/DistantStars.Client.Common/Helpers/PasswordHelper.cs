using System.Windows;
using System.Windows.Controls;

namespace DistantStars.Client.Common.Helpers
{
    public class PasswordHelper : DependencyObject
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password", typeof(string), typeof(PasswordHelper), new PropertyMetadata(new PropertyChangedCallback(OnPasswordChanged)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="password"></param>
        public static void SetPassword(DependencyObject obj, string password)
        {
            obj.SetValue(PasswordProperty, password);
        }

        #region bool Attach 是否附加

        /// <summary>
        /// 是否附加 附加依赖属性获取值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetAttach(DependencyObject obj)
        {
            return (bool)obj.GetValue(AttachProperty);
        }

        /// <summary>
        /// 是否附加 附加依赖属性设置值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetAttach(DependencyObject obj, bool value)
        {
            obj.SetValue(AttachProperty, value);
        }

        /// <summary>
        /// 是否附加 附加依赖属性
        /// </summary>
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached(
            "Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(new PropertyChangedCallback(OnAttachChanged)));

        private static bool _isUpdating;
        private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (!_isUpdating)
                {
                    passwordBox.Password = e.NewValue.ToString();
                }
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }

        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _isUpdating = true;
                SetPassword(passwordBox, passwordBox.Password);
                _isUpdating = false;
            }
        }

        #endregion
    }
}
