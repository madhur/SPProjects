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

namespace SPListDashboard
{
    public class SortableCollectionDeferRefresh : IDisposable
    {
        private readonly SortableCollectionView _collectionView;

        internal SortableCollectionDeferRefresh(SortableCollectionView collectionView)
        {
            _collectionView = collectionView;
        }

        public void Dispose()
        {
            // refresh the collection when disposed.
            _collectionView.Refresh();
        }
    }
}
