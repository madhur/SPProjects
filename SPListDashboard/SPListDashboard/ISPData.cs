namespace SPListDashboard
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface ISPData
    {

         List<string> GetLists();

         List<string> GetColumns(string listName);

         SortableCollectionView GetData(string listName);

    }

}