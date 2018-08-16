using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Kotoha.Plugin;
using Kotoha.Plugin.Impl;

using Utf8Json;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace Kotoha
{
    internal class PluginHost : IDisposable
    {
        private readonly Dictionary<string, KotohaEngine> _instanceCache;
        private readonly Dictionary<string, string> _talkerGroups;
        private CompositionContainer _container;

        [ImportMany]
        public List<IKotohaEngine> KotohaEngines { get; set; }

        [ImportMany]
        public List<IKotohaTalker> KotohaTalkers { get; set; }

        public PluginHost()
        {
            _instanceCache = new Dictionary<string, KotohaEngine>();
            _talkerGroups = new Dictionary<string, string>();

            KotohaEngines = new List<IKotohaEngine>();
            KotohaTalkers = new List<IKotohaTalker>();
        }

        public void Dispose()
        {
            foreach (var keyValuePair in _instanceCache)
                keyValuePair.Value?.Dispose();
            foreach (var engine in KotohaEngines)
                engine?.Dispose(); // いらないはず？
            _container?.Dispose();
        }

        public void LoadAssemblies(string path, bool recursive)
        {
            var builder = new RegistrationBuilder();
            builder.ForTypesDerivedFrom<IKotohaEngine>().ExportInterfaces();
            builder.ForTypesDerivedFrom<IKotohaTalker>().ExportInterfaces();
            builder.ForType<PluginHost>().Export();

            var catalog = new AggregateCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly(), builder));

            foreach (var file in Directory.EnumerateFiles(path, "*.dll", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                try
                {
                    var asmCatalog = new AssemblyCatalog(Assembly.LoadFile(file), builder);
                    if (asmCatalog.Parts.Any())
                        catalog.Catalogs.Add(asmCatalog);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
        }

        public void LoadJsonConfig(string path)
        {
            var talkers = JsonSerializer.Deserialize<List<KotohaTalker>>(new StreamReader(path).ReadToEnd());
            LoadClasses(talkers);
        }

        public void LoadClasses(IEnumerable<IKotohaTalker> talkers)
        {
            foreach (var talker in talkers)
                KotohaTalkers.Add(talker);
        }

        public void Initialize()
        {
            // create instances
            foreach (var engine in KotohaEngines)
            {
                // register auto-detected talkers
                foreach (var talker in engine.Talkers)
                    KotohaTalkers.Add(talker);

                var instance = new KotohaEngine(engine);
                _instanceCache.Add(engine.Name, instance);
            }

            // create talker group (ignore ID == null)
            foreach (var talkerGroup in KotohaTalkers.Where(w => w.Name != null).GroupBy(w => w.Engine))
                foreach (var talker in talkerGroup.Select(w => w.Name))
                    _talkerGroups.Add(talker, talkerGroup.Key);
        }

        public KotohaEngine GetTalkEngine(string name)
        {
            return _instanceCache[_talkerGroups[name]];
        }
    }
}