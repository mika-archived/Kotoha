namespace Kotoha.Plugins
{
    public interface IControlMapper
    {
        // for WPF
        void Register<T>(object key, Role role);

        // for Win32
        void Register(object key, Role role);
    }
}