using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistantStars.Client.BarrageModule.Models
{
    public class BarrageGift : BarrageInfo
    {
        public string GiftId { get; set; }
        /// <summary>
        /// 礼物动图
        /// </summary>
        public string DynamicGraph { get; set; }
        /// <summary>
        /// 礼物个数
        /// </summary>
        public int Count { get; set; }

        #region int Continuity 礼物连续
        /// <summary>
        /// 礼物连续 字段
        /// </summary>
        private int _Continuity;
        /// <summary>
        /// 礼物连续 属性
        /// </summary>
        public int Continuity
        {
            get => _Continuity;
            set
            {
                if (_Continuity != value)
                {
                    _Continuity = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

    }
}
