using Kotoha.Plugin.Automation.Controls.Interface;

using RM.Friendly.WPFStandardControls;

namespace Kotoha.Plugin.Automation.Controls.Wpf
{
    internal class WpfButton : IButton
    {
        private readonly WPFButtonBase _button;

        public WpfButton(WPFButtonBase button)
        {
            _button = button;
        }

        public bool IsEnabled => _button.IsEnabled;

        public void Click()
        {
            _button.EmulateClick();
        }
    }
}