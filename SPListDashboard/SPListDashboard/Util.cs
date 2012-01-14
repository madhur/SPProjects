using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using Microsoft.SharePoint.Client;

namespace SPListDashboard
{
    public static class Util
    {
        private static RowIndexConverter _rowIndexConverter = new RowIndexConverter();

        public static DataGridColumn CreateColumn(string property)
        {
            return new DataGridTextColumn()
            {
                CanUserSort = true,
                Header = property,
                SortMemberPath = property,
                IsReadOnly = true,
                Binding = new Binding("Data")
                {
                    Converter = _rowIndexConverter as IValueConverter,
                    ConverterParameter = property
                }
            };
        }

        public static ClientContext GetContext()
        {
            if (ClientContext.Current != null)
                return ClientContext.Current;
            else
                return new ClientContext("http://sp.madhurmoss.com");

        }

    }
}
