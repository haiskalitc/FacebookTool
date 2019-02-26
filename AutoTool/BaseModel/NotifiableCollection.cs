using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool.BaseModel
{
    class NotifiableCollection<T> : Collection<T>, INotifyCollectionChanged
    {
        public NotifiableCollection() { }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        protected override void RemoveItem(int index)
        {
            T item = base[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Move(int oldIndex, int newIndex)
        {
            T item = base[oldIndex];
            base.SetItem(newIndex, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move,
                item, newIndex, oldIndex));
        }
        protected override void SetItem(int index, T item)
        {
            T oldValue = base[index];
            base.SetItem(index, item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                    item, oldValue));
        }

        #region INotifyCollectionChanged Members

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;


        #endregion
    }
}