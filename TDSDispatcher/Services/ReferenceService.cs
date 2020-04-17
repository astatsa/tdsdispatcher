using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TDSDispatcher.Views;
using Unity;

namespace TDSDispatcher.Services
{
    class ReferenceService : ISelectable
    {
        private readonly IUnityContainer container;
        public ReferenceService(IUnityContainer container)
        {
            this.container = container;
        }

        public object Select(string refName, object selectedItem = null, Window owner = null)
        {
            container.Resolve<ElementView>().Select(refName, owner);
            return null;
        }
    }
}
