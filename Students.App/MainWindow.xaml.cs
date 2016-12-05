using System;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Ioc;
using Students.App.ViewModel;
using Students.Common.Services;

namespace Students.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDialogService
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            SimpleIoc.Default.Register<IDialogService>(() => this, true);
            SimpleIoc.Default.Register(() => Dispatcher.CurrentDispatcher, true);

            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        public void ShowError(string message, string title)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK,
                    MessageBoxOptions.DefaultDesktopOnly);
            }));
        }

        public void ShowQuestion(string message, string title, Action<bool> onAnswer)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                var messageBoxResult = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question,
                    MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);

                onAnswer(messageBoxResult == MessageBoxResult.Yes);
            }));
        }
    }
}