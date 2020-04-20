using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TDSDispatcher.Helpers;
using TDSDispatcher.Models;
using TDSDispatcher.ViewModels;
using Unity;

namespace TDSDispatcher.Views
{
    /// <summary>
    /// Interaction logic for ElementView.xaml
    /// </summary>
    public partial class ElementView : Window
    {
        private readonly IUnityContainer container;
        public ElementView(IUnityContainer container)
        {
            InitializeComponent();
            this.container = container;
        }

        public bool? AddOrEdit(EntityInfo entityInfo, bool isEdit, Window owner = null, object model = null)
        {
            var view = GetInitializedView(entityInfo.ModelName, ("EntityInfo", entityInfo), ("IsEdit", isEdit), ("Model", model));
            if (view == null)
                return null;

            this.ContentControl.Content = view;
            this.Owner = owner;
            this.DataContext = view.DataContext;

            if(view.DataContext is ICloseRequest closeRequest)
            {
                closeRequest.CloseRequest +=
                    (s, e) =>
                    {
                        this.DialogResult = e;
                        this.Close();
                    };
            }

            return this.ShowDialog();
        }

        public object Select(EntityInfo entityInfo, Window owner)
        {
            var view = GetInitializedView($"{entityInfo.ModelName}List", ("EntityInfo", entityInfo), ("SelectionMode", true));
            if (view == null)
                return null;

            this.ContentControl.Content = view;
            this.Owner = owner;
            this.DataContext = view.DataContext;
            object res = null;
            if(view.DataContext is ISelectionAware selectionAware)
            {
                selectionAware.Selected +=
                    (s, e) =>
                    {
                        res = e;
                        this.Close();
                    };
            }
            this.ShowDialog();
            return res;
        }

        private FrameworkElement GetInitializedView(string name, params (string name, object value)[] parameters)
        {
            var view = container.Resolve<object>(name) as FrameworkElement;
            if (view != null && view.DataContext != null && view.DataContext is INavigationAware navigation)
            {
                var nc = new NavigationContext(container.Resolve<IRegionNavigationService>(), new Uri(name, UriKind.Relative));
                foreach(var par in parameters)
                {
                    nc.Parameters.Add(par.name, par.value);
                }
                navigation.OnNavigatedTo(nc);
            }

            return view;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //regionManager.Regions.Remove(ViewRegions.ElementWindowContent);
        }
    }
}
