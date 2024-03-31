using System.Text.Json;
using Equinox.Chatlauncher.Models;
using Microsoft.VisualBasic;

namespace Equinox.Chatlauncher.Controllers
{
    public class SettingsController
    {
        private Settings settings = new();
        public Settings Settings
        {
            get { return settings; }
            set { settings = value; }
        }
        
        public void NewSettingFile()
        {
            Models.Settings Settings = new();
            Settings.ExeFile = "C:\\3DXChat\\3DXChat.exe";
            Settings.Path = "C:\\3DXChat";
            Settings.RegistryKey = "Software\\SexGameDevil\\3DXChat";
            Settings.LoginValue = "loginData_h2649537910";
            string Json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("settings.json", Json);
        }

        public bool LoadSettingFile()
        {
            try
            {
                string Json = File.ReadAllText("settings.json");
                Settings = JsonSerializer.Deserialize<Models.Settings>(Json) ?? new();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading settings.json: {e.Message}");
                Settings = new();
                return false;
            }
        }

        public bool SaveSettingFile()
        {
            try
            {
                string Json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("settings.json", Json);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving settings.json: {e.Message}");
                return false;
            }
        }
    }
}
