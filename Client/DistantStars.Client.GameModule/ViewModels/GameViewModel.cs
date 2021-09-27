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
using DistantStars.Client.Model.Models.Games;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace DistantStars.Client.GameModule.ViewModels
{
    public class GameViewModel : ContentViewModelBase
    {
        FrameworkElement View;

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        ///     数据源
        /// </summary>
        public ObservableCollection<ObservableCollection<Block>> DataSourceList { get; set; } =
            new ObservableCollection<ObservableCollection<Block>>();
        /// <summary>
        /// 分组数据
        /// </summary>
        readonly Dictionary<int, List<Block>> _groupData = new Dictionary<int, List<Block>>();

        /// <summary>
        /// 关卡级别
        /// </summary>
        private Level _level;

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

        public GameViewModel(IEventAggregator eventAggregator, IRegionManager regionManager) : base(regionManager)
        {
            _eventAggregator = eventAggregator;
            _level = new Level();
        }

        #region LoadCommand 加载命令

        private DelegateCommand<object> _LoadCommand;
        /// <summary>
        /// 命令名称命令
        /// </summary>
        public DelegateCommand<object> LoadCommand => _LoadCommand ?? (_LoadCommand = new DelegateCommand<object>(Load));

        private void Load(object obj)
        {
            if (obj is FrameworkElement w)
            {
                View = w;
                Application.Current.MainWindow.Activated += MainWindow_Activated;
                RandomDataSource(true);
                InitialTimer();
            }
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            View.Focus();
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
                var match = GetMatch(_firstBlock, obj, out var pathPoints);
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

        #region 判断消除
        //垂直 X 相等  取 最小y -> 最大y 遍历，判断 value是否为0  遇到不是0返回false
        public bool Vertical(Block a, Block b)
        {
            if (a.X != b.X) //垂直遍历 x必须相同
            {
                return false;
            }

            int yMax = Math.Max(a.Y, b.Y) - 1; //忽略自己
            int yMin = Math.Min(a.Y, b.Y) + 1; //从相邻的开始， 忽略自己

            for (int i = yMin; i <= yMax; i++)
            {
                if (DataSourceList[a.X][i].Tag != 0) //0代表 空
                {
                    return false;
                }
            }

            return true;
        }
        //水平 y 相等  取 最小x -> 最大X 遍历，判断 value是否为0  遇到不是0返回false
        public bool Horizontal(Block a, Block b)
        {
            if (a.Y != b.Y) //垂直遍历 x必须相同
            {
                return false;

            }
            int xMax = Math.Max(a.X, b.X) - 1; //忽略自己
            int xMin = Math.Min(a.X, b.X) + 1; //从相邻的开始， 忽略自己

            for (int i = xMin; i <= xMax; i++)
            {
                if (DataSourceList[i][a.Y].Tag != 0) //0代表 空
                {
                    return false;
                }
            }

            return true;
        }
        public bool OnePoint(Block a, Block b, out Block[] pathPoints)
        {

            Block pointC = new Block() { X = a.X, Y = b.Y }; //x 和 a相同  ，y和b相同
            Block pointD = new Block() { X = b.X, Y = a.Y };
            pathPoints = null;
            bool result = false;
            if (DataSourceList[pointC.X][pointC.Y].Tag == 0)
            {
                pathPoints = new[] { a, pointC, b };
                result = Vertical(a, pointC) && Horizontal(pointC, b);
            }
            if (!result && DataSourceList[pointD.X][pointD.Y].Tag == 0)
            {
                pathPoints = new[] { a, pointD, b };
                result = Horizontal(a, pointD) && Vertical(pointD, b); //从 a-->c 再 从c-->b
            }
            return result;

        }
        public bool TwoPointPath(Block a, Block b, out Block[] pathPoints)
        {
            pathPoints = null;
            //找一点 满足 
            for (int i = 0; i < DataSourceList.Count; i++)
            {
                for (int j = 0; j < DataSourceList[i].Count; j++)
                {
                    if (a.X == i && a.Y == j || b.X == i && b.Y == j) continue;
                    //if (i != a.X && i != b.X && j != a.Y && j != b.Y)
                    if (DataSourceList[i][j].Tag == 0)
                    {
                        // 这个点 是a的一点拐点，且是 b的 垂直或者水平点
                        //或者这个点是 b的一点拐点， 且 是 a的垂直点 或者水平点
                        var c = new Block() { Y = j, X = i };
                        if (OnePoint(a, c, out pathPoints) && (Vertical(c, b) || Horizontal(c, b)) || OnePoint(c, b, out pathPoints) && (Vertical(a, c) || Horizontal(a, c)))
                        {
                            var blocks = pathPoints.ToList();
                            blocks.Insert(0, a);
                            blocks.Add(b);
                            pathPoints = blocks.ToArray();
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public bool GetMatch(Block a, Block b, out Block[] pathPoints)
        {

            bool isMatch = false;
            pathPoints = null;
            if (a.X == b.X || a.Y == b.Y)  //处理  位置 处于  十字线情况  不存在 一拐点
            {
                isMatch = a.X == b.X ? Vertical(a, b) : Horizontal(a, b);
                if (isMatch)
                {
                    pathPoints = new[] { a, b };
                    return isMatch;
                }


                isMatch = TwoPointPath(a, b, out pathPoints);

                return isMatch;
            }

            isMatch = OnePoint(a, b, out pathPoints);
            if (isMatch)
            {
                return isMatch;
            }

            isMatch = TwoPointPath(a, b, out pathPoints);

            return isMatch;
        }
        #endregion
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
                        let match = GetMatch(block1, block2, out _)
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
                var count = _groupData.Sum(l => l.Value.Count);
                AutoControl = count > 0;
                while (AutoControl)
                {
                    GetJoin(out var block1, out var block2);
                    _firstBlock = block1;
                    Checked(block2);
                    if (count == _groupData.Sum(l => l.Value.Count))
                    {
                        View.Dispatcher.Invoke(Remake);
                    }
                    Thread.Sleep(200);
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
            View.Dispatcher.Invoke(() => { RandomDataSource(); });
            AutoControl = false;
            //_View.Show("你输了!!!");
        }
        /// <summary>
        /// 赢
        /// </summary>
        private void WinControl()
        {
            TimerStart = true;
            _timer.Stop();
            _level.NextLevel();
            View.Dispatcher.Invoke(() => { RandomDataSource(); });
            TimeProgress = 100;
            AutoControl = false;
            //_View.Show("你赢了!!!");
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
