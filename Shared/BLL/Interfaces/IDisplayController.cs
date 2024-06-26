using BLL.Enums;

namespace BLL.Interfaces
{
    public interface IDisplayController
    {
        // Declare the methods and properties here
        bool LauncApplicationOnNoLoginFound();
        SelectLoginChoice SelectLogin(string[] LoginKeys, string currentLoginKey, out string Login);
        bool StoreNewLogin(out string NewName);
    }
}