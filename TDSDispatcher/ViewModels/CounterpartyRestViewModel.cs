using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TDSDispatcher.Extensions;
using TDSDispatcher.Services;

namespace TDSDispatcher.ViewModels
{
    class CounterpartyRestViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService dialogService;
        private readonly ITdsApiService apiService;
        #region Commands
        public ICommand SaveCloseCommand => new DelegateCommand(
            () =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            });
        #endregion

        #region Properties
        /*private ICollection<CounterpartyRest> myVar;

        public ICollection<CounterpartyRest> MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }*/

        #endregion

        #region IDialogAware implementation
        public string Title => "Остатки поставщиков";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if(parameters == null || !parameters.TryGetValue("CounterpartyId", out int counterpartyId))
            {
                dialogService.ShowMessageBox("Ошибка", "Не указан идентификатор элемента справочника!", new ButtonResult[] { ButtonResult.OK });
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            }
        }
        #endregion

        public CounterpartyRestViewModel(IDialogService dialogService, ITdsApiService apiService)
        {
            this.dialogService = dialogService;
            this.apiService = apiService;
        }
    }
}
