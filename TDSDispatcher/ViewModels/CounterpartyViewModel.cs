using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDispatcher.Views;
using TDSDTO.References;
using Unity;

namespace TDSDispatcher.ViewModels
{
    class CounterpartyViewModel : BaseEntityViewModel<Counterparty>
    {
        public ICommand OpenRestCommand => new DelegateCommand(
            () =>
            {
                dialogService.ShowDialog(nameof(CounterpartyRestView), new DialogParameters
                {
                    { "CounterpartyId", Model.Id }
                }, null);
            });

        public CounterpartyViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService, ITDSRepository repository)
            : base(referenceService, apiService, dialogService, repository)
        {
        }
    }
}
