using Students.App.ViewModel;

namespace Students.App.Dialogs.Interfaces
{
    public interface ISettingsDialog
    {
        void BindViewModel(SettingsViewModel viewModel);
        void Close();
        void ShowDialog();
    }
}
