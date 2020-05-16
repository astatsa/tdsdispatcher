using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDispatcher.ViewModels.Dialogs
{
    class QuickSearchViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware
        public string Title => "Поиск";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
        #endregion
    }
}
