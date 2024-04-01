namespace Equinox.Chatlauncher.Interfaces
{
    public interface IDisplayController
    {
        // Declare the methods and properties here
        bool NoLogin();
        string SelectLogin(string[] LoginKeys, string currentLoginKey);
        bool StoreNewLogin(out string NewName);
    }
}