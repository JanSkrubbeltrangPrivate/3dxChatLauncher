using BLL.Models;

namespace BLL.Interfaces
{
    public interface ISettingController
    {
        Settings Settings { get; set; }
        // Declare the methods and properties here
        bool LoadSettingFile();
        bool SaveSettingFile();
        void NewSettingFile();
    }
}