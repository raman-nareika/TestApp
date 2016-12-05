using System;

namespace Students.Common.Services
{
    public interface IDialogService
    {
        void ShowError(string message, string title);
        void ShowQuestion(string message, string title, Action<bool> onAnswer);
    }
}
