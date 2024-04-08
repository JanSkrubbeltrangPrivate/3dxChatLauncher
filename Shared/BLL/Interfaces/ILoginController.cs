using BLL.Models;

namespace BLL.Interfaces
{
    public interface ILoginController
    {
        Dictionary<string, byte[]> LoginData { get; set; }
        // Declare the methods and properties here
        bool LoadLoginData();
        bool SaveLoginData();
        bool FindLogin(out string foundLoginKey);
        bool GetLoginData(Settings settings, out byte[]? Current);
        void SetLoginData(Settings settings, byte[] data);
    }
}