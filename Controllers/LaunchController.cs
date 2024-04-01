using System.Diagnostics;
using Equinox.Chatlauncher.Interfaces;
using Equinox.Chatlauncher.Models;

namespace Equinox.Chatlauncher.Controllers
{
    public class LaunchController : ILaunchController
    {
        public void LaunchApplication(Settings Settings)
        {
            if (!File.Exists(Settings.ExeFile))
            {
                Console.WriteLine($"The application file does not exist ({Settings.ExeFile}). Please check the settings.json file and restart the program");
                Console.ReadKey(true);
                return;
            }
            var psi = new ProcessStartInfo(Settings.ExeFile)
            {
                WorkingDirectory = Settings.Path
            };
            Process.Start(psi);
        }
    }
}