using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
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

        public bool ShowDetail => !String.IsNullOrWhiteSpace(Detail);

        private ICollection<object> buttons;
        public ICollection<object> Buttons
        {
            get => buttons;
            set => SetProperty(ref buttons, value);
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
            if (parameters.TryGetValue("Title", out string title))
            {
                this.Title = title;
            }
            if (parameters.TryGetValue("Message", out string message))
            {
                this.Message = message;
            }
            if (parameters.TryGetValue("Detail", out string detail))
            {
                this.Detail = detail;
            }
            if (parameters.TryGetValue("Buttons", out ButtonResult[] buttons) && buttons != null)
            {
                bool? isDefaultCancel = buttons.Length == 1 ? (bool?)true : null;
                Buttons = buttons
                    .Select(x => (object)new 
                    { 
                        buttonsProperties[x].Text, 
                        IsDefault =  isDefaultCancel ?? buttonsProperties[x].IsDefault, 
                        IsCancel = isDefaultCancel ?? buttonsProperties[x].IsCancel, 
                        buttonsProperties[x].ButtonResult
                    })
                    .ToList();
            }

            if (Buttons == null)
            {
                Buttons = new List<object>
                {
                    new
                    {
                        buttonsProperties[ButtonResult.OK].Text,
                        IsDefault = true,
                        IsCancel = true,
                        buttonsProperties[ButtonResult.OK].ButtonResult
                    }
                };
            }
        }
        #endregion

        #region Commands
        private ICommand buttonCommand;
        public ICommand ButtonCommand =>
            buttonCommand ?? (buttonCommand = new DelegateCommand<ButtonResult?>(
                x => RequestClose?.Invoke(new DialogResult(x.Value))));
        #endregion

        private static readonly Dictionary<ButtonResult, (string Text, bool IsDefault, bool IsCancel, ButtonResult ButtonResult)> buttonsProperties = new Dictionary<ButtonResult, (string, bool, bool, ButtonResult)>
        {
            { ButtonResult.OK, ("OK", true, false, ButtonResult.OK) },
            { ButtonResult.Cancel, ("Отмена", false, true, ButtonResult.Cancel) },
            { ButtonResult.Yes, ("Да", true, false, ButtonResult.Yes) },
            { ButtonResult.No, ("Нет", false, true, ButtonResult.No) }
        };
    }
}
