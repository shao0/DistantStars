using System.ComponentModel;
using Prism.Mvvm;

namespace DistantStars.Client.BarrageModule.Models
{
    public class BarrageInfo : BindableBase
    {
        public BarrageInfo()
        {
            OutSeconds = 30;
        }

        public int OutSeconds;
        /// <summary>
        /// 昵称
        /// </summary>
        public string NikeName { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 类容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 弹幕颜色
        /// </summary>
        public BarrageColor BarrageColor { get; set; }
        /// <summary>
        /// 弹幕类型
        /// </summary>
        public BarrageType BarrageType { get; set; }
        /// <summary>
        /// 字典转json
        /// </summary>
        public string JsonMessage { get; set; }

    }
    /// <summary>
    /// 弹幕属性
    /// </summary>
    public enum BarrageType
    {
        /// <summary>
        /// 弹幕消息
        /// </summary>
        Message,
        /// <summary>
        /// 弹幕礼物
        /// </summary>
        Gift,
    }

    public enum BarrageColor
    {
        [Description("#2c3e50")]
        Default,
        [Description("#ff0000")]
        Color1 = 1,
        [Description("#1e87f0")]
        Color2 = 2,
        [Description("#7ac84b")]
        Color3 = 3,
        [Description("#ff7f00")]
        Color4 = 4,
        [Description("#9b39f4")]
        Color5 = 5,
        [Description("#ff69b4")]
        Color6 = 6,
    }

}
