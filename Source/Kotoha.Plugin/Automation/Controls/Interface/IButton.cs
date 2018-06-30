namespace Kotoha.Plugin.Automation.Controls.Interface
{
    public interface IButton
    {
        bool IsEnabled { get; }

        void Click();
    }
}