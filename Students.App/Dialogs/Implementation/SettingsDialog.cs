using System;
using Students.App.Dialogs.Interfaces;
using Students.App.View;
using Students.App.ViewModel;

namespace Students.App.Dialogs.Implementation
{
    public class SettingsDialog : ISettingsDialog
    {
        private SettingView _view;

        public void BindViewModel(SettingsViewModel viewModel)
        {
            GetDialog().DataContext = viewModel;
        }

        public void ShowDialog()
        {
            GetDialog().ShowDialog();
        }

        public void Close()
        {
            GetDialog().Close();
        }

        private SettingView GetDialog()
        {
            if (_view == null)
            {
                _view = new SettingView();
                _view.Closed += view_Closed;
            }
            return _view;
        }

        void view_Closed(object sender, EventArgs e)
        {
            _view = null;
        }
    }
}
