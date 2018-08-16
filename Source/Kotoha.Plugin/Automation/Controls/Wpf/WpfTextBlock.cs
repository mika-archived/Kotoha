using Kotoha.Plugin.Automation.Controls.Interface;

using RM.Friendly.WPFStandardControls;

namespace Kotoha.Plugin.Automation.Controls.Wpf
{
    public class WpfTextBlock : ITextBlock
    {
        private readonly WPFTextBlock _textBlock;

        public WpfTextBlock(WPFTextBlock textBlock)
        {
            _textBlock = textBlock;
        }

        public string Text => _textBlock.Text;
    }
}