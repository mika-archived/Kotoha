using System;
using System.Diagnostics;
using System.Threading.Tasks;

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
                player.LoadConfigs($@"{Environment.CurrentDirectory}\plugins\talkers.json");

                // Initialize. You MUST initialize after loaded plugins.
                player.Initialize();

                // Play voice.
                await player.SpeechAsync("こんにちは", "Aoi");
                await player.SpeechAsync("おはよう", "Akane");

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
}