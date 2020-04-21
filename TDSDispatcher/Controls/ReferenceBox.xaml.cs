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
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof(SelectedValue), typeof(object), typeof(ReferenceBox),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true
            });
        public static readonly DependencyProperty SelectServiceProperty = DependencyProperty.Register(nameof(SelectService), typeof(ISelectable), typeof(ReferenceBox));
        public static readonly DependencyProperty DisplayMemberProperty = DependencyProperty.Register(nameof(DisplayMember), typeof(string), typeof(ReferenceBox));
        public static readonly DependencyProperty ValueMemberProperty = DependencyProperty.Register(nameof(ValueMember), typeof(string), typeof(ReferenceBox));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(ReferenceBox),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true
            });

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
            set
            {
                SetValue(SelectedValueProperty, value);
                Text = GetPropertyValue<string>(DisplayMember, SelectedValue);
                Value = GetPropertyValue<object>(ValueMember, SelectedValue);
            }
        }

        public ISelectable SelectService
        { 
            get => (ISelectable)GetValue(SelectServiceProperty);
            set => SetValue(SelectServiceProperty, value); 
        }

        public string DisplayMember 
        { 
            get => (string)GetValue(DisplayMemberProperty);
            set
            {
                SetValue(DisplayMemberProperty, value);
                Text = GetPropertyValue<string>(DisplayMember, SelectedValue);
            }
        }

        public string ValueMember 
        { 
            get => (string)GetValue(ValueMemberProperty);
            set
            {
                SetValue(ValueMemberProperty, value);
                Value = GetPropertyValue<object>(ValueMember, SelectedValue);
            }
        }

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public ICommand SelectCommand => new DelegateCommand<Window>(
            x =>
            {
                if(SelectService != null)
                {
                    SelectedValue = SelectService.Select(RefName, SelectedValue, x);
                }
            });

        private T GetPropertyValue<T>(string propertyName, object obj)
        {
            if (!String.IsNullOrWhiteSpace(propertyName))
            {
                var value = SelectedValue;
                if (value != null)
                {
                    return (T)value.GetType().GetProperty(propertyName)?.GetValue(value);
                }
            }
            return default;
        }
    }
}
