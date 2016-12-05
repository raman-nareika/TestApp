using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Students.Common;
using Students.Common.Services;

namespace Students.App.Commands
{
    public class CommandBase<TItem> : ICommandBase where TItem : class
    {
        private Uri _image;
        private string _name;
        private readonly Action<TItem> _execute;
        private readonly Func<TItem, bool> _canExecute;

        public CommandBase(string name, Action<TItem> execute, Func<TItem, bool> canExecute = null)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _name = name;
            _execute = execute;
            _canExecute = canExecute;

            CommandManager.RequerySuggested += CommandManagerOnRequerySuggested;
        }

        public Uri Image
        {
            get { return _image; }
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null || _canExecute(parameter as TItem);
            }
            catch (Exception e)
            {
                var exception = new InvalidOperationException($"Error occurred while running command '{Name}'.", e);

                var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
                dialogService.ShowError(GetAllMessages(exception), Application.GetWindowTitle(Name));
            }

            return false;
        }

        public void Execute(object parameter)
        {
            try
            {
                _execute(parameter as TItem);
            }
            catch (Exception e)
            {
                var exception = new InvalidOperationException($"Error occurred while running command '{Name}'.", e);

                var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
                dialogService.ShowError(GetAllMessages(exception), Application.GetWindowTitle(Name));
            }
        }

        public event EventHandler CanExecuteChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CommandManagerOnRequerySuggested(object sender, EventArgs eventArgs)
        {
            OnCanExecuteChanged();
        }

        private string GetAllMessages(Exception exception)
        {
            var allMessages = new StringBuilder();

            while (exception != null)
            {
                if (allMessages.Length > 0)
                    allMessages.Append(" ");

                allMessages.Append(exception.Message);

                var aggregateException = exception as AggregateException;
                if (aggregateException != null)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                        allMessages.AppendFormat(" {0}", GetAllMessages(innerException));
                }

                exception = exception.InnerException;
            }

            return allMessages.ToString();
        }
    }
}
