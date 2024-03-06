using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Win32;


if (!OperatingSystem.IsWindows())
{
    Console.WriteLine("This program only works on Windows");
    return;
}

Dictionary<string, byte[]> LoginData = new();
Settings Settings = new();
var JsonOptions = new JsonSerializerOptions { WriteIndented = true};

if(!File.Exists("settings.json"))
{
    File.WriteAllText("settings.json", JsonSerializer.Serialize(Settings, JsonOptions));
    Console.WriteLine("Please edit the settings.json file and restart the program");
    return;
}

LoadData();

string foundLoginKey = "";
if (GetLoginData(out byte[]? Current) && Current != null)
{
    if (!FindLogin(out foundLoginKey))
    {
        StoreNewLogin(Current);
    }
};

SaveData();
        
var login = GetLogin(LoginData.Keys.ToArray());

if (login != "")
{
    SetLoginData(LoginData[login]);
    LaunchApplication();
}

void LaunchApplication()
{
    if(!File.Exists(Settings.ExeFile))
    {
        Console.WriteLine($"The application file does not exist ({Settings.ExeFile}). Please check the settings.json file and restart the program");
        Console.ReadKey();
        return;
    }
    var psi = new ProcessStartInfo(Settings.ExeFile)
    {
        WorkingDirectory = Settings.Path
    };
    Process.Start(psi);
}

bool FindLogin(out string key)
{
    key = "";
    foreach (var k in LoginData.Keys)
    {
        var data = LoginData[k];
        if (data.SequenceEqual(Current))
        {
            key = k;
            return true;
        }
    }
    return false;
}

string GetLogin(string[] LoginKeys)
{
    for (int i = 0; i < LoginKeys.Length; i++)
    {
        Console.WriteLine($"{i + 1}:\t{LoginKeys[i]}");
    }
    Console.WriteLine("Enter the number of the login you want to use (q to quit)");
    var key = Console.ReadKey();

    if (key.Key == ConsoleKey.Q)
    {
        return "";
    }

    if (int.TryParse(key.KeyChar.ToString(), out int index))
    {
        if (index > 0 && index <= LoginKeys.Length)
        {
            return LoginKeys[index - 1];
        }
    }
    return "";
}


void LoadData()
{
    if (File.Exists("data.dat"))
    {
        LoginData = JsonSerializer.Deserialize<Dictionary<string, byte[]>>(File.ReadAllText("data.dat")) ?? new();
    }
    if(File.Exists("settings.json"))
    {
        Settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json")) ?? new();
    }
}

void SaveData()
{
    File.WriteAllText("data.dat", JsonSerializer.Serialize(LoginData, JsonOptions));
}

bool StoreNewLogin(byte[] data)
{
    Console.WriteLine("New Login found. Do you want to store it? (y/n)");
    var key = Console.ReadKey();
    if (key.Key == ConsoleKey.Y)
    {

        Console.WriteLine("\n\rEnter a name for this login");
        string name = Console.ReadLine() ?? "Default";
        LoginData[name] = data;
        return true;
    }
    return false;
}

bool GetLoginData(out byte[]? data)
{
    using (var key = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, false))
    {
        if (key != null)
        {
            data = (Byte[]?)key.GetValue(Settings.LoginValue, new byte[0]);
            return true;
        }
    }
    data = null;
    return false;
}


void SetLoginData(byte[] data)
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
