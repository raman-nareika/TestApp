using System.Windows;
using AutoMapper;
using GalaSoft.MvvmLight.Threading;
using Students.App.Profiles;

namespace Students.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<SettingsProfile>();
                cfg.AddProfile<StudentsProfile>();
            });
        }
    }
}
