using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DistantStars.Client.Common.Helpers;
using DistantStars.Client.Common.ViewModels;
using DistantStars.Client.Resource.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DistantStars.Client.ToolModule.ViewModels
{
    public class PicturePornHubViewModel : ContentViewModelBase
    {
        /// <summary>
        /// 生成图片字节
        /// </summary>
        private byte[] _imageBytes;

        public PicturePornHubViewModel(IRegionManager region) : base(region)
        {

        }

        #region string FolderPath 文件夹地址
        /// <summary>
        /// 文件夹地址字段
        /// </summary>
        private string _FolderPath;
        /// <summary>
        /// 文件夹地址属性
        /// </summary>
        public string FolderPath
        {
            get => _FolderPath;
            set => SetProperty(ref _FolderPath, value);
        }
        #endregion


        #region string WhiteString 黑底白字
        /// <summary>
        /// 属性名称字段
        /// </summary>
        private string _whiteString;
        /// <summary>
        /// 属性名称属性
        /// </summary>
        public string WhiteString
        {
            get => _whiteString;
            set => SetProperty(ref _whiteString, value);
        }
        #endregion

        #region string BlackString 黄底黑字
        /// <summary>
        /// 黄底黑字字段
        /// </summary>
        private string _BlackString;
        /// <summary>
        /// 黄底黑字属性
        /// </summary>
        public string BlackString
        {
            get => _BlackString;
            set => SetProperty(ref _BlackString, value);
        }
        #endregion

        #region double RectangleWidth 矩形宽
        /// <summary>
        /// 矩形宽字段
        /// </summary>
        private int _RectangleWidth = 512;
        /// <summary>
        /// 矩形宽属性
        /// </summary>
        public int RectangleWidth
        {
            get => _RectangleWidth;
            set => SetProperty(ref _RectangleWidth, value);
        }
        #endregion

        #region double RectangleHeight 矩形高
        /// <summary>
        /// 矩形高字段
        /// </summary>
        private int _RectangleHeight = 512;
        /// <summary>
        /// 矩形高属性
        /// </summary>
        public int RectangleHeight
        {
            get => _RectangleHeight;
            set => SetProperty(ref _RectangleHeight, value);
        }
        #endregion

        #region int FontSize 文字大小
        /// <summary>
        /// 文字大小字段
        /// </summary>
        private int _FontSize = 10;
        /// <summary>
        /// 文字大小属性
        /// </summary>
        public int FontSize
        {
            get => _FontSize;
            set => SetProperty(ref _FontSize, value);
        }
        #endregion

        #region BitmapImage Picture 图片
        /// <summary>
        /// 图片字段
        /// </summary>
        private BitmapImage _Picture;
        /// <summary>
        /// 图片属性
        /// </summary>
        public BitmapImage Picture
        {
            get => _Picture;
            set => SetProperty(ref _Picture, value);
        }
        #endregion

        #region GenerateCommand 生成图片命令
        /// <summary>
        /// 生成图片命令
        /// </summary>
        public ICommand GenerateCommand => new DelegateCommand<object>(Generate);

        private async void Generate(object obj)
        {
            var message = _View.Show("正在生成图片...", ShowEnum.ShowLoading);
            await Task.Run(() =>
               {
                   var bitmap = new Bitmap(RectangleWidth, RectangleHeight, PixelFormat.Format32bppArgb);
                   Graphics g = Graphics.FromImage(bitmap);
                   g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; //设置图片质量
                   g.Clear(Color.Black);
                   var white = new SolidBrush(Color.White);
                   var black = new SolidBrush(Color.Black);
                   var yellow = new SolidBrush(Color.FromArgb(253, 152, 39));
                   var font = new Font("雅黑", FontSize, FontStyle.Bold);
                   SizeF whiteStringSize = g.MeasureString(WhiteString, font);
                   SizeF blackStringSize = g.MeasureString(BlackString, font);
                   GetCoordinate(RectangleWidth, RectangleHeight, whiteStringSize, blackStringSize, out var parameter);
                   g.FillRectangle(yellow, parameter.RectangleX, parameter.RectangleY, parameter.RectangleWidth,
                       parameter.RectangleHeight);
                   g.DrawString(WhiteString, font, white, parameter.WhiteX, parameter.WhiteY);
                   g.DrawString(BlackString, font, black, parameter.BlackX, parameter.BlackY);
                   using (var ms = new MemoryStream())
                   {
                       bitmap.Save(ms, ImageFormat.Jpeg);
                       _imageBytes = ms.GetBuffer();
                       Picture = _imageBytes.ConvertBitmapImage();
                   }
               });
            message.Close();
            _View.Show("生成完成");
        }

        #endregion

        #region SaveCommand 保存命令
        /// <summary>
        /// 保存命令
        /// </summary>
        public ICommand SaveCommand => new DelegateCommand<object>(Save);

        private void Save(object obj)
        {
            if (_imageBytes?.Length > 0)
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                var imagePath = $"{FolderPath}\\{WhiteString}{BlackString}.jpg";
                using (var ms = new MemoryStream(_imageBytes))
                {
                    using (var fs = new FileStream(imagePath, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                    }
                }

                _View.Show($"保存完成:{imagePath}");
            }
            else
            {
                _View.Show("未生成图片");
            }
        }

        #endregion

        #region SelectedFolderCommand 选择文件夹命令
        /// <summary>
        /// 选择文件夹命令
        /// </summary>
        public ICommand SelectedFolderCommand => new DelegateCommand<object>(SelectedFolder);

        private void SelectedFolder(object obj)
        {
            var folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                FolderPath = folder.SelectedPath;
            }
        }

        #endregion


        void GetCoordinate(int w, int h, SizeF whiteStringSize, SizeF blackStringSize, out BitmapParameter parameter)
        {
            parameter = new BitmapParameter();
            parameter.RectangleX = 4;
            parameter.RectangleY = h / 2 + parameter.RectangleX;
            parameter.RectangleWidth = w - 2 * parameter.RectangleX;
            parameter.RectangleHeight = h / 2 - 2 * parameter.RectangleX;
            parameter.WhiteX = (int)(w - whiteStringSize.Width) / 2;
            parameter.WhiteY = (int)(h / 2 - whiteStringSize.Height) / 2;
            parameter.BlackX = (int)(w - blackStringSize.Width) / 2;
            parameter.BlackY = (int)(h / 2 - blackStringSize.Height) / 2 + h / 2;
        }


        public override void Close()
        {
            _imageBytes = null;
        }
    }
    struct BitmapParameter
    {
        public int RectangleX;
        public int RectangleY;
        public int RectangleWidth;
        public int RectangleHeight;
        public int WhiteX;
        public int WhiteY;
        public int BlackX;
        public int BlackY;
    }

}
