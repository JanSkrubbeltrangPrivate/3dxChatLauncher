using System.Diagnostics;
using System.Text.Json;
using Equinox.Chatlauncher.Controllers;
using Equinox.Chatlauncher.Views;
using Microsoft.Win32;


namespace Equinox.Chatlauncher
{
    public class Program
    {
        static void Main(string[] args)
        {
            MainView MainView = new();
            MainView.Run();
        }
    }
}
