using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DistantStars.Client.BarrageModule.Helpers
{
    public static class LocalFileHelper
    {
        public static JObject LocalImage;
        static readonly string LocalImageJson = $"{AppDomain.CurrentDomain.BaseDirectory}/Images/LocalImageJson.json";

        static LocalFileHelper()
        {
            LocalImage = JObject.Parse(File.Exists(LocalImageJson) ? File.ReadAllText(LocalImageJson) : "{bilibili:{},douyu:{}}");
        }
        /// <summary>
        /// 获取本地gif地址
        /// </summary>
        /// <param name="giftId"></param>
        /// <param name="group">bilibili,douyu</param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetGiftLocalPath(this string giftId, string group, string uri)
        {
            string result = string.Empty;
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(LocalImage.ToString());
            if (dictionary[group].ContainsKey(giftId) && File.Exists(dictionary[group][giftId]))
            {
                result = dictionary[group][giftId];
            }
            else
            {
                try
                {
                    var e = Path.GetExtension(uri);
                    var save = $"{AppDomain.CurrentDomain.BaseDirectory}\\Images\\{group}";
                    if (!Directory.Exists(save))
                    {
                        Directory.CreateDirectory(save);
                    }
                    result = $"{save}\\{giftId}{e}";
                    DownloadFile(uri, result);
                    dictionary[group].Add(giftId, result);
                    File.WriteAllText(LocalImageJson, JsonConvert.SerializeObject(dictionary,Formatting.Indented));
                }
                catch (Exception e)
                {

#if DEBUG
                    //Application.Current.MainWindow.Show(e.Message, outTime: 60);
#endif
                    Console.WriteLine(e.Message);
                }
            }
            return result;
        }

        static void GetHttpsFile(string uri, string saveFile)
        {
            var http = (HttpWebRequest)WebRequest.Create(uri);
            http.Method = "GET";
            using (var response = (HttpWebResponse)http.GetResponse())
            {
                var s = response.GetResponseStream();
                Bitmap bitmap = new Bitmap(s);
                bitmap.Save(saveFile);
                s.Dispose();
            }
        }

        /// <summary>
        /// 下载线上文件到本地(gif图片可以保持动画)
        /// </summary>
        /// <param name="URL">下载文件链接</param>
        /// <param name="filename">保存到本地的地址</param>
        /// <returns></returns>
        public static void DownloadFile(string URL, string filename)
        {
            HttpWebRequest req = null;
            HttpWebResponse rep = null;
            Stream st = null;
            Stream so = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(URL); //请求网络资源

                req.UserAgent =
                    "Mozilla/5.0 (Linux; Android 5.0; SM-G900P Build/LRX21T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Mobile Safari/537.36";
                rep = (HttpWebResponse)req.GetResponse(); //返回Internet资源的响应
                long totalBytes = rep.ContentLength; //获取请求返回内容的长度
                st = rep.GetResponseStream(); //读取服务器的响应资源，以IO流的形式进行读写
                so = new FileStream(filename, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length); //读取当前字节流的总长度
                }

                so.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (so != null)
                {
                    so.Close();
                    so.Dispose();
                }

                if (st != null)
                {
                    st.Close();
                    st.Dispose();
                }

                if (rep != null)
                {
                    rep.Close();
                    rep.Close();
                } // rep.Dispose();

                if (req != null)
                {
                    req.Abort();
                }

            }
        }
    }
}
