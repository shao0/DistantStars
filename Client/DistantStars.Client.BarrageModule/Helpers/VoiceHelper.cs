using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows;

namespace DistantStars.Client.BarrageModule.Helpers
{
    public static class VoiceHelper
    {
        public static PromptBuilder _current;
        private static readonly Queue<PromptBuilder> PromptQueue;
        private static SpeechSynthesizer _speechSynthesizer;
        static  VoiceHelper()
        {
            PromptQueue = new Queue<PromptBuilder>();
            AutoPlay();
        }
        public static void VoicePlay(this string message)
        {
            PromptBuilder promptBuilder = new PromptBuilder();
            promptBuilder.AppendText(SymbolConversion(message));
            PromptQueue.Enqueue(promptBuilder);
        }
        static string  SymbolConversion( string msg) => msg.Replace("?", "问号").Replace("？", "问号").Replace("！", "叹号").Replace("!", "叹号");

        private static void AutoPlay()
        {
            ThreadPool.QueueUserWorkItem(Play);
        }

        private static void Play(object state)
        {
            while (true)
            {
                if (PromptQueue.Count > 0)
                {
                    lock (PromptQueue)
                    {
                        _current = PromptQueue.Dequeue();
                    }
                    try
                    {
                        _speechSynthesizer = new SpeechSynthesizer();
                        _speechSynthesizer.Rate = 5;
                        _speechSynthesizer.SetOutputToDefaultAudioDevice();
                        _speechSynthesizer.Speak(_current);
                        _speechSynthesizer.Dispose();
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        //Application.Current.MainWindow.Show(e.Message, outTime: 60);
#endif
                        Console.WriteLine(e.Message);
                    }
                }
                Thread.Sleep(30);
            }
        }
    }
}
