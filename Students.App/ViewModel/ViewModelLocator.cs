/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Students.App.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Students.App.Dialogs.Implementation;
using Students.App.Dialogs.Interfaces;
using Students.App.Helpers.Implementation;
using Students.App.Helpers.Interfaces;
using Students.Data.Services.Implementation;
using Students.Data.Services.Interfaces;

namespace Students.App.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            try
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                #region Dialogs

                SimpleIoc.Default.Register<ISettingsDialog, SettingsDialog>();
                SimpleIoc.Default.Register<IStudentDialog, StudentDialog>();

                #endregion

                #region Services

                SimpleIoc.Default.Register<ISettingsService, SettingsService>();
                SimpleIoc.Default.Register<IStudentFacade, StudentFacade>();
                SimpleIoc.Default.Register<IStudentService, StudentService>();

                #endregion

                #region Helpers

                SimpleIoc.Default.Register<ISettingsHelper, SettingsHelper>();
                SimpleIoc.Default.Register<IStudentHelper, StudentHelper>();

                #endregion

                #region ViewModels

                SimpleIoc.Default.Register<MainViewModel>();
                SimpleIoc.Default.Register<SettingsViewModel>();
                SimpleIoc.Default.Register<StudentListViewModel>();
                SimpleIoc.Default.Register<StudentViewModel>();

                #endregion
            }
            catch(Exception e) { }
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public StudentListViewModel StudentListViewModel => ServiceLocator.Current.GetInstance<StudentListViewModel>();
        public StudentViewModel StudentViewModel => ServiceLocator.Current.GetInstance<StudentViewModel >();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}