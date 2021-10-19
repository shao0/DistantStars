using System.Windows;
using System.Windows.Media;

namespace DistantStars.Client.Resource.Helpers
{
    public static class VisualHelper
    {
        /// <summary>
        /// 视觉树查询子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T FindVisualChild<T>(this DependencyObject d) where T : DependencyObject
        {
            if (d == null) return default;
            if (d is T t) return t;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var child = VisualTreeHelper.GetChild(d, i);

                var result = FindVisualChild<T>(child);
                if (result != null) return result;
            }

            return default;
        }
        /// <summary>
        /// 视觉树查询父元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T FindVisualParent<T>(this DependencyObject d) where T : DependencyObject =>
        d switch
        {
            null => default,
            T t => t,
            Window _ => null,
            _ => FindVisualParent<T>(VisualTreeHelper.GetParent(d))
        };
    }
}
