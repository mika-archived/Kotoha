using System;
using System.Diagnostics;

using Kotoha.Plugin;

namespace Kotoha
{
    public class KotohaEngine : IDisposable
    {
        private readonly IKotohaEngine _engine;
        private bool _isInitialized;

        private Process _process;

        public KotohaEngine(IKotohaEngine engine)
        {
            _engine = engine;
            _isInitialized = false;
        }

        public void Dispose()
        {
            _engine?.Dispose();
            _process?.Dispose();
        }

        public void Speech(string text, IKotohaTalker talker)
        {
            Initialize();
            _engine.Speech(text, talker);
        }

        public void SaveAs(string text, IKotohaTalker talker, string path)
        {
            Initialize();
        }

        private void Initialize()
        {
            // Search process
            if (_isInitialized)
                return;
            _process = _engine.FindCurrentProcess() ?? Process.Start(_engine.FindMainExecutable());
            if (_process == null)
                throw new NullReferenceException("Cannot attach or launch to process.");

            _engine.Initialize(_process.MainWindowHandle);
            _isInitialized = true;
        }
    }
}