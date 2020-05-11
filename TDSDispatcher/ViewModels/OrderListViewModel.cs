using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Repositories;
using TDSDTO.Documents;
using Unity;

namespace TDSDispatcher.ViewModels
{
    class OrderListViewModel : ReferenceViewModel<Order>
    {
        public OrderListViewModel(IRegionManager regionManager, IUnityContainer container, ITDSRepository repository, IDialogService dialogService) 
            : base(regionManager, container, repository, dialogService)
        {
        }
    }
}
