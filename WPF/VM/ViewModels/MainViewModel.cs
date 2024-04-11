using BLL.Controllers;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VM.Interfaces;
using VM.Models;
using VM.Views;

namespace VM.ViewModels
{
    public class MainViewModel : BaseNotifyModel, IViewModel
    {
        public IViewModel? Parent { private set; get; }

        public AppViewModel App { private set; get; }

        public MainViewModel(AppViewModel App)
        {
            this.App = App;
            Launch = new DelegateCommandResolverAsync(OnLaunch);
            App.LoginController.LoadLoginData();
            if(App.LoginController.GetLoginData(App.SettingController.Settings, out byte[]? loginData))
            {
                if(!App.LoginController.FindLogin(out string FoundLogin) && loginData != null)
                {
                    NewLogin nl = new();
                    nl.NewName.Text = "";
                    if(nl.ShowDialog() ?? false)
                    {
                        App.LoginController.LoginData[nl.NewName.Text] = loginData;
                    }
                }
            }
            
            foreach (string key in App.LoginController.LoginData.Keys)
            {
                Logins.Add(key);
            }
        }

        private ObservableCollection<string> logins = new();

        public ObservableCollection<string> Logins
        {
            get { return logins; }
            set { logins = value; OnPropertyChanged("Logins"); }
        }

        private string selectedItem = string.Empty;
        public string SelectedItem { get { return selectedItem; } set { selectedItem = value; OnPropertyChanged("SelectedItem"); } }

        public ICommand Launch { get; }

        private async Task OnLaunch()
        {
            if(!SelectedItem.Equals(string.Empty)) 
            {
                App.LoginController.SetLoginData(App.SettingController.Settings, App.LoginController.LoginData[SelectedItem]);
                LaunchController l = new();
                l.LaunchApplication(App.SettingController.Settings);
            }
        }

    }
}
