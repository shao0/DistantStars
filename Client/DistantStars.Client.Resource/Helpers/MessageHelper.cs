using System.Windows;
using DistantStars.Client.Resource.Controls.Tips;
using DistantStars.Client.Resource.Data.Enum;
using DistantStars.Client.Resource.Data.Info;
using DistantStars.Client.Resource.Proxy;

namespace DistantStars.Client.Resource.Helpers
{
    public static class MessageHelper
    {
        /// <summary>
        /// 窗口消息
        /// </summary>
        /// <param name="element"></param>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        public static IMessage Show(this FrameworkElement element, string msg, ShowType type = ShowType.Info, int waitTime = 6)
        {
            return MessageControl.ShowMessage(element, ShowMessageInfo.Create(msg, type, waitTime));
        }
        /// <summary>
        /// 加载遮罩信息
        /// </summary>
        /// <param name="element"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static IMessage Loading(this FrameworkElement element, string msg)
        {
            return LoadingControl.ShowLoadingAnimation(element, msg);
        }

    }

}
