using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Brotli;
using DistantStars.Client.BarrageModule.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using zlib;

namespace DistantStars.Client.BarrageModule.Barrages
{
    public class BiliBiliBarrage : BarrageBase
    {

        protected override Task InitialInfo()
        {
            return Task.Run(() => WebSocketUrl = "wss://broadcastlv.chat.bilibili.com/sub");
        }

        protected override void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            byte[] data = { 0x00, 0x00, 0x00, 0x1F, 0x00, 0x10, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x5B, 0x6F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x20, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x5D };
            _WebSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);
        }
        protected override byte[] GetFirstConnectInfo()
        {
            var o = new
            {
                clientver = "1.6.3",
                platform = "web",
                protover = 2,
                roomid = RoomNumber,
                uid = 2900344,
                type = 2,
            };
            var json = JsonConvert.SerializeObject(o);
            return Encoding.UTF8.GetBytes(json);
        }


        protected override byte[] GetSendPackage(byte[] message)
        {
            byte[] result = null;
            short head = 16;
            int cound = message.Length + head;
            short edition = 1;
            int type = 7;
            int sequence = 1;
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    var coundBytes = BitConverter.GetBytes(cound).Reverse().ToArray();
                    bw.Write(coundBytes);
                    var headBytes = BitConverter.GetBytes(head).Reverse().ToArray();
                    bw.Write(headBytes);
                    var editionBytes = BitConverter.GetBytes(edition).Reverse().ToArray();
                    bw.Write(editionBytes);
                    var typeBytes = BitConverter.GetBytes(type).Reverse().ToArray();
                    bw.Write(typeBytes);
                    var sequenceBytes = BitConverter.GetBytes(sequence).Reverse().ToArray();
                    bw.Write(sequenceBytes);
                    bw.Write(message);
                    result = ms.ToArray();
                }
            }
            return result;
        }

        protected override void AnalysisJson(string jsonMessage)
        {
            var jObject = JObject.Parse(jsonMessage);
            if (jObject["cmd"]?.ToString() == "DANMU_MSG")
            {
                var barrageInfo = new BarrageInfo();
                barrageInfo.NikeName = jObject["info"][2][1].ToString();
                barrageInfo.Content = jObject["info"][1].ToString();
                barrageInfo.JsonMessage = jsonMessage;
                OnReceiveBarrage(barrageInfo);
            }
        }


        #region 解析

        protected override async Task<List<string>> BodyToJson(Analysis analysis)
        {
            List<string> result = new List<string>();
            if (analysis.ProtocolVersion == 0)
            {
                var str = Encoding.UTF8.GetString(analysis.Body, 0, analysis.Body.Length);
                var index = str.LastIndexOf('}');
                var json = str.Substring(0, index + 1);
                result.Add(json);
            }
            else if (analysis.ProtocolVersion == 2)
            {
                var body = deCompressBytes(analysis.Body);
                var analysisList = await AnalysisPackage(body, body.Length);
                var jObjectList = await AnalysisBody(analysisList);
                result.AddRange(jObjectList);
            }
            else if (analysis.ProtocolVersion == 3)
            {
                var body = BrotliDecompress(analysis.Body);
                var analysisList = await AnalysisPackage(body, body.Length);
                var jObjectList = await AnalysisBody(analysisList);
                result.AddRange(jObjectList);
            }
            else if (analysis.ProtocolVersion == 1 && analysis.Operation == 3)
            {
                var num = BitConverter.ToInt32(analysis.Body.Take(4).Reverse().ToArray(), 0);
                var msg = $"当前房间人气:{num}";
                Console.WriteLine("==================");
                Console.WriteLine(msg);
                OnReceiveBarrage(new BarrageInfo { NikeName = "主机", Content = msg });
                Console.WriteLine("==================");
            }
            else if (analysis.ProtocolVersion == 1 && analysis.Operation == 8)
            {
                var msg = "进入房间开启心跳包";
                Console.WriteLine(msg);
                _Timer.Interval = 30000;
                _Timer.Start();
            }
            return result;
        }

        protected override void PackageToAnalysis(byte[] data, ref int analysisNumber, out Analysis model)
        {
            var packetLengthBytes = data.Skip(analysisNumber).Take(4).Reverse().ToArray();
            var packetLength = BitConverter.ToInt32(packetLengthBytes, 0);
            var protocolVersionBytes = data.Skip(analysisNumber + 6).Take(2).Reverse().ToArray();
            var protocolVersion = BitConverter.ToInt16(protocolVersionBytes, 0);
            var operationBytes = data.Skip(analysisNumber + 8).Take(4).Reverse().ToArray();
            var operation = BitConverter.ToInt32(operationBytes, 0);
            var body = data.Skip(analysisNumber + 16).Take(packetLength - 16).ToArray();
            model = new Analysis
            {
                PacketLength = packetLength,
                HeaderLength = 16,
                ProtocolVersion = protocolVersion,
                Operation = operation,
                SequenceId = 1,
                Body = body
            };
            analysisNumber += packetLength;
        }
        #region Zlib解压
        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        /// <param name="sourceByte">需要被解压缩的字节数组</param>
        /// <returns>解压后的字节数组</returns>
        private byte[] deCompressBytes(byte[] sourceByte)
        {
            MemoryStream inputStream = new MemoryStream(sourceByte);
            Stream outputStream = deCompressStream(inputStream);
            byte[] outputBytes = new byte[outputStream.Length];
            outputStream.Position = 0;
            outputStream.Read(outputBytes, 0, outputBytes.Length);
            outputStream.Close();
            inputStream.Close();
            return outputBytes;
        }
        /// <summary>
        /// 解压缩流
        /// </summary>
        /// <param name="sourceStream">需要被解压缩的流</param>
        /// <returns>解压后的流</returns>
        private Stream deCompressStream(Stream sourceStream)
        {
            MemoryStream outStream = new MemoryStream();
            ZOutputStream outZStream = new ZOutputStream(outStream);
            CopyStream(sourceStream, outZStream);
            outZStream.finish();
            return outStream;
        }
        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="input">原始流</param>
        /// <param name="output">目标流</param>
        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }
        #endregion
        /// <summary>
        /// Brotli解压
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] BrotliDecompress(byte[] data)
        {
            return data.DecompressFromBrotli();
        }


        #endregion

    }
}
