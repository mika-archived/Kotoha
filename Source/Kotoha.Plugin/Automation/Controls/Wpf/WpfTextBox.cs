using Kotoha.Plugin.Automation.Controls.Interface;

using RM.Friendly.WPFStandardControls;

namespace Kotoha.Plugin.Automation.Controls.Wpf
{
    internal class WpfTextBox : ITextBox
    {
        private readonly WPFTextBox _textBox;

        public WpfTextBox(WPFTextBox textBox)
        {
            _textBox = textBox;
        }

        public string Text
        {
            get => _textBox.Text;
            set => _textBox.EmulateChangeText(value);
        }
    }
}