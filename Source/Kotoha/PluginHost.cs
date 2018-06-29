using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Kotoha.Interfaces;

namespace Kotoha
{
    internal class PluginHost : IDisposable
    {
        private CompositionContainer _container;

        [ImportMany]
        public List<IKotohaEngine> Engines { get; set; }

        [ImportMany]
        public List<IKotohaTalker> Talkers { get; set; }

        public void Dispose()
        {
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
        }
    }
}