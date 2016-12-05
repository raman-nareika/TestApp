using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Students.App.Commands;
using Students.App.Dialogs.Interfaces;
using Students.App.Helpers.Interfaces;
using Students.App.Model;
using Students.Common.Data;
using Students.Common.Services;
using Students.Data.Data;

namespace Students.App.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class StudentViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IStudentHelper _studentHelper;

        private string _firstName;
        private string _lastName;
        private byte _age;
        private Gender _gender;

        public ICommandBase EditStudentCommand { get; set; }
        public ICommandBase DeleteWindowCommand { get; set; }
        public ICommandBase SaveStudentCommand { get; set; }
        public ICommandBase CloseWindowCommand { get; set; }
        public StudentViewModel()
        {
            _studentHelper = ServiceLocator.Current.GetInstance<IStudentHelper>();

            SetupCommands();
        }

        public bool IsSelected { get; set; }
        public Mode Mode { get; set; }
        public int Id { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if(!string.IsNullOrEmpty(_firstName) && _firstName.Equals(value))
                    return;

                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (!string.IsNullOrEmpty(_lastName) && _lastName.Equals(value))
                    return;

                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public byte Age
        {
            get { return _age; }
            set
            {
                if (_age == value)
                    return;

                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public Gender Gender
        {
            get { return _gender; }
            set
            {
                if (_gender.Equals(value))
                    return;

                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string FullName => $"{FirstName} {LastName}";
        public string FullAge => $"{Age} {GetAgePrefix(Age)}";

        private void SetupCommands()
        {
            EditStudentCommand = new CommandBase<StudentViewModel>("Edit", ExecuteEditStudentCommand);
            DeleteWindowCommand = new CommandBase<StudentViewModel>("Delete", ExecuteDeleteWindowCommand);
            SaveStudentCommand = new CommandBase<StudentViewModel>("Save", ExecuteSaveStudentCommand);
            CloseWindowCommand = new CommandBase<StudentViewModel>("Close", ExecuteCloseWindowCommand);
        }

        private void ExecuteDeleteWindowCommand(StudentViewModel studentViewModel)
        {
            ServiceLocator.Current.GetInstance<IDialogService>()
                .ShowQuestion("Are you sure?", $"Delete Student {FullName}",
                    yes =>
                    {
                        if (yes)
                        {
                            _studentHelper.DeleteStudent(this);
                            Messenger.Default.Send<NotificationMessage>(null, NotificationMessages.UpdateStudentList);
                        }
                    });
        }

        private void ExecuteEditStudentCommand(StudentViewModel studentViewModel)
        {
            var dialog = ServiceLocator.Current.GetInstance<IStudentDialog>();
            Mode = Mode.Edit;
            dialog.BindViewModel(this);
            dialog.ShowDialog();
        }

        private void ExecuteCloseWindowCommand(StudentViewModel studentViewModel)
        {
            var dialog = ServiceLocator.Current.GetInstance<IStudentDialog>();

            Messenger.Default.Send<NotificationMessage>(null, NotificationMessages.UpdateStudentList);
            dialog.Close();
        }

        private void ExecuteSaveStudentCommand(StudentViewModel studentViewModel)
        {
            if (Mode.Equals(Mode.Add))
                _studentHelper.Save(this);
            else
                _studentHelper.Update(this);

            ExecuteCloseWindowCommand(this);
        }

        private string GetAgePrefix(byte age)
        {
            var mod = age%10;

            if ((age > 10 && age < 15) || (mod >= 5 && mod <= 9) || mod == 0)
                return "лет";

            if (mod >= 2 && mod <= 4)
                return "года";

            if (mod == 1)
                return "год";

            return string.Empty;

        }

        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals(nameof(FirstName)))
                {
                    if (string.IsNullOrEmpty(FirstName))
                    {
                        return "Please enter your first name";
                    }
                }
                else if (columnName.Equals(nameof(LastName)))
                {
                    if (string.IsNullOrEmpty(LastName))
                    {
                        return "Please enter your last name";
                    }
                }
                else if (columnName.Equals(nameof(Age)))
                {
                    if (Age < 16 || Age > 100)
                    {
                        return "Age should be in the range from 16 to 100";
                    }
                }

                return null;
            }
        }

        public string Error => null;
    }
}