using BLL.Controllers;
using BLL.Enums;
using BLL.Interfaces;

namespace _3dxChatLauncher.Views
{
    public class MainView
    {
        public void MainMenu()
        {
            ISettingController SettingsController = new SettingsController();
            ILoginController LoginController = new LoginController();
            ILaunchController LaunchController = new LaunchController();
            IDisplayController DisplayController = new DisplayController();
           
            if (!File.Exists("settings.json"))
            {
                SettingsController.NewSettingFile();
                Console.WriteLine("Please edit the settings.json file and restart the program");
                return;
            }

            if (!SettingsController.LoadSettingFile() || !LoginController.LoadLoginData())
                return;

            string FoundLoginKey = "";
            
            if(LoginController.GetLoginData(SettingsController.Settings, out byte[]? CurrentData) && CurrentData != null)
            {
                if (!LoginController.FindLogin(out FoundLoginKey))
                {
                    if(DisplayController.StoreNewLogin(out string NewName))
                    {
                        LoginController.LoginData[NewName] = CurrentData;
                    }
                }
            }

            if (!LoginController.SaveLoginData())
                return;

            if (LoginController.LoginData.Count > 0)
            {
                switch(DisplayController.SelectLogin(LoginController.LoginData.Keys.ToArray(), FoundLoginKey, out string Login))
                {
                    case SelectLoginChoice.Quit:
                        break;
                    case SelectLoginChoice.Launch:
                        LaunchController.LaunchApplication(SettingsController.Settings);
                        break;
                    case SelectLoginChoice.Found:
                        LoginController.SetLoginData(SettingsController.Settings, LoginController.LoginData[Login]);
                        LaunchController.LaunchApplication(SettingsController.Settings);
                        break;
                }
            }
            else
            {
                if(DisplayController.LauncApplicationOnNoLoginFound())
                    LaunchController.LaunchApplication(SettingsController.Settings);
            }
        }
    }
}