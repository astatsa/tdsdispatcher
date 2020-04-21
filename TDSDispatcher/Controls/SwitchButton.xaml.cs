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

namespace TDSDispatcher.Controls
{
    /// <summary>
    /// Interaction logic for SwitchButton.xaml
    /// </summary>
    public partial class SwitchButton : CheckBox
    {
        public static readonly DependencyProperty TrueTextProperty = DependencyProperty.Register(nameof(TrueText), typeof(string), typeof(SwitchButton));
        public static readonly DependencyProperty FalseTextProperty = DependencyProperty.Register(nameof(FalseText), typeof(string), typeof(SwitchButton));

        public string TrueText 
        { 
            get => (string)GetValue(TrueTextProperty);
            set => SetValue(TrueTextProperty, value); 
        }

        public string FalseText
        {
            get => (string)GetValue(FalseTextProperty);
            set => SetValue(FalseTextProperty, value);
        }
        public SwitchButton()
        {
            InitializeComponent();
        }
    }
}
