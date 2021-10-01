using DistantStars.Client.Resource.Data.Enum;

namespace DistantStars.Client.Resource.Data.Info
{
    /// <summary>
    /// 消息信息类
    /// </summary>
    public class ShowMessageInfo
    {
        /// <summary>
        /// 展示信息
        /// </summary>
        public string ShowMessage { get; set; }
        /// <summary>
        /// 展示类型
        /// </summary>
        public ShowType ShowType { get; set; }
        /// <summary>
        /// 展示等待时间
        /// </summary>
        public int WaitTime { get; set; }
        /// <summary>
        /// 创建消息信息类
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        public static ShowMessageInfo Create(string msg, ShowType type, int waitTime) => new ShowMessageInfo { ShowMessage = msg, ShowType = type, WaitTime = waitTime };
        /// <summary>
        /// 创建消息信息类
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ShowMessageInfo Create(string msg, ShowType type) => Create(msg, type, 6);
        /// <summary>
        /// 创建消息信息类
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        public static ShowMessageInfo Create(string msg, int waitTime) => Create(msg, ShowType.Info, waitTime);
        /// <summary>
        /// 创建消息信息类
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ShowMessageInfo Create(string msg) => Create(msg, ShowType.Info);
    }
}
