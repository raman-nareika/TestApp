using AutoMapper;
using Students.App.Helpers.Interfaces;
using Students.App.ViewModel;
using Students.Data.Data;
using Students.Data.Services.Interfaces;

namespace Students.App.Helpers.Implementation
{
    public class SettingsHelper : ISettingsHelper
    {
        private readonly ISettingsService _settingsService;

        public SettingsHelper(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public Settings Load()
        {
            var settings = _settingsService.Open();

            return settings;
        }

        public void Save(SettingsViewModel settings)
        {
            var sett = Mapper.Map<Settings>(settings);

            _settingsService.Save(sett);
        }
    }
}
