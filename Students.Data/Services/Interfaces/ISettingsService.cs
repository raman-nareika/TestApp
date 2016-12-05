using Students.Data.Data;

namespace Students.Data.Services.Interfaces
{
    public interface ISettingsService
    {
        void Save(Settings settings);
        Settings Open();
    }
}
