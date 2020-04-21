using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDispatcher.Extensions
{
    static class DialogExtensions
    {
        public static void ShowMessageBox(this IDialogService dialogService, string title, string message, string detail, Action<IDialogResult> callback)
        {
            dialogService.ShowDialog("MessageBox", new DialogParameters
            {
                { "Title", title },
                { "Message", message },
                { "Detail", detail }
            }, callback);
        }
    }
}
