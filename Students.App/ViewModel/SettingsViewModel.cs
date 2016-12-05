using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;
using Students.App.Commands;
using Students.App.Dialogs.Interfaces;
using Students.App.Helpers.Interfaces;
using Students.App.Model;
using Students.Common.Data;

namespace Students.App.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsHelper _settingsHelper;
        private string _sourceFilePath;

        public ICommandBase SaveSettingsCommand { get; set; }
        public ICommandBase OpenFileDialogCommand { get; set; }

        public SettingsViewModel()
        {
            _settingsHelper = ServiceLocator.Current.GetInstance<ISettingsHelper>();

            SaveSettingsCommand = new CommandBase<SettingsViewModel>("Save", ExecuteSaveSettingsCommand);
            OpenFileDialogCommand = new CommandBase<SettingsViewModel>("Open", ExecuteOpenFileDialog);

            InitSettings();
        }

        public string SourceFilePath
        {
            get { return _sourceFilePath; }
            set
            {
                if (!string.IsNullOrEmpty(_sourceFilePath) && _sourceFilePath.Equals(value))
                    return;

                _sourceFilePath = value;
                OnPropertyChanged(nameof(SourceFilePath));
            }
        }

        private void ExecuteOpenFileDialog(SettingsViewModel settingsViewModel)
        {
            var openFileDialog = new OpenFileDialog {Filter = "XML Files (*.xml)|*.xml"};

            if (openFileDialog.ShowDialog() == true)
            {
                var path = openFileDialog.FileName;

                SourceFilePath = path;
            }
        }

        private void ExecuteSaveSettingsCommand(SettingsViewModel settingsViewModel)
        {
            var dialog = ServiceLocator.Current.GetInstance<ISettingsDialog>();
            
            _settingsHelper.Save(this);
            Messenger.Default.Send<NotificationMessage>(null, NotificationMessages.UpdateStudentList);
            dialog.Close();
        }

        private void InitSettings()
        {
            var settings = _settingsHelper.Load();

            SourceFilePath = settings?.SourceFilePath;
        }
    }
}