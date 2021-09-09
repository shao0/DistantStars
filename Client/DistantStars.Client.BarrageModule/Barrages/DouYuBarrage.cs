using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DistantStars.Client.BarrageModule.Helpers;
using DistantStars.Client.BarrageModule.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DistantStars.Client.BarrageModule.Barrages
{
    public class DouYuBarrage : BarrageBase
    {
        string RoomInfoUrl = "https://www.douyu.com/betard/";
        private JObject _gift;

        /// <summary>
        /// 主体信息转json
        /// </summary>
        /// <param name="analysis"></param>
        /// <returns></returns>
        protected override Task<List<string>> BodyToJson(Analysis analysis) => Task.Run(() => new List<string> { Encoding.UTF8.GetString(analysis.Body) });
        /// <summary>
        /// 包转解析类
        /// </summary>
        /// <param name="data"></param>
        /// <param name="analysisNumber"></param>
        /// <param name="model"></param>
        protected override void PackageToAnalysis(byte[] data, ref int analysisNumber, out Analysis model)
        {
            var packetLengthBytes = data.Skip(analysisNumber).Take(4).ToArray();
            //包总长度
            var packetLength = BitConverter.ToInt32(packetLengthBytes, 0);
            //包总长度有两遍所以跳过八位
            var protocolVersionBytes = data.Skip(analysisNumber + 8).Take(2).ToArray();
            //通信版本
            var protocolVersion = BitConverter.ToInt16(protocolVersionBytes, 0);
            //主体内容(有两位其他信息所以多跳过两位)
            var body = data.Skip(analysisNumber + 12).Take(packetLength - 10).ToArray();
            model = new Analysis
            {
                PacketLength = packetLength,
                HeaderLength = 12,
                ProtocolVersion = protocolVersion,
                SequenceId = 1,
                Body = body
            };
            analysisNumber += packetLength + 4;
        }
        /// <summary>
        /// 解析json
        /// </summary>
        /// <param name="jsonMessage"></param>
        protected override async void AnalysisJson(string jsonMessage)
        {
            try
            {
                Dictionary<string, string> dictionary = MessageSerializeDictionary(jsonMessage);
                if (!dictionary.ContainsKey("type"))
                {
                    return;
                }
                var json = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
                switch (dictionary["type"])
                {
                    case "pingreq":
                        {
                            await JoinGroup();
                            break;
                        }

                    case "chatmsg": BarrageMessage(dictionary, json); break;
                    #region 其他类型
                    //case "loginres":
                    //case "uenter":
                    //case "ranklist":
                    //case "noble_num_info":
                    //case "qausrespond":
                    //case "newblackres":
                    //case "cthn":
                    //case "mrkl":
                    //case "synexp":
                    //case "blab":
                    //case "rnewbc":
                    //case "spbc":
                    //case "gbroadcast":
                    //case "rtss_update":
                    //case "mfcdopenx":
                    //case "anbc":
                    //case "lucky_wheel_star_pool":
                    //case "configscreen":
                    //case "srres":
                    //case "frank":
                    //case "fswrank":
                    //case "frbc":
                    //case "wirt":
                    //case "rtss_":
                    //case "ro_date_succ":
                    //case "rank_change":
                    //    break;
                    //case "dgb":
                    //    var giftId = dictionary["eic"];
                    //    var jToken = GetGift(giftId);
                    //    var giftName = jToken["name"];
                    //    var giftMessage = $"感谢{dictionary["nn"]}赠送的{giftName}";
                    //    VoiceHelper.AddVoiceText(giftMessage);
                    //    Console.WriteLine(giftMessage);
                    //    break;
                    //case "tsboxb":
                    //    Console.WriteLine(msg);ViewShow?.Invoke(msg);ViewShow?.Invoke(msg);
                    //    break;
                    //case "upgrade":
                    //    Console.WriteLine(msg);ViewShow?.Invoke(msg);ViewShow?.Invoke(msg);
                    //    break;
                    //case "musicopen":
                    //    Console.WriteLine(msg);ViewShow?.Invoke(msg);ViewShow?.Invoke(msg);
                    //    break;
                    //case "rri":
                    //    Console.WriteLine(msg);ViewShow?.Invoke(msg);ViewShow?.Invoke(msg);
                    //    break;
                    //case "spbc"://礼物广播 
                    #endregion
                    case "dgb": BarrageGift(dictionary, json); break;
                    default:
                        break;
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
        /// <summary>
        /// 弹幕礼物
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="json"></param>
        private void BarrageGift(Dictionary<string, string> dictionary, string json)
        {
            string gift = dictionary["gfid"];
            string gif = string.Empty;
            var jToken = GetGift(gift);
            if (jToken?["name"] != null)
            {
                gift = jToken["name"].ToString();
                gif = dictionary["gfid"].GetGiftLocalPath("douyu", jToken["himg"].ToString());
            }

            var barrageGift = new BarrageGift();
            barrageGift.BarrageType = BarrageType.Gift;
            barrageGift.NikeName = dictionary["nn"];
            barrageGift.UserId = dictionary["uid"];
            barrageGift.GiftId = dictionary["gfid"];
            barrageGift.Content = gift;
            barrageGift.DynamicGraph = gif;
            barrageGift.JsonMessage = json;
            barrageGift.Count = int.Parse(dictionary["gfcnt"]);
            barrageGift.Continuity = int.Parse(dictionary["bcnt"]);
            OnReceiveBarrage(barrageGift);
        }
        /// <summary>
        /// 弹幕消息
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="json"></param>
        private void BarrageMessage(Dictionary<string, string> dictionary, string json)
        {
            BarrageInfo barrageInfo;
#if DEBUG
            //if (string.IsNullOrWhiteSpace(dictionary["txt"]))
            //{
            //    barrageInfo = new BarrageGift();
            //    barrageInfo.BarrageType = BarrageType.Gift;
            //}
            //else
            {
                barrageInfo = new BarrageInfo();
                barrageInfo.BarrageType = BarrageType.Message;
            }
#else 
            barrageInfo = new BarrageInfo();
            barrageInfo.BarrageType = BarrageType.Message;
#endif
            barrageInfo.NikeName = dictionary["nn"];
            barrageInfo.UserId = dictionary["uid"];
            barrageInfo.Content = dictionary["txt"];
            if (dictionary.ContainsKey("col"))
            {
                barrageInfo.BarrageColor = ToBarrageColor(dictionary["col"]);
            }
            barrageInfo.JsonMessage = json;
            OnReceiveBarrage(barrageInfo);
        }

        BarrageColor ToBarrageColor(string colorNumber)
        => (BarrageColor)int.Parse(colorNumber);
        /// <summary>
        /// 进入分组
        /// </summary>
        /// <returns></returns>
        private async Task JoinGroup()
        {
            var group = new Dictionary<string, string>();
            group.Add("type", "joingroup");
            group.Add("rid", RoomNumber.ToString());
            group.Add("gid", "1");
            var groupBytes = GetSendPackage(DictionaryToByte(@group));
            Console.WriteLine("发送加入房间包");
            //发送组信息
            await _WebSocket.SendAsync(new ArraySegment<byte>(groupBytes), WebSocketMessageType.Binary, true,
                CancellationToken.None);
            _Timer.Interval = 45000;
            //启动心跳包
            _Timer.Start();
        }

        /// <summary>
        /// 消息序列化为字典
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Dictionary<string, string> MessageSerializeDictionary(string msg)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var array in msg.Split('/').Select(s => s.Replace("@S", "/").Replace("@A", "@").Split('@', '=')))
            {
                if (array.Length > 1)
                {
                    var key = result.ContainsKey(array[0]) ? array[0] + "__" : array[0];
                    result.Add(key, array[2]);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取第一连接信息
        /// </summary>
        /// <returns></returns>
        protected override byte[] GetFirstConnectInfo()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("type", "loginreq");
            dictionary.Add("roomid", RoomNumber.ToString());
            dictionary.Add("dfl", "sn@A=107@Sss@A=1/sn@A=108@Sss@A=1/sn@A=105@Sss@A=1/@Sss@A=1");
            dictionary.Add("username", "28747444");
            dictionary.Add("uid", "28747444");
            dictionary.Add("ver", "20190610");
            dictionary.Add("aver", "218101901");
            dictionary.Add("ct", "0");
            return DictionaryToByte(dictionary);
        }
        /// <summary>
        /// 字典转bytes
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        byte[] DictionaryToByte(Dictionary<string, string> dictionary) => Encoding.UTF8.GetBytes(dictionary.Aggregate(string.Empty, (first, item) => first + $"{Escaped(item.Key)}@={Escaped(item.Value)}/") + "\0");
        /// <summary>
        /// 获取发送包
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override byte[] GetSendPackage(byte[] message)
        {
            byte[] result = new byte[message.Length + 12];
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    var coundBytes = BitConverter.GetBytes(message.Length + 8);
                    bw.Write(coundBytes);
                    bw.Write(coundBytes);
                    ushort type = 689;
                    var typeBytes = BitConverter.GetBytes(type);
                    bw.Write(typeBytes);
                    ushort e = 0;
                    var eBytes = BitConverter.GetBytes(e);
                    bw.Write(eBytes);
                    bw.Write(message);
                    result = ms.ToArray();
                }
            }
            return result;
        }
        /// <summary>
        /// 关键字符转义
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string Escaped(string source) => source.Replace("@", "@A").Replace("/", "@S");
        /// <summary>
        /// 初始化信息
        /// </summary>
        /// <returns></returns>
        protected override async Task InitialInfo()
        {
            WebSocketUrl = "wss://danmuproxy.douyu.com:8503/";
            var giftId = JObject.Parse(await GetGiftJson($"{RoomInfoUrl}{RoomNumber}"))["room"]["giftTempId"];
            var giftUrl = $"https://webconf.douyucdn.cn/resource/common/gift/gift_template/{giftId}.json";
            var giftResult = await GetGiftJson(giftUrl);
            var giftJson = giftResult.Replace("DYConfigCallback(", "").Replace("});", "}");
            _gift = JObject.Parse(giftJson);
        }
        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var heartbeat = new Dictionary<string, string>();
            heartbeat.Add("type", "mrkl");
            var heartbeatBytes = GetSendPackage(DictionaryToByte(heartbeat));
            _WebSocket.SendAsync(new ArraySegment<byte>(heartbeatBytes), WebSocketMessageType.Binary, true, CancellationToken.None);
            Console.WriteLine("发送心跳包");
        }
        /// <summary>
        /// 获取礼物信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JToken GetGift(string id)
        {
            var result = _gift["data"].FirstOrDefault(jToken => jToken["id"].ToString() == id);

            return result;
        }
        /// <summary>
        /// 获取礼物json
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        async Task<string> GetGiftJson(string url)
        {
            string result = string.Empty;
            try
            {
                result = await Task.Run(() =>
               {
                   var http = (HttpWebRequest)WebRequest.Create(url);
                   http.Method = "GET";
                   using (var response = (HttpWebResponse)http.GetResponse())
                   {
                       using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                       {
                           return sr.ReadToEnd();
                       }
                   }
               });
            }
            catch (Exception e)
            {
#if DEBUG
                //Application.Current.MainWindow.Show(e.Message,outTime:60);
#endif
                Console.WriteLine(e.Message);
            }
            return result;
        }
    }
}
