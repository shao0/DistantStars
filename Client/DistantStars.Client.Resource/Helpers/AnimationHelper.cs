using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace DistantStars.Client.Resource.Helpers
{
    public static class AnimationHelper
    {
        /// <summary>
        /// 绑定double类型动画
        /// </summary>
        /// <param name="bindingObject">绑定对象</param>
        /// <param name="dependencyProperty">绑定属性</param>
        /// <param name="toValue">绑定属性达到值</param>
        /// <param name="milliseconds">属性达到绑定值时间(毫秒)</param>
        /// <param name="beginTime">等待动画时间(毫秒)</param>
        /// <param name="completed">动画完成委托</param>
        public static void BindingDoubleAnimation(this Animatable bindingObject, DependencyProperty dependencyProperty, double toValue, double milliseconds = 500, double beginTime = 0, Action completed = null)
        {
            var doubleAnimation = CreateDoubleAnimation(toValue, milliseconds, beginTime, completed);
            bindingObject.BeginAnimation(dependencyProperty, doubleAnimation);
        }

        /// <summary>
        /// 绑定double类型动画
        /// </summary>
        /// <param name="bindingObject">绑定对象</param>
        /// <param name="dependencyProperty">绑定属性</param>
        /// <param name="toValue">绑定属性达到值</param>
        /// <param name="milliseconds">属性达到绑定值时间(毫秒)</param>
        /// <param name="beginTime">等待动画时间(毫秒)</param>
        /// <param name="completed">动画完成委托</param>
        public static void BindingDoubleAnimation(this UIElement bindingObject, DependencyProperty dependencyProperty, double toValue, double milliseconds = 500, double beginTime = 0, Action completed = null)
        {
            var doubleAnimation = CreateDoubleAnimation(toValue, milliseconds, beginTime, completed);
            bindingObject.BeginAnimation(dependencyProperty, doubleAnimation);
        }

        /// <summary>
        /// 创建double类型动画
        /// </summary>
        /// <param name="toValue">绑定属性达到值</param>
        /// <param name="milliseconds">属性达到绑定值时间(毫秒)</param>
        /// <param name="beginTime">等待动画时间(毫秒)</param>
        /// <param name="completed">动画完成委托</param>
        /// <returns></returns>
        private static DoubleAnimation CreateDoubleAnimation(double toValue, double milliseconds, double beginTime,
            Action completed)
        {
            var doubleAnimation = new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)));

            if (beginTime > 0)
            {
                doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
            }

            doubleAnimation.EasingFunction = new CubicEase {EasingMode = EasingMode.EaseOut};

            if (completed != null)
            {
                doubleAnimation.Completed += (_, __) => completed.Invoke();
            }

            return doubleAnimation;
        }

    }
}
