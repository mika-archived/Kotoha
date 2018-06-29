using System;

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

        public void LoadPlugins(string directory, bool recursive = false)
        {
            _pluginHost.Initialize(directory, recursive);
        }

        public void PlayAsync(string text, string name)
        {
            var engine = _pluginHost.GetTalkEngine(name);
            engine.PlayAsync(text, name);
        }

        public void SaveAsync(string text, string talker, string path) { }
    }
}