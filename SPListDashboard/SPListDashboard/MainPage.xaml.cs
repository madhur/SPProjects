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
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Windows.Data;
using System.Diagnostics;
using System.Collections.ObjectModel;


namespace SPListDashboard
{
    public partial class MainPage : UserControl
    {
       // ISPData myDataSource = new SPClientData();
        private RowIndexConverter _rowIndexConverter = new RowIndexConverter();
        Web site;
        List spList;
        Dictionary<string, string> internalNames = new Dictionary<string, string>();
        ListItemCollection listItems;
        PagedCollectionView data;

        public MainPage()
        {
            InitializeComponent();
            BindDropDowns();
            ListComboBox.SelectionChanged += new SelectionChangedEventHandler(ListComboBox_SelectionChanged);
        }



        void ListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string listName = ListComboBox.SelectedValue as string;
            BindColumns(listName);
        }



        

        private void BindDropDowns()
        {
            BindLists();
        }

       

        /// <summary>
        /// Copies the XML contents of the textbox into the DataGrid
        /// </summary>
        //private void XmlToGrid(string xmlData)
        //{
        //    // clear the grid
        //    MainGrid.ItemsSource = null;
        //    MainGrid.Columns.Clear();

        //    // grab the xml into a XDocument
        //    XDocument xmlDoc = XDocument.Parse(xmlData);

        //    // find the columns
        //    List<string> columnNames = xmlDoc.Descendants("column")
        //                                     .Attributes("name")
        //                                     .Select(a => a.Value)
        //                                     .ToList();

        //    // add them to the grid
        //    foreach (string columnName in columnNames)
        //    {
        //        MainGrid.Columns.Add(CreateColumn(columnName));
        //    }

        //    SortableCollectionView data = new SortableCollectionView();

        //    // add the rows
        //    var rows = xmlDoc.Descendants("row");
        //    foreach (var row in rows)
        //    {
        //        Row rowData = new Row();
        //        int index = 0;
        //        var cells = row.Descendants("cell");
        //        foreach (var cell in cells)
        //        {
        //            rowData[columnNames[index]] = cell.Value;
        //            index++;
        //        }
        //        data.Add(rowData);
        //    }

        //    MainGrid.ItemsSource = data;
        //}


        //private DataGridColumn CreateColumn(string property)
        //{
        //    return new DataGridTextColumn()
        //    {
        //        CanUserSort = true,
        //        Header = property,
        //        SortMemberPath = property,
        //        IsReadOnly = true,
        //        Binding = new Binding("Data")
        //        {
        //            Converter = _rowIndexConverter as IValueConverter,
        //            ConverterParameter = property                    
        //        }
        //    };
        //}

        private void BindLists()
        {
            ClientContext clientContext = Util.GetContext();
            Site siteCollection = clientContext.Site;
            site = clientContext.Web;
            clientContext.Load(site, s=>s.Lists);
            clientContext.ExecuteQueryAsync(webSucceededCallback, webFailedCallback);
        }

        void webFailedCallback(object sender, ClientRequestFailedEventArgs args)
        {
            Debug.WriteLine(args.Message);
        }

        void webSucceededCallback(object sender, ClientRequestSucceededEventArgs args)
        {
            List<string> spLists = new List<string>();

            foreach (List spList in site.Lists)
            {
                spLists.Add(spList.Title);
            }



            Dispatcher.BeginInvoke(
            delegate
            {
                ListComboBox.ItemsSource = spLists;
            }
            );


        }

        private void BindColumns(string listName)
        {
            
            //ClientContext clientContext = new ClientContext("http://sp.madhurmoss.com");
            ////Site siteCollection = clientContext.Site;
            //Web site = clientContext.Web;
            
            //spList = site.Lists.GetByTitle(listName);
            //clientContext.Load(spList.Fields);
            //clientContext.ExecuteQueryAsync(ListSucceededCallback, webFailedCallback);

            FirstGroupComboBox.ItemsSource = MainGrid.Columns;

           
        }

        //void ListSucceededCallback(object sender, ClientRequestSucceededEventArgs args)
        //{
        //    List<string> spFields = new List<string>();

        //    foreach (Field spField in spList.Fields)
        //    {
        //        spFields.Add(spField.Title);
        //    }



        //    Dispatcher.BeginInvoke(
        //    delegate
        //    {
        //        FirstGroupComboBox.ItemsSource = spFields;
        //        SecondGroupComboBox.ItemsSource = spFields;
        //    }
        //    );


        //}

        private void BindData()
        {
            string listName = ListComboBox.SelectedValue as string;

            ClientContext clientContext = Util.GetContext();
            Web web = clientContext.Web;
            ListCollection spLists = web.Lists;
            spList = spLists.GetByTitle(listName);
            clientContext.Load(spList);
            clientContext.Load(spList.Fields);

            //foreach (DataGridTextColumn dgc in MainGrid.Columns)
            //{
            //    string colName = dgc.Header as string;
            //    Field fieldName = spList.Fields.GetByInternalNameOrTitle(colName);
            //    clientContext.Load(fieldName, f => f.Title, f => f.InternalName);
            //}

         //   
            listItems = spList.GetItems(CamlQuery.CreateAllItemsQuery());
            clientContext.Load(listItems);
            //clientContext.Load(listItems, items => items.IncludeWithDefaultProperties(item => item.DisplayName));
            clientContext.ExecuteQueryAsync(GridSucceededCallback, webFailedCallback);
           
        }

        

        private void GridSucceededCallback(object sender, ClientRequestSucceededEventArgs args)
        {
            List<string> colList = new List<string>();

            ObservableCollection<Row> rowsData = new ObservableCollection<Row>();
            //SortableCollectionView data = new SortableCollectionView();

         //   List<ListItem> lim = listItems.ToList();

            foreach (Field field in spList.Fields)
            {
                try
                {
                    internalNames.Add(field.Title, field.InternalName);
                }
                catch (ArgumentException)
                {
                }
            }
            
            foreach (ListItem listItem in listItems)
            {
                Row rowData = new Row();

                foreach (DataGridTextColumn textColumn in MainGrid.Columns)
                {
                    try
                    {
                        string colName=textColumn.Header as string;
                        string internalName = internalNames[colName];
                        rowData[colName] = listItem[internalName];
                    }
                    catch (PropertyOrFieldNotInitializedException ex)
                    {
                        Debug.WriteLine("test");
                    }
                }
                rowsData.Add(rowData);
            }

            data = new PagedCollectionView(rowsData);


          


            if (data.Count > 0)
            {

               
                Dispatcher.BeginInvoke(
                delegate
                {
                    


                    MainGrid.ItemsSource = data;
                    
                }
                );
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            BindData();

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //string listName = ListComboBox.SelectedValue as string;
            //SPListColumns listColumns = new SPListColumns(listName,MainGrid);
            //listColumns.Closed += new EventHandler(listColumns_Closed);
            //listColumns.Show();

            data.GroupDescriptions.Clear();
        }

        void listColumns_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void FirstGroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string firstGrouping = FirstGroupComboBox.SelectedValue as string;
            firstGrouping = firstGrouping.Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(firstGrouping))
            {
                data.GroupDescriptions.Add(new PropertyGroupDescription(firstGrouping));
            }

            data.Refresh();
            MainGrid.ItemsSource = data;
            
        }

        private void ListComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            BindData();
        }
    }
}
