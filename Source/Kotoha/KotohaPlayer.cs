using System;
using System.Threading.Tasks;

namespace Kotoha
{
    public class KotohaPlayer : IDisposable
    {
        private readonly PluginHost _pluginHost;

        public KotohaPlayer()
        {
            _pluginHost = new PluginHost();
        }

        public void Dispose()
        {
            _pluginHost?.Dispose();
        }

        public void Initialize() { }

        public void LoadPlugins(string directory, bool recursive = false)
        {
            _pluginHost.Initialize(directory, recursive);
        }

        public async Task PlayAsync(string text, string talker) { }

        public async Task SaveAsync(string text, string talker, string path) { }
    }
}