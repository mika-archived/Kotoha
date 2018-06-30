using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Kotoha.Plugin;
using Kotoha.Plugin.Automation;
using Kotoha.Plugin.Automation.Controls.Interface;

using Microsoft.Win32;

namespace Kotoha.Engine.Voiceroid2
{
    public class Voiceroid2Engine : IKotohaEngine
    {
        private WpfControlFinder _controlFinder;
        private ITextBox _editor;
        private IButton _setCaretToFirstButton; // waiter
        private IButton _speechButton;

        public Process FindCurrentProcess()
        {
            return Process.GetProcessesByName("VoiceroidEditor").FirstOrDefault();
        }

        public string FindMainExecutable()
        {
            var regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\SharedDlls");
            return regKey?.GetValueNames().FirstOrDefault(w => w.EndsWith("VoiceroidEditor.exe"));
        }

        public void Initialize(IntPtr hWnd)
        {
            _controlFinder = new WpfControlFinder(hWnd);
            _editor = _controlFinder.FindTextBox();
            _speechButton = _controlFinder.FindButton(6);
            _setCaretToFirstButton = _controlFinder.FindButton(8);
        }

        public void Dispose()
        {
            _controlFinder?.Dispose();
        }

        public async Task SpeechAsync(string text, IKotohaTalker talker)
        {
            _editor.Text = $"{talker.Name}＞{text}";
            _speechButton.Click();
            // delay
            await Task.Delay(TimeSpan.FromMilliseconds(200));

            while (!_setCaretToFirstButton.IsEnabled)
                await Task.Delay(TimeSpan.FromMilliseconds(100));
        }

        public Task SaveAsAsync(string text, IKotohaTalker talker, string path)
        {
            throw new NotImplementedException();
        }
    }
}