using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

using Kotoha.Plugin;
using Kotoha.Plugin.Automation;
using Kotoha.Plugin.Automation.Controls.Interface;
using Kotoha.Plugin.Helpers;
using Kotoha.Plugin.Impl;

using Microsoft.Win32;

namespace Kotoha.Engine.VOICEROID2
{
    public class Voiceroid2Engine : IKotohaEngine
    {
        private WpfControlFinder _controlFinder;
        private ITextBox _editor;
        private IButton _setCaretToFirstButton; // waiter
        private IButton _speechButton;

        // Constructor
        public Voiceroid2Engine()
        {
            // Register talkers from VOICEROID2 configuration
            // Engine's registered talkers are loaded in `Initialize` lifecycle.
            // If you want to register talkers in engine, please register in constructor.
            Talkers = new List<IKotohaTalker>();

            var standard = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AHS", "VOICEROID", "2.0", "Standard.settings");
            LoadPresets(standard, true);

            var document = new XmlDocument();
            document.Load(standard);

            var userPreset = document.SelectSingleNode("/UserSettings/VoicePreset/VoicePresetFilePath");
            if (userPreset == null)
                throw new InvalidOperationException();
            var userdir = SpecialFolderHelper.GetSpecialFolder(userPreset.SelectSingleNode("SpecialFolder")?.InnerText);
            var partialPath = userPreset.SelectSingleNode("PartialPath")?.InnerText;

            LoadPresets(Path.Combine(userdir, partialPath ?? throw new InvalidOperationException()));
        }

        private void LoadPresets(string path, bool isDeep = false)
        {
            var document = new XmlDocument();
            document.Load(path);

            var rootPath = isDeep ? "/UserSettings/VoicePreset/VoicePresets/" : "/ArrayOfVoicePreset/";
            var presets = document.SelectNodes($"{rootPath}VoicePreset");
            if (presets == null)
                throw new NullReferenceException();

            foreach (XmlNode preset in presets)
                Talkers.Add(new KotohaTalker {Engine = Name, Name = preset.SelectSingleNode("PresetName")?.InnerText});
        }

        #region IKotohaEngine

        public string Name => "VOICEROID2";
        public List<IKotohaTalker> Talkers { get; }

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

        #endregion
    }
}