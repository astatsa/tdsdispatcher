using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Services;
using TDSDTO.Documents;

namespace TDSDispatcher.ViewModels
{
    class OrderViewModel : BaseEntityViewModel<Order>
    {
        public OrderViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService) 
            : base(referenceService, apiService, dialogService)
        {
        }
    }
}
