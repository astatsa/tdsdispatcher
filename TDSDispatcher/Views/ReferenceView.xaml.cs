﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            var dg = sender as DataGrid;
            if (!String.IsNullOrEmpty(attr?.DisplayName))
            {
                e.Column.Header = attr.DisplayName;
                e.Column.MinWidth = attr.MinWidth;
                e.Column.CellStyle = new Style(typeof(DataGridCell), dg.CellStyle);
                e.Column.CellStyle.Setters.Add(new Setter(TextBox.TextAlignmentProperty,
                    (TextAlignment)attr.HorizontalAlignment));

                if (e.Column is DataGridBoundColumn col)
                {
                    if (!String.IsNullOrWhiteSpace(attr.MemberName))
                    {
                        col.Binding = new Binding($"{e.PropertyName}.{attr.MemberName}");
                    }
                    if (!string.IsNullOrWhiteSpace(attr.Format))
                    {
                        col.Binding.StringFormat = attr.Format;
                    }                    
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void DataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            //DataGrid grid = (DataGrid)sender;
            //if (grid.Columns.Count > 1)
            //{
            //    grid.Columns[^1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            //}
        }
    }
}
