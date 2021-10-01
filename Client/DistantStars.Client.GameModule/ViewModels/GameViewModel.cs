using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DistantStars.Client.Common.ViewModels;
using DistantStars.Client.GameModule.Common;
using DistantStars.Client.Model.Models.Games;
using DistantStars.Client.Resource.Data.Enum;
using DistantStars.Client.Resource.Helpers;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace DistantStars.Client.GameModule.ViewModels
{
    public class GameViewModel : ContentViewModelBase
    {

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        ///     数据源
        /// </summary>
        public ObservableCollection<ObservableCollection<Block>> DataSourceList { get; set; } =
            new ObservableCollection<ObservableCollection<Block>>();
        /// <summary>
        /// 分组数据
        /// </summary>
        private readonly Dictionary<int, List<Block>> _groupData = new Dictionary<int, List<Block>>();

        /// <summary>
        /// 关卡级别
        /// </summary>
        private readonly Level _level;

        /// <summary>
        /// 第一个选中
        /// </summary>
        private Block _firstBlock;

        /// <summary>
        /// 启动时间计时
        /// 
        /// </summary>
        public bool TimerStart = true;

        /// <summary>
        /// 时间计时
        /// </summary>
        private DispatcherTimer _timer;

        private bool AutoControl = false;

        /// <summary>
        /// 宽高
        /// </summary>
        public double WidthHeight { get; set; } = 50;

        /// <summary>
        /// 最大时间
        /// </summary>
        public double MaxProgress { get; set; } = 100;



        public GameViewModel(IEventAggregator eventAggregator, IRegionManager regionManager) : base(regionManager)
        {
            _eventAggregator = eventAggregator;
            _level = new Level();
        }
        #region string ConnectPath 连接路径
        /// <summary>
        /// 连接路径 字段
        /// </summary>
        private string _ConnectPath;
        /// <summary>
        /// 连接路径 属性
        /// </summary>
        public string ConnectPath
        {
            get => _ConnectPath;
            set
            {
                if (_ConnectPath != value)
                {
                    _ConnectPath = value;
                    RaisePropertyChanged();
                    DelayedEmpty(_ConnectPath);
                }
            }
        }

        private async void DelayedEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            await Task.Delay(200);
            ConnectPath = string.Empty;
        }

        #endregion

        #region double TimeProgress 时间进度
        /// <summary>
        /// 时间进度 字段
        /// </summary>
        private double _TimeProgress;
        /// <summary>
        /// 时间进度 属性
        /// </summary>
        public double TimeProgress
        {
            get => _TimeProgress;
            set
            {
                if (_TimeProgress != value)
                {
                    _TimeProgress = value;
                    _eventAggregator.GetEvent<PubSubEvent<double>>().Publish(TimeProgress / MaxProgress);
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region  加载

        public override void LoadedContinue()
        {
            base.LoadedContinue();
            RandomDataSource(true);
            InitialTimer();
        }

        private void InitialTimer()
        {
            TimeProgress = MaxProgress;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeProgress--;
            if (TimeProgress < 0)
            {
                LoseControl();
            }
        }



        /// <summary>
        /// 随机数据源
        /// </summary>
        private void RandomDataSource(bool init = false)
        {
            var random = new Random();
            var range = new List<int>();
            var enumerable = Enumerable.Range(1, 32).ToArray();
            for (int i = 0; i < _level.TagNumber; i++)
            {
                var tagNumber = enumerable[random.Next(enumerable.Length - 1)];
                for (int j = 0; j < _level.Modulus; j++)
                {
                    range.Add(tagNumber);
                }
            }
            for (int i = 0; i < 18; i++)
            {
                if (init)
                {
                    DataSourceList.Add(new ObservableCollection<Block>());
                }
                else
                {
                    DataSourceList[i].Clear();
                }
                for (int j = 0; j < 18; j++)
                {
                    var content = 0;
                    if (i != 0 && i != 17 && j != 0 && j != 17)
                    {
                        var next = random.Next(range.Count - 1);
                        content = range[next];
                        range.Remove(content);
                    }
                    var block = new Block()
                    {
                        X = i,
                        Y = j,
                        Tag = content
                    };
                    DataSourceList[i].Add(block);
                    if (block.Tag != 0)
                    {
                        if (!_groupData.ContainsKey(block.Tag))
                        {
                            _groupData.Add(block.Tag, new List<Block>());
                        }
                        _groupData[block.Tag].Add(block);
                    }
                }
            }
        }

        #endregion

        #region CheckedCommand 选中命令

        private DelegateCommand<Block> _CheckedCommand;
        /// <summary>
        /// 选中命令
        /// </summary>
        public DelegateCommand<Block> CheckedCommand => _CheckedCommand ?? (_CheckedCommand = new DelegateCommand<Block>(Checked));

        private void Checked(Block obj)
        {
            if (_firstBlock == null)
            {
                _firstBlock = obj;
            }
            else if (_firstBlock == obj)
            {
                _firstBlock.IsChecked = false;
                _firstBlock = null;
            }
            else
            {
                var match = DataSourceList.BlockMatch(_firstBlock, obj, out var pathPoints);
                if (match && _firstBlock.Tag == obj.Tag)
                {
                    CorrectSetTime();
                    DrawConnectPath(pathPoints);
                    _groupData[_firstBlock.Tag].Remove(_firstBlock);
                    _groupData[_firstBlock.Tag].Remove(obj);
                    if (_groupData[_firstBlock.Tag].Count <= 0) _groupData.Remove(_firstBlock.Tag);
                    _firstBlock.Tag = obj.Tag = 0;
                    foreach (var blocks in DataSourceList)
                    {
                        foreach (var block in blocks)
                        {
                            block.Tips = false;
                        }
                    }
                    if (!_groupData.Any()) WinControl();
                    if (TimerStart)
                    {
                        TimerStart = false;
                        _timer.Start();
                    }
                }
                _firstBlock.IsChecked = obj.IsChecked = false;
                _firstBlock = null;
            }
        }
        /// <summary>
        /// 连接正确设置时间
        /// </summary>
        private void CorrectSetTime()
        {
            var timeProgress = _TimeProgress + _level.AddTime;
            TimeProgress = timeProgress > MaxProgress ? MaxProgress : timeProgress;
        }
        /// <summary>
        /// 绘制连接路线
        /// </summary>
        /// <param name="pathPoints"></param>
        private void DrawConnectPath(Block[] pathPoints)
        {
            StringBuilder sb = new StringBuilder("M ");
            foreach (var pathPoint in pathPoints)
            {
                var pathPointX = pathPoint.X * WidthHeight + WidthHeight / 2;
                var pathPointY = pathPoint.Y * WidthHeight + WidthHeight / 2;
                sb.Append(pathPointX);
                sb.Append(',');
                sb.Append(pathPointY);
                sb.Append(',');
            }
            sb.Remove(sb.Length - 1, 1);
            ConnectPath = sb.ToString();
        }
        #endregion

        #region TipsCommand 提示命令

        private DelegateCommand _TipsCommand;
        /// <summary>
        /// 提示命令
        /// </summary>
        public DelegateCommand TipsCommand => _TipsCommand ?? (_TipsCommand = new DelegateCommand(Tips));

        private void Tips()
        {
            GetJoin(out var block1, out var block2);
            block1.Tips = block2.Tips = true;
        }

        void GetJoin(out Block a, out Block b)
        {
            a = b = null;
            foreach (var keyValuePair in _groupData)
            {
                var array = keyValuePair.Value;
                foreach (var block1 in array)
                {
                    foreach (var block2 in
                        from block2
                            in array
                        where block1.Tag != 0 && block2.Tag != 0 && block1 != block2
                        let match = DataSourceList.BlockMatch(block1, block2, out _)
                        where match && block1.Tag == block2.Tag
                        select block2)
                    {
                        a = block1;
                        b = block2;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 连接消除
        /// </summary>
        /// <returns></returns>
        bool KeepLook()
        {
            while (AutoControl)
            {
                var removeTagList = new List<int>();
                var result = false;
                foreach (var keyValue in _groupData)
                {
                    for (int i = 0; i < keyValue.Value.Count; i++)
                    {
                        for (var j = i + 1; j < keyValue.Value.Count; j++)
                        {
                            if (keyValue.Value[i].Tag == 0 || keyValue.Value[j].Tag == 0) continue;
                            var b = DataSourceList.BlockMatch(keyValue.Value[i], keyValue.Value[j], out var pathPoints);
                            if (!b) continue;
                            result = true;
                            CorrectSetTime();
                            DrawConnectPath(pathPoints);
                            keyValue.Value[i].Tag = keyValue.Value[j].Tag = 0;
                            Thread.Sleep(200);
                        }
                    }
                    var removeBlock = keyValue.Value.Where(block => block.Tag == 0).ToList();
                    foreach (var block in removeBlock)
                    {
                        keyValue.Value.Remove(block);
                    }

                    if (keyValue.Value.Count == 0) removeTagList.Add(keyValue.Key);
                }

                foreach (var tag in removeTagList)
                {
                    _groupData.Remove(tag);
                }

                if (result && _groupData.Count > 0)
                {
                    continue;
                }
                return result || _groupData.Count == 0;
            }
            return true;
        }

        #endregion

        #region RemakeCommand 刷新命令

        private DelegateCommand _RemakeCommand;
        /// <summary>
        /// 刷新命令
        /// </summary>
        public DelegateCommand RemakeCommand => _RemakeCommand ?? (_RemakeCommand = new DelegateCommand(Remake));

        private void Remake()
        {
            List<int[]> pointList = new List<int[]>();
            List<Block> contentList = new List<Block>();
            var random = new Random();
            for (var i = 0; i < DataSourceList.Count; i++)
            {
                for (var j = 0; j < DataSourceList[i].Count; j++)
                {
                    if (DataSourceList[i][j].Tag == 0) continue;
                    pointList.Add(new[] { i, j });
                    contentList.Add(DataSourceList[i][j]);
                }
            }
            foreach (int[] point in pointList)
            {
                var next = random.Next(0, contentList.Count);
                var block = contentList[next];
                block.X = point[0];
                block.Y = point[1];
                DataSourceList[block.X][block.Y] = block;
                contentList.Remove(block);
            }
        }

        #endregion

        #region AutoCommand 自动命令

        private DelegateCommand _AutoCommand;
        /// <summary>
        /// 自动命令
        /// </summary>
        public DelegateCommand AutoCommand => _AutoCommand ?? (_AutoCommand = new DelegateCommand(Auto));

        private void Auto()
        {
            Task.Run(() =>
            {
                AutoControl = true;
                while (AutoControl)
                {
                    var keepLook = KeepLook();
                    if (keepLook)
                    {
                        WinControl();
                        return;
                    }
                    _View.Dispatcher.Invoke(Remake);
                }
            });
        }

        #endregion

        /// <summary>
        /// 输
        /// </summary>
        private void LoseControl()
        {
            TimerStart = true;
            _timer.Stop();
            _level.Reset();
            TimeProgress = 100;
            _View.Dispatcher.Invoke(() => { RandomDataSource(); });
            AutoControl = false;
            _View.Show("你输了!!!",ShowType.Error);
        }
        /// <summary>
        /// 赢
        /// </summary>
        private void WinControl()
        {
            TimerStart = true;
            _timer.Stop();
            _level.NextLevel();
            _View.Dispatcher.Invoke(() => { RandomDataSource(); });
            TimeProgress = 100;
            AutoControl = false;
            _View.Show("你赢了!!!", ShowType.Success);
        }

        public override void Close()
        {
            AutoControl = false;
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= Timer_Tick;
                _timer = null;
            }
            _firstBlock = null;
            DataSourceList?.Clear();
            _groupData?.Clear();
        }
    }
}
