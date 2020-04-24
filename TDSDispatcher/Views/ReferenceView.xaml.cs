﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TDSDTO;

namespace TDSDispatcher.Views
{
    /// <summary>
    /// Interaction logic for ReferenceView.xaml
    /// </summary>
    public partial class ReferenceView : UserControl
    {
        public ReferenceView(Func<object> dataContextResolver)
        {
            InitializeComponent();

            DataContext = dataContextResolver();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var pd = e.PropertyDescriptor as PropertyDescriptor;
            var attr = pd.Attributes[typeof(DisplayFormatAttribute)] as DisplayFormatAttribute;
            if (!String.IsNullOrEmpty(attr?.DisplayName))
            {
                e.Column.Header = attr.DisplayName;
                e.Column.MinWidth = attr.MinWidth;

                if (e.Column is DataGridTextColumn col)
                {
                    if (!string.IsNullOrWhiteSpace(attr.Format))
                    {
                        col.Binding.StringFormat = attr.Format;
                    }
                    col.CellStyle = new Style();
                    col.CellStyle.Setters.Add(new Setter(TextBox.TextAlignmentProperty,
                        (TextAlignment)attr.HorizontalAlignment));
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void DataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            if (grid.Columns.Count > 1)
            {
                //grid.Columns[^1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}
