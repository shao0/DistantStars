using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Prism.Commands;
using DistantStars.Client.BarrageModule.Barrages;
using DistantStars.Client.BarrageModule.Models;
using DistantStars.Client.Model.ViewModels;
using Prism.Regions;

namespace DistantStars.Client.BarrageModule.ViewModels
{
    public class BarrageViewModel : ContentViewModelBase
    {
        private FrameworkElement _view;

        //public ObservableCollection<BarrageInfo> MessageList { get; set; } = new ObservableCollection<BarrageInfo> { };

        public ObservableCollection<BarrageInfo> BarrageList { get; set; } = new ObservableCollection<BarrageInfo> { };

        public ObservableCollection<BarrageGift> GiftList { get; set; } = new ObservableCollection<BarrageGift> { };

        private DispatcherTimer _dispatcherTimer;

        public BarrageViewModel(IRegionManager region) : base(region)
        {
        }

        #region 弹幕
        private BarrageBase _DouYu;

        public BarrageBase DouYu
        {
            get
            {
                if (_DouYu == null)
                {
                    _DouYu = new DouYuBarrage();
                    _DouYu.ReceiveBarrage += ReceiveBarrage;
                    _DouYu.ConnectStateChanged += ConnectStateChanged;
                }


                return _DouYu;
            }
            set => _DouYu = value;
        }


        private BarrageBase _BiliBili;

        public BarrageBase BiliBili
        {
            get
            {
                if (_BiliBili == null)
                {
                    _BiliBili = new BiliBiliBarrage();
                    _BiliBili.ReceiveBarrage += ReceiveBarrage;
                    _BiliBili.ConnectStateChanged += ConnectStateChanged;
                }
                return _BiliBili;
            }
            set => _BiliBili = value;
        }

        #endregion

        #region int RoomNumber 房间号
        /// <summary>
        /// 属性名称 字段
        /// </summary>
        private int? _RoomNumber;
        /// <summary>
        /// 属性名称 属性
        /// </summary>
        public int? RoomNumber
        {
            get => _RoomNumber;
            set
            {
                if (_RoomNumber != value)
                {
                    _RoomNumber = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region string Platform 平台
        /// <summary>
        /// 平台 字段
        /// </summary>
        private string _platform = "BiliBili";
        /// <summary>
        /// 平台 属性
        /// </summary>
        public string Platform
        {
            get => _platform;
            set
            {
                if (_platform != value)
                {
                    _platform = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region bool NotReceived 连接中
        /// <summary>
        /// 连接中 字段
        /// </summary>
        private bool _notReceived = true;
        /// <summary>
        /// 连接中 属性
        /// </summary>
        public bool NotReceived
        {
            get => _notReceived;
            set
            {
                if (_notReceived != value)
                {
                    _notReceived = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region LoadedCommand 加载命令

        private DelegateCommand<object> _LoadedCommand;
        /// <summary>
        /// 加载命令
        /// </summary>
        public DelegateCommand<object> LoadedCommand => _LoadedCommand ?? (_LoadedCommand = new DelegateCommand<object>(Loaded));

        private void Loaded(object obj)
        {
            if (obj is FrameworkElement element)
            {
                _view = element;
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                _dispatcherTimer.Tick += DispatcherTimer_Tick;
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var removes = new List<BarrageGift>();
            foreach (var barrage in GiftList)
            {
                if (barrage.OutSeconds <= 0)
                {
                    removes.Add(barrage);
                }
                else
                {
                    barrage.OutSeconds--;
                }
            }
            foreach (var barrageModel in removes)
            {
                GiftList.Remove(barrageModel);
            }
            if (GiftList.Count == 0)
                _dispatcherTimer.Stop();
        }

        #endregion

        #region ConnectCommand 连接命令

        BarrageBase GetBarrageCore()
        {
            BarrageBase result = null;
            switch (Platform)
            {
                case "BiliBili":
                    result = BiliBili;
                    break;
                case "DouYu":
                    result = DouYu;
                    break;
            }
            return result;
        }

        private DelegateCommand _ConnectCommand;
        /// <summary>
        /// 连接命令
        /// </summary>
        public DelegateCommand ConnectCommand => _ConnectCommand ?? (_ConnectCommand = new DelegateCommand(Connect));

        private void Connect()
        {
            BarrageBase barrageBase = GetBarrageCore();
            if (NotReceived && RoomNumber != null)
            {
                NotReceived = false;
                barrageBase.Start(RoomNumber.Value);
            }
            else
            {
                barrageBase.Stop();
            }
        }

        #endregion


        private void ReceiveBarrage(object sender, BarrageInfo e)
        {
            switch (e.BarrageType)
            {
                case BarrageType.Message:
                    {
                        if (BarrageList.Count >= 20)
                        {
                            BarrageList.RemoveAt(0);
                        }
                        BarrageList.Add(e);
                        break;
                    }
                case BarrageType.Gift:
                    {
                        var gift = (BarrageGift)e;
                        var barrageGift = GiftList.FirstOrDefault(b => b.UserId == gift.UserId && b.GiftId == gift.GiftId);
                        if (barrageGift != null)
                        {
                            barrageGift.Continuity = gift.Continuity;
                            barrageGift.OutSeconds = 30;
                        }
                        else
                        {
                            GiftList.Add(gift);
                            //if (MessageList.Count >= 2)
                            //    MessageList.RemoveAt(0);
                            //MessageList.Add(e);
                        }
                        if (!_dispatcherTimer.IsEnabled)
                            _dispatcherTimer.Start();
                        break;
                    }
                default:
                    break;
            }
        }

        private void ConnectStateChanged(object sender, bool e)
        {
            if (e) return;
            BarrageList.Clear();
            GiftList.Clear();
            //MessageList.Clear();
            NotReceived = true;
        }

    }
}
