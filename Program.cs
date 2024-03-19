using System.Diagnostics;
using System.Text.Json;
using Microsoft.Win32;

if (!OperatingSystem.IsWindows())
{
    Console.WriteLine("This program only works on Windows");
    return;
}

Dictionary<string, byte[]> LoginData = new();
Settings Settings = new();
var JsonOptions = new JsonSerializerOptions { WriteIndented = true };

if (!File.Exists("settings.json"))
{
    NewSettingFile();
    Console.WriteLine("Please edit the settings.json file and restart the program");
    return;
}

if (!LoadData())
    return;

string foundLoginKey = "";
if (GetLoginData(out byte[]? Current) && Current != null)
{
    if (!FindLogin(out foundLoginKey))
    {
        StoreNewLogin(Current);
    }
};

if (!SaveData())
    return;

if (LoginData.Count > 0)
{
    var login = GetLogin(LoginData.Keys.ToArray(), foundLoginKey);
    if (login != "")
    {
        SetLoginData(LoginData[login]);
        LaunchApplication();
    }
}
else
{
    NoLogin();
}

void NoLogin()
{
    Console.WriteLine("No logins found! (q to quit / l to launch the game)");

    while (true)
    {
        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.Q:
                return;

            case ConsoleKey.L:
                LaunchApplication();
                return;
        }
    }
}

void NewSettingFile()
{
    Settings.ExeFile = "C:\\3DXChat\\3DXChat.exe";
    Settings.Path = "C:\\3DXChat";
    Settings.RegistryKey = "Software\\SexGameDevil\\3DXChat";
    Settings.LoginValue = "loginData_h2649537910";

    File.WriteAllText("settings.json", JsonSerializer.Serialize(Settings, JsonOptions));
}

void LaunchApplication()
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

void PrepareScreen()
{
    Console.Clear();
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Title = "3DXChat Login Selector";
    Console.WriteLine("      3DXChat Login Selector");
    Console.WriteLine("====================================");
}

string GetLogin(string[] LoginKeys, string currentLoginKey)
{
    PrepareScreen();
    Console.WriteLine("Select the login you want to use");
    Console.WriteLine("====================================");
    for (int i = 0; i < LoginKeys.Length && i < 10; i++)
    {
        if (LoginKeys[i] == currentLoginKey)
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine($"{i + 1}:\t{LoginKeys[i]}");
        Console.ForegroundColor = ConsoleColor.Gray;

    }
    Console.WriteLine("====================================");
    Console.WriteLine("Enter the number of the login you want to use (q to quit / l to start without changing the login)");

    while (true)
    {
        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.Q:
                return "";

            case ConsoleKey.L:
                LaunchApplication();
                return "";

            default:
                if (int.TryParse(key.KeyChar.ToString(), out int index))
                {
                    if (index > 0 && index <= LoginKeys.Length)
                    {
                        return LoginKeys[index - 1];
                    }
                }
                break;
        }
    }
}


bool LoadData()
{
    if (File.Exists("data.dat"))
    {
        try
        {
            LoginData = JsonSerializer.Deserialize<Dictionary<string, byte[]>>(File.ReadAllText("data.dat")) ?? new();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading data.dat: {e.Message}");
            return false;
        }
    }
    if (File.Exists("settings.json"))
    {
        try
        {
            Settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json")) ?? new();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading settings.json: {e.Message}");
            return false;
        }
    }
    return true;
}

bool SaveData()
{
    try
    {
        File.WriteAllText("data.dat", JsonSerializer.Serialize(LoginData, JsonOptions));
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error saving data.dat: {e.Message}");
        return false;
    }
    return true;
}

bool StoreNewLogin(byte[] data)
{
    PrepareScreen();
    Console.WriteLine("New Login found. Do you want to store it? (y/n)");
    var key = Console.ReadKey(true);
    if (key.Key == ConsoleKey.Y)
    {

        Console.WriteLine("\n\rEnter a name for this login");
        string name = Console.ReadLine() ?? "Default";
        LoginData[name] = data;
        foundLoginKey = name;
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
