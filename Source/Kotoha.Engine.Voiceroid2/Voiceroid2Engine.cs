using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

using Kotoha.Plugins;

using Microsoft.Win32;

namespace Kotoha.Engine.Voiceroid2
{
    public class Voiceroid2Engine : IKotohaEngine
    {
        public ConnectionType ConnectionType => ConnectionType.Wpf;

        public Process FindCurrentProcess()
        {
            return Process.GetProcessesByName("VoiceroidEditor").FirstOrDefault();
        }

        public void Initialize(IControlMapper controlMapper)
        {
            controlMapper.Register<TextBox>(0, Role.EditorText);
            controlMapper.Register<Button>(6, Role.PlayButton);
            controlMapper.Register<Button>(2, Role.StopButton);
        }

        public string FindMainExecutable()
        {
            var regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\SharedDlls");
            return regKey?.GetValueNames().FirstOrDefault(w => w.EndsWith("VoiceroidEditor.exe"));
        }
    }
}