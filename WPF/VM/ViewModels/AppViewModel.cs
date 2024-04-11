using BLL.Controllers;
using BLL.Interfaces;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Xps;
using VM.Interfaces;
using VM.Models;

namespace VM.ViewModels
{
    public class AppViewModel : BaseNotifyModel
    {
        private StatusbarViewModel? statusbar;
        public StatusbarViewModel? Statusbar
        {
            get { return statusbar; }
            set
            {
                statusbar = value;
                OnPropertyChanged("Statusbar");
            }
        }
        private IViewModel? currentView;

        public IViewModel? CurrentView
        {
            get { return currentView; }
            set 
            { 
                currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        public AppViewModel()
        {
            Statusbar = new StatusbarViewModel { Message = "Ready" };

#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
#endif          
            if(!File.Exists("settings.json"))
            {
                SettingController.NewSettingFile();
            }
            SettingController.LoadSettingFile();

            Application.Current.Exit += Current_Exit;
            CurrentView = new MainViewModel(this);

        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            SettingController.SaveSettingFile();
            LoginController.SaveLoginData();
            
        }

        public ISettingController SettingController { get; } = new SettingsController();
        public ILoginController LoginController { get; } = new LoginController();
    }
}
