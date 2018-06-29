using System;
using System.Diagnostics;
using System.Windows.Controls;

using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;

using Kotoha.Plugins;

using RM.Friendly.WPFStandardControls;

namespace Kotoha
{
    public class KotohaEngine : IDisposable
    {
        private readonly IControlMapper2 _controlMapper;
        private readonly IKotohaEngine _engine;

        private Process _process;

        public KotohaEngine(IKotohaEngine engine)
        {
            _engine = engine;
            _controlMapper = _engine.ConnectionType == ConnectionType.Wpf ? new WpfControlMapper() : null;
        }

        public void Dispose()
        {
            // _application?.Dispose();
            _process?.Dispose();
        }

        public void Initialize()
        {
            _engine.Initialize(_controlMapper);

            // Search process
            _process = _engine.FindCurrentProcess() ?? Process.Start(_engine.FindMainExecutable());
        }

        public void PlayAsync(string text, string name)
        {
            var app = new WindowsAppFriend(_process);
            var main = WindowControl.FromZTop(app);

            var textBox = new WPFTextBox(main.LogicalTree().ByType<TextBox>()[(int) _controlMapper.FindByRole<TextBox>(Role.EditorText)]);
            textBox.EmulateChangeText(text);

            var button = new WPFButtonBase(main.LogicalTree().ByType<Button>()[(int) _controlMapper.FindByRole<Button>(Role.PlayButton)]);
            button.EmulateClick();
        }
    }
}