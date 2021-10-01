using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;

namespace DistantStars.Client.Common.Helpers
{
    public static class WindowsApiHelper
    {
        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 窗口坐标
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// 最左坐标
            /// </summary>
            public int Left;
            /// <summary>
            /// 最上坐标
            /// </summary>
            public int Top;
            /// <summary>
            /// 最右坐标
            /// </summary>
            public int Right;
            /// <summary>
            /// 最下坐标
            /// </summary>
            public int Bottom;
        }
        /// <summary>
        /// 设置窗口为激活状态
        /// </summary>
        /// <param name="hwnd"></param>
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);
        const int MOUSEEVENTF_MOVE = 0x0001;// 移动鼠标
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;// 模拟鼠标左键按下
        const int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;// 模拟鼠标右键按下
        const int MOUSEEVENTF_RIGHTUP = 0x0010;// 模拟鼠标右键抬起
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起
        const int MOUSEEVENTF_ABSOLUTE = 0x8000; //标示是否采用绝对坐标
        /// <summary>
        /// 鼠标输入
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="cButtons"></param>
        /// <param name="dwExtraInfo"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        /// <summary>
        /// 设置鼠标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [DllImport("User32")]
        public static extern void SetCursorPos(int x, int y);
        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static IntPtr GetWindowIntPtr(this string windowName, string className = null)
        {
            return FindWindow(className, windowName);
        }
        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static (Size, Point) GetWindowInfoByName(this string windowName, string className = null)
        {
            return FindWindow(className, windowName).GetWindowInfo();
        }
        /// <summary>
        /// 获取窗口信息
        /// </summary>
        /// <param name="intPtr"></param>
        /// <returns></returns>
        public static (Size, Point) GetWindowInfo(this IntPtr intPtr)
        {
            var rect = new RECT();
            GetWindowRect(intPtr, ref rect);
            var size = new Size();
            var point = new Point();
            size.Width = rect.Right - rect.Left;
            size.Height = rect.Bottom - rect.Top;
            point.X = rect.Left;
            point.Y = rect.Top;
            return (size, point);
        }
        /// <summary>
        /// 根据句柄激活窗口
        /// </summary>
        /// <param name="intPtr"></param>
        public static void ActivateByIntPtr(this IntPtr intPtr)
        {
            SetForegroundWindow(intPtr);
        }
        /// <summary>
        /// 左键点击
        /// </summary>
        /// <param name="point"></param>
        public static void LeftClick(this Point point)
        {
            SetCursorPos(point.X, point.Y);
            Thread.Sleep(1);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        /// <summary>
        /// 左键点击
        /// </summary>
        /// <param name="point"></param>
        public static void RightClick(this Point point)
        {
            SetCursorPos(point.X, point.Y);
            Thread.Sleep(1);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
    }
}
