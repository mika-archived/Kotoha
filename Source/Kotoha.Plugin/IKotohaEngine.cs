using System.Diagnostics;

namespace Kotoha.Plugins
{
    public interface IKotohaEngine
    {
        /// <summary>
        ///     UI component type.
        /// </summary>
        ConnectionType ConnectionType { get; }

        /// <summary>
        ///     Mapping UI controls to keys.
        /// </summary>
        /// <param name="controlMapper"></param>
        void Initialize(IControlMapper controlMapper);

        /// <summary>
        ///     Find main *.exe file
        /// </summary>
        string FindMainExecutable();

        /// <summary>
        ///     Find process from currently launched processes.
        /// </summary>
        Process FindCurrentProcess();
    }
}