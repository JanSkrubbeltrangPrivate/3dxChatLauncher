namespace Equinox.Chatlauncher.Interfaces
{
    public interface ILoginController
    {
        Dictionary<string, byte[]> LoginData { get; set; }
        // Declare the methods and properties here
        bool LoadLoginData();
        bool SaveLoginData();
        bool FindLogin(out string foundLoginKey);
        bool GetLoginData(Models.Settings settings, out byte[]? Current);
        void SetLoginData(Models.Settings settings, byte[] data);
    }
}