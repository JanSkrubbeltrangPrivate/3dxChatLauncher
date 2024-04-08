using _3dxChatLauncher.Views;

if (!OperatingSystem.IsWindows())
{
    Console.WriteLine("This program only works on Windows");
    return;
}

MainView MainView = new();
MainView.MainMenu();