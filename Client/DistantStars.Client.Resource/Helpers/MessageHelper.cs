using System.Windows;
using DistantStars.Client.Resource.Controls.Tips;
using DistantStars.Client.Resource.Proxy;

namespace DistantStars.Client.Resource.Helpers
{
    public static class MessageHelper
    {
        public static IMessage Show(this FrameworkElement element, string msg, ShowEnum showEnum = ShowEnum.ShowText, int outTime = 3)
        {
            switch (showEnum)
            {
                case ShowEnum.ShowLoading:
                    return LoadingControl.ShowLoadingAnimation(element, msg);
                case ShowEnum.ShowText:
                default:
                    return MessageControl.ShowMessage(element, msg, outTime);
            }
        }

    }

    public enum ShowEnum
    {
        ShowText,
        ShowLoading
    }
}
