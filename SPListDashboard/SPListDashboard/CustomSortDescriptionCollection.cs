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
using System.ComponentModel;
using System.Collections.Specialized;

namespace SPListDashboard
{
    public class CustomSortDescriptionCollection : SortDescriptionCollection
    {

        public event NotifyCollectionChangedEventHandler MyCollectionChanged
        {
            add
            {
                this.CollectionChanged += value;
            }
            remove
            {
                this.CollectionChanged -= value;
            }
        }
    }
}
