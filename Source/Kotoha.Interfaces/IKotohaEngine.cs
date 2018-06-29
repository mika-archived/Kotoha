namespace Kotoha.Interfaces
{
    public interface IKotohaEngine
    {
        /// <summary>
        ///     Find main *.exe file
        /// </summary>
        void FindMainExecutable();

        /// <summary>
        ///     Find process from currently launched processes.
        /// </summary>
        void FindCurrentProcess();
    }
}