using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TDSDispatcher.ViewModels.Dialogs;

namespace TDSDispatcher.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for QuickSearchView.xaml
    /// </summary>
    public partial class QuickSearchView : UserControl
    { 
        public QuickSearchView()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                SearchTextBox.Focus();
                SearchTextBox.SelectionStart = SearchTextBox.Text.Length;
                SearchTextBox.SelectionLength = 0;
            };
        }
    }
}
