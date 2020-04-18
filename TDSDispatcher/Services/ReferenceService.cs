using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TDSDispatcher.Models;
using TDSDispatcher.Repositories;
using TDSDispatcher.Views;
using Unity;

namespace TDSDispatcher.Services
{
    class ReferenceService : ISelectable
    {
        private readonly IUnityContainer container;
        private readonly ITDSRepository repository;

        public ReferenceService(IUnityContainer container, ITDSRepository repository)
        {
            this.container = container;
            this.repository = repository;
        }

        public object Select(string refName, object selectedItem = null, Window owner = null)
        {
            var entityInfo = repository.GetEntityByName(refName);
            if (entityInfo != null)
            {
                return container.Resolve<ElementView>().Select(entityInfo, owner);
            }
            return null;
        }
    }
}
