using System.Runtime.Versioning;
using System.Text.Json;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.Win32;

namespace BLL.Controllers 
{
    public class LoginController: ILoginController
    {
        Dictionary<string, byte[]> loginData = new();
        public Dictionary<string, byte[]> LoginData
        {
            get { return loginData; }
            set {loginData = value; }
        }
        
        byte[]? Current;

        public bool LoadLoginData()
        {
            if (File.Exists("data.dat"))
            {
                try
                {
                    loginData = JsonSerializer.Deserialize<Dictionary<string, byte[]>>(File.ReadAllText("data.dat")) ?? new();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loading data.dat: {e.Message}");
                    return false;
                }
            }
            return true;
        }
        
        public bool SaveLoginData()
        {
            try
            {
                File.WriteAllText("data.dat", JsonSerializer.Serialize(loginData, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving data.dat: {e.Message}");
                return false;
            }
            return true;
        }

        public bool FindLogin(out string key)
        {
            key = "";
            if (Current == null)
            {
                return false;
            }
            foreach (var k in LoginData.Keys)
            {
                var data = loginData[k];
                if (data.SequenceEqual(Current))
                {
                    key = k;
                    return true;
                }
            }
            return false;
        }

        [SupportedOSPlatform("windows")]
        public bool GetLoginData(Settings Settings, out byte[]? data)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, false))
            {
                if (key != null)
                {
                    data = (Byte[]?)key.GetValue(Settings.LoginValue, new byte[0]);
                    Current = data;
                    return true;
                }
            }
            data = null;
            return false;
        }
        
        [SupportedOSPlatform("windows")]
        public void SetLoginData(Settings Settings, byte[] data)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, true))
            {
                if (key != null)
                {
                    key.SetValue(Settings.LoginValue, data);
                    key.Close();
                }
            }
        }

    }
}