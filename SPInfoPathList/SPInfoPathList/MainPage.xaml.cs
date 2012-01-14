using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.SharePoint.Client;
using System.Windows.Data;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Reflection;

namespace SPInfoPathList
{
    public partial class MainPage : UserControl
    {

        List spList;
        ListItemCollection listItems;
        List<BPSFeedbackItem> feedbacks = new List<BPSFeedbackItem>();
        PagedCollectionView data;

        public MainPage()
        {
            InitializeComponent();
            BindData();
        }


        private void BindData()
        {
            ClientContext clientContext = Util.GetContext();
            Web web = clientContext.Web;
            ListCollection spLists = web.Lists;
            spList = spLists.GetByTitle("Business Partner Survey");
            clientContext.Load(spList);

            listItems = spList.GetItems(CamlQuery.CreateAllItemsQuery());
            clientContext.Load(listItems);
            clientContext.ExecuteQueryAsync(GridSucceededCallback, webFailedCallback);

        }

        private void GridSucceededCallback(object sender, ClientRequestSucceededEventArgs args)
        {

            foreach (var listItem in listItems)
            {
                string filePath = listItem["FileRef"].ToString();

                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += HttpsCompleted;

                wc.DownloadStringAsync(new Uri("http://sp.madhurmoss.com"+filePath));
                //wc.DownloadStringAsync(new Uri(filePath, UriKind.Relative));


            }




        }

        private void HttpsCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                XDocument xdoc = XDocument.Parse(e.Result, LoadOptions.None);

                string nameSpace = xdoc.Root.Name.NamespaceName;

                XName projectName = XName.Get("projectname", nameSpace);
                XName projectDcNumber = XName.Get("projectnumber", nameSpace);
                XName parameter = XName.Get("parameter", nameSpace);
                XName rating = XName.Get("rating", nameSpace);
                XName feedback = XName.Get("feedback", nameSpace);
                XName group2 = XName.Get("group2", nameSpace);

                string projectNamestr = xdoc.Descendants(projectName).First().Value;
                string projectDCNumber = xdoc.Descendants(projectDcNumber).First().Value;

                BPSFeedbackItem feedbackItem;
                foreach (XElement xelem in xdoc.Descendants(group2))
                {
                    feedbackItem = new BPSFeedbackItem();
                    feedbackItem.ProjectName = projectNamestr;
                    feedbackItem.ProjectDCNumber = projectDCNumber;
                    feedbackItem.Feedback = xelem.Descendants(feedback).First().Value;
                    feedbackItem.Parameter = xelem.Descendants(parameter).First().Value;
                    feedbackItem.Rating = xelem.Descendants(rating).First().Value;

                    feedbacks.Add(feedbackItem);

                }

                data = new PagedCollectionView(feedbacks);

