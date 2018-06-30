using System;
using System.Linq;

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

        public void Speech(string text, string name)
        {
            var engine = _pluginHost.GetTalkEngine(name);
            engine.Speech(text, _pluginHost.KotohaTalkers.SingleOrDefault(w => w.Id == name));
        }

        public void SaveAs(string text, string name, string path)
        {
            var engine = _pluginHost.GetTalkEngine(name);
            engine.SaveAs(text, _pluginHost.KotohaTalkers.SingleOrDefault(w => w.Id == name), path);
        }
    }
}