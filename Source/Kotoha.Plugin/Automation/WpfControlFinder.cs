using System;
using System.Diagnostics;
using System.Windows.Controls;

using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;

using Kotoha.Plugin.Automation.Controls.Interface;
using Kotoha.Plugin.Automation.Controls.Wpf;

using RM.Friendly.WPFStandardControls;

namespace Kotoha.Plugin.Automation
{
    public class WpfControlFinder : IDisposable
    {
        private readonly WindowsAppFriend _app;
        private readonly WindowControl _main;

        public WpfControlFinder(IntPtr hWnd)
        {
            _app = new WindowsAppFriend(hWnd);
            _main = WindowControl.FromZTop(_app);
        }

        public void Dispose()
        {
            try
            {
                _app?.Dispose();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public ITextBox FindTextBox(int index = 0)
        {
            var textBox = new WPFTextBox(_main.LogicalTree().ByType<TextBox>()[index]);
            return new WpfTextBox(textBox);
        }

        public IButton FindButton(int index = 0)
        {
            var button = new WPFButtonBase(_main.LogicalTree().ByType<Button>()[index]);
            return new WpfButton(button);
        }
    }
}