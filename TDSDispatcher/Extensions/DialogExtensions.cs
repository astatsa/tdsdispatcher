using Prism.Services.Dialogs;
using System;

namespace TDSDispatcher.Extensions
{
    static class DialogExtensions
    {
        public static void ShowMessageBox(this IDialogService dialogService, string title, string message,
            ButtonResult[] buttons = null, string detail = null, Action<IDialogResult> callback = null)
        {
            dialogService.ShowDialog("MessageBox", new DialogParameters
            {
                { "Title", title },
                { "Message", message },
                { "Detail", detail },
                { "Buttons", buttons }
            }, callback);
        }
    }
}
