using Students.App.ViewModel;
using Students.Data.Data;

namespace Students.App.Helpers.Interfaces
{
    public interface ISettingsHelper
    {
        Settings Load();
        void Save(SettingsViewModel settings);
    }
}
