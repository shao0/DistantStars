using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DistantStars.Client.BarrageModule.Models;

namespace DistantStars.Client.BarrageModule.Barrages
{
    public abstract class BarrageBase
    {

        protected event EventHandler<BarrageInfo> _ReceiveBarrage;
        /// <summary>
        /// 接收弹幕触发
        /// </summary>
        public event EventHandler<BarrageInfo> ReceiveBarrage
        {
            add => _ReceiveBarrage += value;
            remove => _ReceiveBarrage -= value;
        }

        protected event EventHandler<bool> _ConnectStateChanged;
        /// <summary>
        /// 连接状态改变触发
        /// </summary>
        public event EventHandler<bool> ConnectStateChanged
        {
            add => _ConnectStateChanged += value;
            remove => _ConnectStateChanged -= value;
        }

        /// <summary>
        /// 客户端WebSocket
        /// </summary>
        protected ClientWebSocket _WebSocket;
        /// <summary>
        /// 心跳包轮询
        /// </summary>
        protected System.Timers.Timer _Timer;
        /// <summary>
        /// 房间号
        /// </summary>
        protected int RoomNumber;

        protected string WebSocketUrl;
        private bool _Connect;
        /// <summary>
        /// 连接
        /// </summary>
        protected bool Connect
        {
            get => _Connect;
            set
            {
                _Connect = value;
                _ConnectStateChanged?.Invoke(this, _Connect);
            }
        }

        protected AutoResetEvent _AutoResetEvent;

        protected BarrageBase()
        {
            _Timer = new System.Timers.Timer();
            _Timer.Elapsed += Timer_Elapsed;
            _AutoResetEvent = new AutoResetEvent(false);
        }
        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e);
        /// <summary>
        /// 获取发送包
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract byte[] GetSendPackage(byte[] message);
        /// <summary>
        /// 解析json
        /// </summary>
        /// <param name="jsonMessage"></param>
        protected abstract void AnalysisJson(string jsonMessage);
        /// <summary>
        /// 初始化信息
        /// </summary>
        /// <returns></returns>
        protected abstract Task InitialInfo();
        /// <summary>
        /// 包转解析类
        /// </summary>
        /// <param name="data"></param>
        /// <param name="analysisNumber"></param>
        /// <param name="model"></param>
        protected abstract void PackageToAnalysis(byte[] data, ref int analysisNumber, out Analysis model);
        /// <summary>
        /// 主体信息转json
        /// </summary>
        /// <param name="analysis"></param>
        /// <returns></returns>
        protected abstract Task<List<string>> BodyToJson(Analysis analysis);
        /// <summary>
        /// 获取第一连接信息
        /// </summary>
        /// <returns></returns>
        protected abstract byte[] GetFirstConnectInfo();
        /// <summary>
        /// 开始获取弹幕
        /// </summary>
        /// <param name="roomId"></param>
        public async void Start(int roomId)
        {
            RoomNumber = roomId;
            await InitialInfo();
            _WebSocket = new ClientWebSocket();
            await _WebSocket.ConnectAsync(new Uri(WebSocketUrl), CancellationToken.None);
            var firstBytes = GetFirstConnectInfo();
            var sendBytes = GetSendPackage(firstBytes);

            await _WebSocket.SendAsync(new ArraySegment<byte>(sendBytes), WebSocketMessageType.Binary, true, CancellationToken.None); //发送数据
            Connect = true;
            while (Connect)
            {
                try
                {
                    var result = new byte[1024 * 4];
                    var receiveResult = await _WebSocket.ReceiveAsync(new ArraySegment<byte>(result), CancellationToken.None);//接受数据
                    var analysisList = await AnalysisPackage(result, receiveResult.Count);
                    var bodyList = await AnalysisBody(analysisList);
                    foreach (var body in bodyList)
                    {
                        AnalysisJson(body);
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    //Application.Current.MainWindow.Show(e.Message, outTime: 60);
#endif
                    Console.WriteLine(e.Message);
                }
            }


        }

        /// <summary>
        /// 断开
        /// </summary>
        public void Stop()
        {
            Connect = false;
            _Timer?.Stop();
            _WebSocket?.Abort();
        }
        /// <summary>
        /// 解析包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected async Task<List<Analysis>> AnalysisPackage(byte[] data, int count)
        {
            List<Analysis> result = new List<Analysis>();
            Analysis model;
            var analysisNumber = 0;
            await Task.Run(() =>
            {
                while (analysisNumber < count)
                {
                    PackageToAnalysis(data, ref analysisNumber, out model);
                    result.Add(model);
                }
            });
            return result;
        }
        /// <summary>
        /// 解析主体信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected async Task<List<string>> AnalysisBody(List<Analysis> list)
        {
            List<string> result = new List<string>();
            foreach (var item in list)
            {
                var stringList = await BodyToJson(item);
                result.AddRange(stringList);
            }
            return result;
        }
        /// <summary>
        /// 接收弹幕
        /// </summary>
        /// <param name="info"></param>
        protected void OnReceiveBarrage(BarrageInfo info)
        {
            _ReceiveBarrage?.Invoke(this, info);
        }
        /// <summary>
        /// 继续接受
        /// </summary>
        public void ContinueReceive()
        {
            _AutoResetEvent.Set();
        }

    }
}
