using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Services;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class CounterpartyViewModel : BaseEntityViewModel<Counterparty>
    {
        public CounterpartyViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService) 
            : base(referenceService, apiService, dialogService)
        {

        }
    }
}
