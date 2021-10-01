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
using DistantStars.Client.Resource.Data.Enum;
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

        private int GameAreaX, GameAreaY;

        private List<List<Block>> BlockList;

        private Dictionary<int, List<Block>> BlockDictionary;
        /// <summary>
        /// 步骤集合
        /// </summary>
        public List<string> StepList { get; set; } = new List<string> { "就绪", "获取游戏区域位置", "游戏区域截图", "解析分组", "消除方块" };

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

        public override void LoadedContinue()
        {
            base.LoadedContinue();

            BlockDictionary = new Dictionary<int, List<Block>>();
            BlockList = new List<List<Block>>();
        }

        #region int IntervalTime 间隔时间
        /// <summary>
        /// 间隔时间字段
        /// </summary>
        private int _IntervalTime = 1;
        /// <summary>
        /// 间隔时间属性
        /// </summary>
        public int IntervalTime
        {
            get => _IntervalTime;
            set
            {
                if (value < 0) value = 0;
                SetProperty(ref _IntervalTime, value);
            }
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
        public ICommand StartCommand => new DelegateCommand(Start);

        private async void Start()
        {
            var msg = "完成";
            var showType = ShowType.Success;
            await Task.Run(() =>
            {
                StepIndex++;
                if (!GetGameArea())
                {
                    msg = "未找到游戏窗口";
                    showType = ShowType.Error;
                    return;
                }
                StepIndex++;
                var bitmap = ScreenShot();
                StepIndex++;
                AnalysisGroup(bitmap);
                if (BlockDictionary.Count == 0)
                {
                    msg = "无可消除的方块";
                    showType = ShowType.Warning;
                    return;
                }
                StepIndex++;
                if (!KeepLook())
                {
                    msg = "无可以连接的方块,请刷新方块位置!!!";
                    showType = ShowType.Warning;
                }
            });
            _View.Show(msg, showType);
            StepIndex = 0;
            BlockDictionary.Clear();
            BlockList.Clear();
        }
        /// <summary>
        /// 获取游戏区域位置
        /// </summary>
        /// <returns></returns>
        bool GetGameArea()
        {
            GameIntPtr = WindowName.GetWindowIntPtr();
            if (GameIntPtr == IntPtr.Zero) return false;
            var windowInfo = GameIntPtr.GetWindowInfo();
            GameAreaX = windowInfo.Item2.X + OffsetX;
            GameAreaY = windowInfo.Item2.Y + OffsetY;
            return true;
        }
        /// <summary>
        /// 截取游戏区域图片
        /// </summary>
        /// <returns></returns>
        Bitmap ScreenShot()
        {
            GameIntPtr.ActivateByIntPtr();

            Thread.Sleep(10);
            // 新建图片
            Bitmap bitmap = new Bitmap(AreaWidth, AreaHeight);

            // 创建画板
            Graphics g = Graphics.FromImage(bitmap);

            // 把屏幕图片拷贝到bitmap中
            g.CopyFromScreen(new Point(GameAreaX, GameAreaY), new Point(0, 0), new Size(AreaWidth, AreaHeight));

            return bitmap;
        }
        /// <summary>
        /// 解析分组
        /// </summary>
        /// <param name="bitmap"></param>
        private void AnalysisGroup(Bitmap bitmap)
        {
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
                        var color = bitmap.GetPixel(xPixel, yPixel);
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

        /// <summary>
        /// 连接消除
        /// </summary>
        /// <returns></returns>
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
                            Thread.Sleep(1);
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

                if (result && BlockDictionary.Count > 0)
                {
                    continue;
                }
                return result || BlockDictionary.Count == 0;
            }
        }
        /// <summary>
        /// 点击消除
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
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
