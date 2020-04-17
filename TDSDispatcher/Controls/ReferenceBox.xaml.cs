using Prism.Commands;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TDSDispatcher.Services;
using TDSDispatcher.Views;

namespace TDSDispatcher.Controls
{
    /// <summary>
    /// Interaction logic for ReferenceBox.xaml
    /// </summary>
    public partial class ReferenceBox : TextBox
    {
        public static readonly DependencyProperty RefNameProperty = DependencyProperty.Register(nameof(RefName), typeof(string), typeof(ReferenceBox));
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof(SelectedValue), typeof(object), typeof(ReferenceBox));
        public static readonly DependencyProperty SelectServiceProperty = DependencyProperty.Register(nameof(SelectService), typeof(ISelectable), typeof(ReferenceBox));

        public ReferenceBox()
        {
            InitializeComponent();
        }

        public string RefName 
        {
            get => (string)GetValue(RefNameProperty);
            set => SetValue(RefNameProperty, value);
        }

        public object SelectedValue
        {
            get => GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }

        public ISelectable SelectService
        { 
            get => (ISelectable)GetValue(SelectServiceProperty);
            set => SetValue(SelectServiceProperty, value); 
        }

        public ICommand SelectCommand => new DelegateCommand<Window>(
            x =>
            {
                if(SelectService != null)
                {
                    SelectedValue = SelectService.Select(RefName, SelectedValue, x);
                }
            });
    }
}
