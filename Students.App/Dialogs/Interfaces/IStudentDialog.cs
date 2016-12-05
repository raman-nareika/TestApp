using Students.App.ViewModel;

namespace Students.App.Dialogs.Interfaces
{
    public interface IStudentDialog
    {
        void BindViewModel(StudentViewModel viewModel);
        void Close();
        void ShowDialog();
    }
}
