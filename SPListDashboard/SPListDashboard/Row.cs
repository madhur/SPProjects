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
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.SharePoint.Client;

namespace SPListDashboard
{
    /// <summary>
    /// Represents a dynamic row of data
    /// </summary>
    public class Row : INotifyPropertyChanged
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        /// <summary>
        /// Gets the column names
        /// </summary>
        public ICollection<string> ColumnNames
        {
            get
            {
                return _data.Keys;
            }
        }
        /// <summary>
        /// a string indexer for accessing the rows proeprties
        /// </summary>
        public object this[string index]
        {
            get
            {
                return _data[index];
            }
            set
            {
                _data[index] = value;

                // any property changes need to be signalled to UI elements bound to the Data property
                OnPropertyChanged("Data");
            }
        }

        public string Requestor
        {
            get
            {
                FieldUserValue fuv = _data["Requestor"] as FieldUserValue;
                if (fuv != null)
                {
                    return fuv.LookupValue;
                }
                else
                {
                    return string.Empty;
                }
            }

        }

        public string BusinessUnit
        {
            get
            {
                return _data["Business Unit"] as string;
            }

        }

        public string FundingSource
        {
            get
            {
                return _data["Funding Source"] as string;
            }

        }

        public bool FundingAvailable
        {
            get
            {
                return bool.Parse(_data["Funding Available"].ToString());
            }

        }

        public string ProjectName
        {
            get
            {
                return _data["Project Name"] as string;
            }

        }

        public string IntakeStatus
        {
            get
            {
                return _data["Intake Status"] as string;
            }

        }

        public string TypeofRequest
        {
            get
            {
                return _data["Type of Request"] as string;
            }

        }

        public string DateRecieved
        {
            get
            {
                return _data["Date Recieved"] as string;
            }

        }

        public string SizingDueBy
        {
            get
            {
                return _data["Sizing Due By"] as string;
            }

        }

        public string Sizingdetails
        {
            get
            {
                return _data["Sizing details"] as string;
            }

        }

        public string ApplicationsImpacted
        {
            get
            {
                return _data["Applications Impacted"] as string;
            }

        }

        public string MarketRegionsImpacted
        {
            get
            {
                return _data["Market/Regions Impacted"] as string;
            }

        }

        public string RequestStatus
        {
            get
            {
                return _data["Request Status"] as string;
            }

        }

        public string Week
        {
            get
            {
                return _data["Week"] as string;
            }

        }

        public string SizingContact
        {
            get
            {
                FieldUserValue fuv = _data["Sizing Contact"] as FieldUserValue;
                if (fuv != null)
                {
                    return fuv.LookupValue;
                }
                else
                {
                    return string.Empty;
                }
            }

        }


        /// <summary>
        /// A property which is used for integrating with the binding framework.
        /// </summary>
        public object Data
        {
            get
            {
                // when the binding framework reads this property, simple return the Row instance. The
                // RowIndexConverter takes care of extracting the correct property value
                return this;
            }
            set
            {
                // the RowIndexConverter will signal property changes by providing an instance of PropertyValueChange.
                PropertyValueChange setter = value as PropertyValueChange;
                _data[setter.PropertyName] = setter.Value;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}
