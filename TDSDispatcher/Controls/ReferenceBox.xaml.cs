using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TDSDispatcher.Services;

namespace TDSDispatcher.Controls
{
    /// <summary>
    /// Interaction logic for ReferenceBox.xaml
    /// </summary>
    public partial class ReferenceBox : TextBox, INotifyPropertyChanged
    {
        public static readonly DependencyProperty RefNameProperty = DependencyProperty.Register(nameof(RefName), typeof(string), typeof(ReferenceBox));
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof(SelectedValue), typeof(object), typeof(ReferenceBox),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                PropertyChangedCallback = (d, e) =>
                {
                    var o = d as ReferenceBox;
                    if (o != null)
                        o.SelectedValue = e.NewValue;
                }
            });
        public static readonly DependencyProperty SelectServiceProperty = DependencyProperty.Register(nameof(SelectService), typeof(ISelectable), typeof(ReferenceBox));
        public static readonly DependencyProperty SelectParameterProperty = DependencyProperty.Register(nameof(SelectParameter), typeof(object), typeof(ReferenceBox));
        public static readonly DependencyProperty SearchServiceProperty = DependencyProperty.Register(nameof(SearchService), typeof(ISearchAware), typeof(ReferenceBox));
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
            this.TextChanged += ReferenceBox_TextChanged;
        }

        private async void ReferenceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchService == null || this.Text.Length < 3)
                return;

            SearchList = await SearchService.SearchAsync(RefName, DisplayMember, this.Text);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable searchList;
        public IEnumerable SearchList
        {
            get => searchList;
            set
            {
                if(searchList != value)
                {
                    searchList = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchList)));
                }
            }
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

        public object SelectParameter 
        { 
            get => GetValue(SelectParameterProperty); 
            set => SetValue(SelectParameterProperty, value); 
        }

        public ISelectable SelectService
        {
            get => (ISelectable)GetValue(SelectServiceProperty);
            set => SetValue(SelectServiceProperty, value);
        }

        public ISearchAware SearchService
        {
            get => (ISearchAware)GetValue(SearchServiceProperty);
            set => SetValue(SearchServiceProperty, value);
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
                if (SelectService != null)
                {
                    var res = SelectService.Select(RefName, SelectedValue, x, SelectParameter);
                    if (res != null)
                        SelectedValue = res;
                }
            });

        private T GetPropertyValue<T>(string propertyName, object obj)
        {
            if (!String.IsNullOrWhiteSpace(propertyName))
            {
                if (obj != null)
                {
                    return (T)obj.GetType().GetProperty(propertyName)?.GetValue(obj);
                }
            }
            return default;
        }
    }
}
