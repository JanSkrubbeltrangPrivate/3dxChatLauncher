using Equinox.Chatlauncher.Controllers;

namespace Equinox.Chatlauncher.Views
{
    public class MainView
    {
        public void Run()
        {
            if (!OperatingSystem.IsWindows())
            {
                Console.WriteLine("This program only works on Windows");
                return;
            }

            SettingsController SettingsController = new();
            LoginController LoginController = new();
            LaunchController LaunchController = new();
            DisplayController DisplayController = new();
           
            if (!File.Exists("settings.json"))
            {
                SettingsController.NewSettingFile();
                Console.WriteLine("Please edit the settings.json file and restart the program");
                return;
            }

            if (!SettingsController.LoadSettingFile() || !LoginController.LoadLoginData())
                return;

            string foundLoginKey = "";
            
            if(LoginController.GetLoginData(SettingsController.Settings, out byte[]? Current) && Current != null)
            {
                if (!LoginController.FindLogin(out foundLoginKey))
                {
                    if(DisplayController.StoreNewLogin(out string NewName))
                    {
                        LoginController.LoginData[NewName] = Current;
                    }
                }
            }

            if (!LoginController.SaveLogin())
                return;

            if (LoginController.LoginData.Count > 0)
            {
                var login = DisplayController.SelectLogin(LoginController.LoginData.Keys.ToArray(), foundLoginKey);
                switch (login)
                {
                    case "":
                        return;
                    case "$$Equinox.LaunchAPP.NOW££":
                        LaunchController.LaunchApplication(SettingsController.Settings);
                        break;
                    default:
                        LoginController.SetLoginData(SettingsController.Settings, LoginController.LoginData[login]);
                        LaunchController.LaunchApplication(SettingsController.Settings);
                        break;
                }
                
                
                if (login != "")
                {
                    LoginController.SetLoginData(SettingsController.Settings, LoginController.LoginData[login]);
                }
            }
            else
            {
                if(DisplayController.NoLogin())
                    LaunchController.LaunchApplication(SettingsController.Settings);
            }

        }
    }

}