using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.Common.ViewModels;
using DistantStars.Client.GameModule.Common;
using DistantStars.Client.Model.Models.Games;
using DistantStars.Client.Resource.Helpers;
using Prism.Commands;

namespace DistantStars.Client.GameModule.ViewModels
{
    public class GameToolViewModel : ViewModelBase
    {
        /// <summary>
        /// 游戏窗口句柄
        /// </summary>
        private IntPtr GameIntPtr;
        /// <summary>
        /// 游戏窗口信息
        /// </summary>
        private (Size, Point) WindowInfo;

        private int GameAreaX, GameAreaY;

        private List<List<Block>> BlockList;

        private Dictionary<int, List<Block>> BlockDictionary;
        /// <summary>
        /// 步骤集合
        /// </summary>
        public List<string> StepList { get; set; } = new List<string> { "开始", "获取游戏区域位置", "游戏区域截图", "解析分组", "消除方块", "完成" };

        #region 游戏默认值
        /// <summary>
        /// 窗口名称
        /// </summary>
        public string WindowName { get; set; } = "QQ游戏 - 连连看角色版";

        /// <summary>
        /// 游戏区域偏移值X
        /// </summary>
        private readonly int OffsetX = 14;
        /// <summary>
        /// 游戏区域偏移值Y
        /// </summary>
        private readonly int OffsetY = 181;

        /// <summary>
        /// 游戏区域宽
        /// </summary>
        private readonly int AreaWidth = 589;
        /// <summary>
        /// 游戏区域高
        /// </summary>
        private readonly int AreaHeight = 385;

        /// <summary>
        /// 游戏方块宽
        /// </summary>
        private readonly int BlockWidth = 31;
        /// <summary>
        /// 游戏方块高
        /// </summary>
        private readonly int BlockHeight = 35;

        /// <summary>
        /// 列
        /// </summary>
        private readonly int Column = 19;
        /// <summary>
        /// 行
        /// </summary>
        private readonly int Row = 11;
        #endregion

        #region int IntervalTime 间隔时间
        /// <summary>
        /// 间隔时间字段
        /// </summary>
        private int _IntervalTime = 10;
        /// <summary>
        /// 间隔时间属性
        /// </summary>
        public int IntervalTime
        {
            get => _IntervalTime;
            set => SetProperty(ref _IntervalTime, value);
        }
        #endregion

        #region int StepIndex 步骤下标
        /// <summary>
        /// 步骤下标字段
        /// </summary>
        private int _StepIndex;
        /// <summary>
        /// 步骤下标属性
        /// </summary>
        public int StepIndex
        {
            get => _StepIndex;
            set => SetProperty(ref _StepIndex, value);
        }
        #endregion

        #region StartCommand 开始命令
        /// <summary>
        /// 开始命令
        /// </summary>
        public ICommand StartCommand => new DelegateCommand<object>(Start);

        private async void Start(object obj)
        {
            {
                StepIndex = 0;
                var msg = "完成";
                var result = await Task.Run(() =>
                {
                    StepIndex++;
                    var b = GetGameArea();
                    if (!b)
                    {
                        msg = "未找到游戏窗口";
                        return b;
                    }
                    StepIndex++;
                    var bitmap = ScreenShot();
                    StepIndex++;
                    AnalysisGroup(bitmap);
                    StepIndex++;
                    return KeepLook();

                });
                if (result)
                {
                    StepIndex++;
                }
                else
                {
                    msg = "无可以连接的方块,请刷新方块位置!!!";
                    StepIndex = 0;
                }
                _View.Show(msg);
            }
        }

        bool GetGameArea()
        {
            GameIntPtr = WindowName.GetWindowIntPtr();
            if (GameIntPtr == IntPtr.Zero) return false;
            WindowInfo = GameIntPtr.GetWindowInfo();
            GameAreaX = WindowInfo.Item2.X + OffsetX;
            GameAreaY = WindowInfo.Item2.Y + OffsetY;
            return true;
        }

        Bitmap ScreenShot()
        {
            GameIntPtr.ActivateByIntPtr();
            Thread.Sleep(10);
            // 新建一个和屏幕大小相同的图片
            Bitmap bitmap = new Bitmap(AreaWidth, AreaHeight);

            // 创建一个画板，让我们可以在画板上画图
            // 这个画板也就是和屏幕大小一样大的图片
            // 我们可以通过Graphics这个类在这个空白图片上画图
            Graphics g = Graphics.FromImage(bitmap);

            // 把屏幕图片拷贝到我们创建的空白图片 CatchBmp中
            g.CopyFromScreen(new Point(GameAreaX, GameAreaY), new Point(0, 0), new Size(AreaWidth, AreaHeight));
            return bitmap;
        }

        private void AnalysisGroup(Bitmap CatchBmp)
        {
            BlockDictionary = new Dictionary<int, List<Block>>();
            BlockList = new List<List<Block>>();
            for (int i = 0; i < Column; i++)
            {
                BlockList.Add(new List<Block>());
                for (int j = 0; j < Row; j++)
                {
                    var hashValue = string.Empty;
                    for (int k = 0; k < 5; k++)
                    {
                        var offsetPixel = 10 + 2 * k;
                        var xPixel = i * BlockWidth + offsetPixel;
                        var yPixel = j * BlockHeight + offsetPixel;
                        var color = CatchBmp.GetPixel(xPixel, yPixel);
                        hashValue += color.GetHashCode();
                    }
                    var block = new Block { X = i, Y = j, Tag = hashValue.GetHashCode() };
                    BlockList[i].Add(block);
                    if (!BlockDictionary.ContainsKey(block.Tag))
                    {
                        BlockDictionary.Add(block.Tag, new List<Block>());
                    }

                    BlockDictionary[block.Tag].Add(block);
                }
            }

            var maxList = BlockDictionary.First();
            foreach (var blocks in BlockDictionary.Where(d => d.Value.Count > maxList.Value.Count))
            {
                maxList = blocks;
            }

            foreach (var block in maxList.Value)
            {
                block.Tag = 0;
            }

            BlockDictionary.Remove(maxList.Key);
        }


        bool KeepLook()
        {
            while (true)
            {
                var removeTagList = new List<int>();
                var result = false;
                foreach (var keyValue in BlockDictionary)
                {
                    for (int i = 0; i < keyValue.Value.Count; i++)
                    {
                        for (var j = i + 1; j < keyValue.Value.Count; j++)
                        {
                            if (keyValue.Value[i].Tag == 0 || keyValue.Value[j].Tag == 0) continue;
                            var b = BlockList.BlockMatch(keyValue.Value[i], keyValue.Value[j], out _);
                            if (!b) continue;
                            result = true;
                            EliminateBlock(keyValue.Value[i].X, keyValue.Value[i].Y);
                            Thread.Sleep(10);
                            EliminateBlock(keyValue.Value[j].X, keyValue.Value[j].Y);
                            Thread.Sleep(IntervalTime);
                            keyValue.Value[i].Tag = keyValue.Value[j].Tag = 0;
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
                    BlockDictionary.Remove(tag);
                }
                if (result && BlockDictionary.Count > 0) continue;
                return result;
            }
        }

        void EliminateBlock(int x, int y)
        {
            var clickX = GameAreaX + x * BlockWidth + BlockWidth / 2;
            var clickY = GameAreaY + y * BlockHeight + BlockHeight / 2;
            GameIntPtr.ActivateByIntPtr();
            new Point(clickX, clickY).LeftClick();
        }
        #endregion



    }
}
