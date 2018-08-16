using System;

namespace Kotoha.Plugin.Helpers
{
    public static class SpecialFolderHelper
    {
        public static string GetSpecialFolder(string folder)
        {
            var specialFolder = (Environment.SpecialFolder) Enum.Parse(typeof(Environment.SpecialFolder), folder);
            return Environment.GetFolderPath(specialFolder);
        }
    }
}