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
        //TODO: Вью-модель списка заявок. Должна быть реализована возможность быстрого фильтра по водителю, просмотру остатков и т.п
        public OrderListViewModel(IRegionManager regionManager, IUnityContainer container, ITDSRepository repository, IDialogService dialogService, Settings settings) 
            : base(regionManager, container, repository, dialogService, settings)
        {
        }
    }
}
