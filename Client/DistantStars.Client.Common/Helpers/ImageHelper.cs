using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace DistantStars.Client.Common.Helpers
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 根据图片路径转换为图片二进制byte
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ConvertBytes(this string path)
        {
            try
            {
                byte[] array;
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                }
                return array;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 根据图片二进制byte数据转换为图片位图
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static BitmapImage ConvertBitmapImage(this byte[] bytes)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.DelayCreation;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(bytes);
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 根据图片二进制byte数据转换为图片位图
        /// </summary>
        /// <param name="fileUri"></param>
        /// <returns></returns>
        public static BitmapImage UriConvertBitmapImage(this string fileUri)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.DelayCreation;
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileUri, UriKind.Absolute);
                bitmapImage.DecodePixelWidth = 40;
                bitmapImage.EndInit();
                return bitmapImage;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 根据图片路径转换为图片位图
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage ConvertBitmapImage(this string path)
        {
            return path.ConvertBytes().ConvertBitmapImage();
        }

        public static string ImagePathToBase64String(this string path)
        {
            return Convert.ToBase64String(path.ConvertBytes());
        }

        public static BitmapImage Base64StringToBitmapImage(this string base64)
        {
            return Convert.FromBase64String(base64).ConvertBitmapImage();
        }

    }
}
