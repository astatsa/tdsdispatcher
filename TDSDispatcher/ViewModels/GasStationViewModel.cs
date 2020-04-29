using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class GasStationViewModel : BaseEntityViewModel<GasStation>
    {
        public GasStationViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService, ITDSRepository repository) 
            : base(referenceService, apiService, dialogService, repository)
        {
        }
    }
}
