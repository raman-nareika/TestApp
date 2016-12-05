using AutoMapper;
using Students.App.ViewModel;
using Students.Data.Data;

namespace Students.App.Profiles
{
    public class SettingsProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SettingsViewModel, Settings>();
            CreateMap<Settings, SettingsViewModel>();
        }
    }
}