                Dispatcher.BeginInvoke(delegate()
                {
                    MainGrid.ItemsSource = data;

                });
            }
        }



        void webFailedCallback(object sender, ClientRequestFailedEventArgs args)
        {
            Debug.WriteLine(args.Message);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ExportDataGrid(MainGrid);
        }



        private void ExportDataGrid(DataGrid dGrid)
        {

            SaveFileDialog objSFD = new SaveFileDialog()
            {
                DefaultExt = "csv",
                Filter = "CSV Files (*.csv)|*.csv|Excel XML (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 1
            };
            if (objSFD.ShowDialog() == true)
            {
                string strFormat = objSFD.SafeFileName.Substring(objSFD.SafeFileName.IndexOf('.') + 1).ToUpper();
                StringBuilder strBuilder = new StringBuilder();
                if (dGrid.ItemsSource == null) return;

                List<string> lstFields = new List<string>();

                if (dGrid.HeadersVisibility == DataGridHeadersVisibility.Column || dGrid.HeadersVisibility == DataGridHeadersVisibility.All)
                {
                    foreach (DataGridColumn dgcol in dGrid.Columns)
                        lstFields.Add(FormatField(dgcol.Header.ToString(), strFormat));
                    BuildStringOfRow(strBuilder, lstFields, strFormat);
                }

                foreach (object data in dGrid.ItemsSource)
                {
                    lstFields.Clear();
                    foreach (DataGridColumn col in dGrid.Columns)
                    {
                        string strValue = "";
                        Binding objBinding = null;
                        if (col is DataGridBoundColumn)
                            objBinding = (col as DataGridBoundColumn).Binding;
                        if (col is DataGridTemplateColumn)
                        {
                            //This is a template column...
                            //    let us see the underlying dependency object
                            DependencyObject objDO =              (col as DataGridTemplateColumn).CellTemplate.LoadContent();
                            FrameworkElement oFE = (FrameworkElement)objDO;
                            FieldInfo oFI = oFE.GetType().GetField("TextProperty");
                            if (oFI != null)
                            {
                                if (oFI.GetValue(null) != null)
                                {
                                    if (oFE.GetBindingExpression( (DependencyProperty)oFI.GetValue(null)) != null)
                                        objBinding =oFE.GetBindingExpression(
                                          (DependencyProperty)oFI.GetValue(null)).ParentBinding;
                                }
                            }
                        }

                        if (objBinding != null)
                        {
                            if (objBinding.Path.Path != "")
                            {
                                PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
                                if (pi != null) strValue = pi.GetValue(data, null).ToString();
                            }
                            if (objBinding.Converter != null)
                            {
                                if (strValue != "")
                                    strValue = objBinding.Converter.Convert(strValue,typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                                else
                                    strValue = objBinding.Converter.Convert(data,typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                            }
                        }
                        lstFields.Add(FormatField(strValue, strFormat));
                    }
                    BuildStringOfRow(strBuilder, lstFields, strFormat);
                }

                StreamWriter sw = new StreamWriter(objSFD.OpenFile());
                if (strFormat == "XML")
                {
                    //Let us write the headers for the Excel XML
                    sw.WriteLine("<?xml version=\"1.0\" " +
                                 "encoding=\"utf-8\"?>");
                    sw.WriteLine("<?mso-application progid" +
                                 "=\"Excel.Sheet\"?>");
                    sw.WriteLine("<Workbook xmlns=\"urn:" +
                                 "schemas-microsoft-com:office:spreadsheet\">");
                    sw.WriteLine("<DocumentProperties " +
                                 "xmlns=\"urn:schemas-microsoft-com:" +
                                 "office:office\">");
                    sw.WriteLine("<Author>Arasu Elango</Author>");
                    sw.WriteLine("<Created>" +
                                 DateTime.Now.ToLocalTime().ToLongDateString() +
                                 "</Created>");
                    sw.WriteLine("<LastSaved>" +
                                 DateTime.Now.ToLocalTime().ToLongDateString() +
                                 "</LastSaved>");
                    sw.WriteLine("<Company>Atom8 IT Solutions (P) " +
                                 "Ltd.,</Company>");
                    sw.WriteLine("<Version>12.00</Version>");
                    sw.WriteLine("</DocumentProperties>");
                    sw.WriteLine("<Worksheet ss:Name=\"Silverlight Export\" " +
                       "xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                    sw.WriteLine("<Table>");
                }
                sw.Write(strBuilder.ToString());
                if (strFormat == "XML")
                {
                    sw.WriteLine("</Table>");
                    sw.WriteLine("</Worksheet>");
                    sw.WriteLine("</Workbook>");
                }
                sw.Close();
            }
        }

        private static void BuildStringOfRow(StringBuilder strBuilder,List<string> lstFields, string strFormat)
        {
            switch (strFormat)
            {
                case "XML":
                    strBuilder.AppendLine("<Row>");
                    strBuilder.AppendLine(String.Join("\r\n", lstFields.ToArray()));
                    strBuilder.AppendLine("</Row>");
                    break;
                case "CSV":
                    strBuilder.AppendLine(String.Join(",", lstFields.ToArray()));
                    break;
            }
        }

        private static string FormatField(string data, string format)
        {
            switch (format)
            {
                case "XML":
                    return String.Format("<Cell><Data ss:Type=\"String" +
                       "\">{0}</Data></Cell>", data);
                case "CSV":
                    return String.Format("\"{0}\"",
                      data.Replace("\"", "\"\"\"").Replace("\n",
                      "").Replace("\r", ""));
            }
            return data;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = comboBox1.SelectedItem as ComboBoxItem;

            if (!item.Content.ToString().Equals("None"))
            {
                data.GroupDescriptions.Clear();
                data.GroupDescriptions.Add(new PropertyGroupDescription(item.Content.ToString()));

                data.Refresh();
                MainGrid.ItemsSource = data;
            }
            else
            {
                data.GroupDescriptions.Clear();
            }
        }

    }
}
