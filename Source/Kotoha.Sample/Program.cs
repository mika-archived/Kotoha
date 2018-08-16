using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Kotoha.Plugin;

namespace Kotoha.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Run().Wait();
        }

        private static async Task Run()
        {
            try
            {
                // Create a instance of Kotoha.
                var player = new KotohaPlayer();

                // Load plugins from specified directory.
                // When you set recursive is true, Kotoha searches plugins recursively into subdirectories.
                // Support plugins are:
                //   1. Engine plugin (e.g. Kotoha.Voiceroid2.dll)
                //     Engine plugin implements IKotohaEngine interface.
                //   2. Talker plugin (e.g. Kotoha.Akane.dll)
                //     Talker plugin implements IKotohaTalker interface.
                //   3. Feature plugin (e.g. Kotoha.Tuning.dll)
                //     Feature plugin implements IKotohaFeature interface.
                player.LoadPlugins($@"{Environment.CurrentDirectory}\plugins", true);

                // Kotoha also supports config (JSON format) file for talker definition.
                // player.LoadConfigs($@"{Environment.CurrentDirectory}\plugins\talkers.json");

                // and class load. You can configure talkers dynamically.
                player.LoadClasses(new List<IKotohaTalker> {new Yukari()});

                // Initialize. You MUST initialize after loaded plugins.
                player.Initialize();

                // Play voice.
                await player.SpeechAsync("こんにちは", "琴葉 葵");
                await player.SpeechAsync("おはよう", "琴葉 茜");

                // Save voice.
                // await player.SaveAs("おはよう", "Akane", $@"{Environment.CurrentDirectory}\dist\ohayo.wav");

                // Finisihed.
                player.Dispose();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }

    internal class Yukari : IKotohaTalker
    {
        public string Id => "Yukari";
        public string Name => "結月ゆかり";
        public string Engine => "VOICEROID2";
    }
}