using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using Students.App.Commands;
using Students.App.Dialogs.Interfaces;

namespace Students.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private StudentListViewModel _studentListViewModel;
        public ICommandBase ShowSettingsModal { get; set; }

        public MainViewModel()
        {
            _studentListViewModel = new StudentListViewModel();

            ShowSettingsModal = new CommandBase<MainViewModel>("Settings", ExecuteShowSettingsModal);
        }

        public StudentListViewModel StudentListViewModel => _studentListViewModel;

        private void ExecuteShowSettingsModal(MainViewModel mainViewModel)
        {
            var dialog = ServiceLocator.Current.GetInstance<ISettingsDialog>();

            dialog.BindViewModel(new SettingsViewModel());
            dialog.ShowDialog();
        }
    }
}