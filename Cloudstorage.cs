using System.IO;

namespace FortniteLauncher
{
    public class Cloudstorage
    {
        public static string defaultEngine = $@"{Directory.GetCurrentDirectory()}\CloudStorage\DefaultEngine.ini";
        public static string defaultGame = $@"{Directory.GetCurrentDirectory()}\CloudStorage\DefaultGame.ini";
        public static string defaultInput = $@"{Directory.GetCurrentDirectory()}\CloudStorage\DefaultInput.ini";
        public static string defaultRuntimeOptions = $@"{Directory.GetCurrentDirectory()}\CloudStorage\DefaultRuntimeOptions.ini";
    }
}
