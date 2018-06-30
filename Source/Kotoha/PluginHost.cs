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
        }

        public void Dispose()
        {
            foreach (var keyValuePair in _instanceCache)
                keyValuePair.Value?.Dispose();
            foreach (var engine in KotohaEngines)
                engine?.Dispose(); // いらないはず？
            _container?.Dispose();
        }

        public void Initialize(string path, bool recursive)
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

            // create talker group
            foreach (var talkerGroup in KotohaTalkers.GroupBy(w => w.Engine))
                foreach (var talker in talkerGroup.Select(w => w.Id))
                    _talkerGroups.Add(talker, talkerGroup.Key);

            // create instances
            foreach (var engine in KotohaEngines)
            {
                var instance = new KotohaEngine(engine);
                _instanceCache.Add(engine.GetType().Name, instance);
            }
        }

        public KotohaEngine GetTalkEngine(string name)
        {
            return _instanceCache[_talkerGroups[name]];
        }
    }
}