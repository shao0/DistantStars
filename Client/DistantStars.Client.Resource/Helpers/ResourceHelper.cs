using System.Windows;

namespace DistantStars.Client.Resource.Helpers
{
    public static class ResourceHelper
    {
        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetResource<T>(this string key)
        {
            if (Application.Current.TryFindResource(key) is T resource)
            {
                return resource;
            }

            return default;
        }
    }
}
