using Kotoha.Interfaces;

namespace Kotoha.Engine.Voiceroid2
{
    public class Voiceroid2Engine : IKotohaEngine
    {
        public void FindMainExecutable()
        {
            // Registry: コンピューター\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\SharedDlls
        }

        public void FindCurrentProcess()
        {
            // Process: VOICEROID2
        }
    }
}