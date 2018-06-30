using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

        public async Task SpeechAsync(string text, IKotohaTalker talker)
        {
            Initialize();
            await _engine.SpeechAsync(text, talker);
        }

        public async Task SaveAsAsync(string text, IKotohaTalker talker, string path)
        {
            Initialize();
            await _engine.SaveAsAsync(text, talker, path);
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