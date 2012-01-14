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
using System.Collections.ObjectModel;

namespace SPListDashboard
{
    public class CustomCollectionViewGroup : INotifyPropertyChanged
    {

         // Fields
    private int _itemCount;
    private ReadOnlyObservableCollection<object> _itemsRO;
    private ObservableCollection<object> _itemsRW;
    private object _name;

    // Events
    public event PropertyChangedEventHandler PropertyChanged;

    

    // Methods
    protected void CollectionViewGroup(object name)
    {
        this._name = name;
        this._itemsRW = new ObservableCollection<object>();
        this._itemsRO = new ReadOnlyObservableCollection<object>(this._itemsRW);
    }

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, e);
        }
    }

    // Properties
    public bool IsBottomLevel { get; set; }

    public int ItemCount
    {
        get
        {
            return this._itemCount;
        }
    }

    public ReadOnlyObservableCollection<object> Items
    {
        get
        {
            return this._itemsRO;
        }
    }

    public object Name
    {
        get
        {
            return this._name;
        }
    }

    protected int ProtectedItemCount
    {
        get
        {
            return this._itemCount;
        }
        set
        {
            this._itemCount = value;
            this.OnPropertyChanged(new PropertyChangedEventArgs("ItemCount"));
        }
    }

    protected ObservableCollection<object> ProtectedItems
    {
        get
        {
            return this._itemsRW;
        }
    }

    }
}
