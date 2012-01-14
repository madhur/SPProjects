using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.SharePoint.Client;
using System.Diagnostics;
using System.Collections;
using System.Windows.Data;

namespace SPListDashboard
{
    public partial class SPListColumns : ChildWindow
    {
        Web site;
        List spList;
        ListItemCollection listItems;
        string listName;
        public DataGrid MainGrid;

        public SPListColumns(string listName, DataGrid MainGrid)
        {
            
            InitializeComponent();
            if (!string.IsNullOrEmpty(listName))
            {
                this.listName = listName;
                
            }

            this.MainGrid = MainGrid;
        }

        void ColumnsListBox_Loaded(object sender, RoutedEventArgs e)
        {

            foreach (DataGridTextColumn columnValue in ColumnsListBox.Items)
            {

                ListBoxItem li = ColumnsListBox.ItemContainerGenerator.ContainerFromItem(columnValue) as ListBoxItem;
                if (li != null)
                {

                    if (MainGrid.Columns.First(s => s.Header == columnValue.Header).Visibility == System.Windows.Visibility.Collapsed)
                    {
                        li.IsSelected = false;
                    }
                    else
                    {
                        li.IsSelected = true;
                    }
                    
                }

            }
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            ColumnsListBox.Loaded += new RoutedEventHandler(ColumnsListBox_Loaded);
            BindColumns(listName);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            
            
            
            foreach (DataGridTextColumn columnValue in ColumnsListBox.Items)
            {

                ListBoxItem li = ColumnsListBox.ItemContainerGenerator.ContainerFromItem(columnValue) as ListBoxItem;
                if (li != null)
                {
                    if (!li.IsSelected)
                    {
                        MainGrid.Columns.First(s => s.Header == columnValue.Header).Visibility = System.Windows.Visibility.Collapsed;
                    }
                }

            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BindColumns(string listName)
        {

               ColumnsListBox.Items.Clear();
               ColumnsListBox.ItemsSource = MainGrid.Columns;
        }
    }


    public class ColumnConverter : IValueConverter
    {
        // This converts the DateTime object to the string to display.
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
           
            //MainGrid.Columns.First(s => s.Header == columnValue.Header).Visibility = System.Windows.Visibility.Collapsed;
            return value.ToString();
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

