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

        public bool? Navigate(string refName, bool isEdit, Window owner = null)
        {
            this.ContentControl.Content = container.Resolve<object>(refName);
            //regionManager.RequestNavigate(ViewRegions.ElementWindowContent, refName, new NavigationParameters($"IsEdit={isEdit}"));
            this.Owner = owner;
            return this.ShowDialog();
        }

        public object Select(string refName, Window owner)
        {
            var view = container.Resolve<object>($"{refName}List") as FrameworkElement;
            if(view != null && view.DataContext != null && view.DataContext is INavigationAware navigation)
            {
                var nc = new NavigationContext(container.Resolve<IRegionNavigationService>(), new Uri(refName, UriKind.Relative));
                nc.Parameters.Add("RefName", refName);
                nc.Parameters.Add("SelectionMode", true);
                navigation.OnNavigatedTo(nc);
            }
            this.ContentControl.Content = view;
            this.Owner = owner;
            this.ShowDialog();
            return null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //regionManager.Regions.Remove(ViewRegions.ElementWindowContent);
        }
    }
}
