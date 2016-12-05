using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Students.App.Collection;
using Students.App.Commands;
using Students.App.Dialogs.Interfaces;
using Students.App.Helpers.Interfaces;
using Students.App.Model;
using Students.App.Properties;
using Students.Common;
using Students.Common.Data;
using Students.Common.Services;

namespace Students.App.ViewModel
{
    public class StudentListViewModel : ViewModelBase
    {
        private readonly IStudentHelper _studentHelper;
        private bool _needToLoadItems;
        [NotNull]private ObservableCollectionWithItemNotify<StudentViewModel> _students;
        [CanBeNull]private StudentViewModel _selectedStudent;

        [NotNull]public CommandBase<StudentListViewModel> ShowAddModalCommand { get; set; }
        [NotNull]public CommandBase<StudentListViewModel> DeleteManyCommand { get; set; }


        public StudentListViewModel()
        {
            _needToLoadItems = true;

            _studentHelper = ServiceLocator.Current.GetInstance<IStudentHelper>();

            SetupCommands();
            LoadStudents();

            Messenger.Default.Register<NotificationMessage>(this, NotificationMessages.UpdateStudentList, HandleUpdateStudentList);
        }

        [CanBeNull]
        public StudentViewModel SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;

                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        public List<StudentViewModel> SelectedStudents => StudentList.Where(x => x.IsSelected).ToList();

        [NotNull]
        public ObservableCollectionWithItemNotify<StudentViewModel> StudentList
        {
            get
            {
                if (_needToLoadItems)
                {
                    _needToLoadItems = false;

                    Task.Factory.StartNew(() =>
                    {
                        _students = new ObservableCollectionWithItemNotify<StudentViewModel>(LoadStudents());
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Input,
                            new Action(() => OnPropertyChanged(nameof(StudentList))));
                    });
                }

                return _students;
            }
        }

        [NotNull]
        private IEnumerable<StudentViewModel> LoadStudents()
        {
            try
            {
                return _studentHelper.LoadStudents();
            }
            catch (Exception e)
            {
                ServiceLocator.Current.GetInstance<IDialogService>().ShowError(
                    $"Error while loading list of students: {e.Message}", Application.ProductName);
            }

            return new List<StudentViewModel>();
        }

        private void Reload()
        {
            try
            {
                _needToLoadItems = true;
                OnPropertyChanged(nameof(StudentList));
            }
            catch (Exception e)
            {
                _needToLoadItems = false;

                ServiceLocator.Current.GetInstance<IDialogService>().ShowError(
                    $"Error while reloading list of students: {e.Message}", Application.ProductName);
            }
        }

        private void SetupCommands()
        {
            DeleteManyCommand = new CommandBase<StudentListViewModel>("Delete", ExecuteDeleteMany, x => SelectedStudent != null && SelectedStudents.Any());
            ShowAddModalCommand = new CommandBase<StudentListViewModel>("Add Student", ExecuteShowAddModalCommand);
        }

        private void ExecuteDeleteMany(StudentListViewModel studentListViewModel)
        {
            ServiceLocator.Current.GetInstance<IDialogService>()
                .ShowQuestion("Are you sure?", $"Delete {SelectedStudents.Count} Students",
                    yes =>
                    {
                        if (yes)
                        {
                            foreach (var selectedStudent in SelectedStudents)
                            {
                                _studentHelper.DeleteStudent(selectedStudent);
                            }
                            
                            Messenger.Default.Send<NotificationMessage>(null, NotificationMessages.UpdateStudentList);
                        }
                    });
        }

        private void ExecuteShowAddModalCommand(StudentListViewModel obj)
        {
            var dialog = ServiceLocator.Current.GetInstance<IStudentDialog>();

            dialog.BindViewModel(new StudentViewModel());
            dialog.ShowDialog();
        }

        private void HandleUpdateStudentList(NotificationMessage message)
        {
            Reload();
        }
    }
}