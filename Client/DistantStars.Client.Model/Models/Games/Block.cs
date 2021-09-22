using Prism.Mvvm;

namespace DistantStars.Client.Model.Models.Games
{
    public class Block : BindableBase
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string Img => $"/DistantStars.Client.Resource;component/Images/Game/爱宠{_tag.ToString().PadLeft(2, '0')}.png";

        private bool _IsChecked;
        /// <summary>
        /// 选中
        /// </summary>
        public bool IsChecked
        {
            get => _IsChecked;
            set => SetProperty(ref _IsChecked, value);
        }



        private int _tag;
        /// <summary>
        /// 类容
        /// </summary>
        public int Tag
        {
            get => _tag;
            set
            {
                if (_tag != value)
                {
                    SetProperty(ref _tag, value);
                }
            }
        }

        #region bool Tips 提示
        /// <summary>
        /// 提示 字段
        /// </summary>
        private bool _Tips;
        /// <summary>
        /// 提示 属性
        /// </summary>
        public bool Tips
        {
            get => _Tips;
            set
            {
                if (_Tips != value)
                {
                    _Tips = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

    }
}
