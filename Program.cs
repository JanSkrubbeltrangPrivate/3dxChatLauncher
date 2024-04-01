using Equinox.Chatlauncher.Views;

namespace Equinox.Chatlauncher
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (!OperatingSystem.IsWindows())
            {
                Console.WriteLine("This program only works on Windows");
                return;
            }

            MainView MainView = new();
            MainView.Run();
        }
    }
}
