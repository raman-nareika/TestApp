using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Students.App.Collection
{
    public class ObservableCollectionWithItemNotify<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public ObservableCollectionWithItemNotify(IEnumerable<T> collection)
            : base(collection)
        {
            foreach (T item in Items)
                item.PropertyChanged += ItemOnPropertyChanged;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= ItemOnPropertyChanged;

            if (e.NewItems != null)
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += ItemOnPropertyChanged;

            base.OnCollectionChanged(e);
        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }
    }
}
