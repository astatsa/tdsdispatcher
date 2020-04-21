using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace TDSDispatcher.ViewModels.Dialogs
{
    class MessageBoxViewModel : BindableBase, IDialogAware
    {
        #region Properties
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string message;
        public string Message
        { 
            get => message;
            set => SetProperty(ref message, value);
        }

        private string detail;
        public string Detail
        {
            get => detail;
            set => SetProperty(ref detail, value, () => RaisePropertyChanged(nameof(ShowDetail)));
        }

        public bool ShowDetail
        {
            get => !String.IsNullOrWhiteSpace(Detail);
        }

        #endregion

        #region IDialogAware implementation
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if(parameters.TryGetValue("Title", out string title))
            {
                this.Title = title;
            }
            if(parameters.TryGetValue("Message", out string message))
            {
                this.Message = message;
            }
            if(parameters.TryGetValue("Detail", out string detail))
            {
                this.Detail = detail;
            }
        }
        #endregion

        #region Commands
        private ICommand buttonCommand;
        public ICommand ButtonCommand =>
            buttonCommand ?? (buttonCommand = new DelegateCommand<string>(
                x =>
                {
                    DialogResult result = null;
                    if (Enum.TryParse<ButtonResult>(x, out ButtonResult br))
                    {
                        result = new DialogResult(br);
                    }
                    RequestClose?.Invoke(result);
                }));
        #endregion
    }
}
