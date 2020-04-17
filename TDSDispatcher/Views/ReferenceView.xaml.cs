using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TDSDispatcher.Helpers;
using Unity;

namespace TDSDispatcher.Views
{
    /// <summary>
    /// Interaction logic for ReferenceView.xaml
    /// </summary>
    public partial class ReferenceView : UserControl
    {
        public ReferenceView()
        {
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var pd = e.PropertyDescriptor as PropertyDescriptor;
            var attr = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
            if(!String.IsNullOrEmpty(attr?.DisplayName))
            {
                e.Column.Header = attr.DisplayName;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
