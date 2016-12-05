using System;
using System.ComponentModel;
using System.Windows.Input;
using Students.App.Properties;

namespace Students.App.Commands
{
    public interface ICommandBase : INotifyPropertyChanged, ICommand
    {
        [CanBeNull]
        Uri Image { get; set; }

        [NotNull]
        string Name { get; set; }
    }
}
