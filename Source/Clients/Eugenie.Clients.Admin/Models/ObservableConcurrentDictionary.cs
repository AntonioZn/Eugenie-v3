namespace Eugenie.Clients.Admin.Models
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Threading;

    /// <summary>
    ///     Provides a thread-safe dictionary for use with data binding.
    /// </summary>
    /// <typeparam name="TKey">Specifies the type of the keys in this collection.</typeparam>
    /// <typeparam name="TValue">Specifies the type of the values in this collection.</typeparam>
    public class ObservableConcurrentDictionary<TKey, TValue> :
        ICollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>,
        INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly SynchronizationContext _context;
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

        /// <summary>
        ///     Initializes an instance of the ObservableConcurrentDictionary class.
        /// </summary>
        public ObservableConcurrentDictionary()
        {
            this._context = AsyncOperationManager.SynchronizationContext;
            this._dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        /// <summary>Event raised when the collection changes.</summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Event raised when a property on the collection changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Notifies observers of CollectionChanged or PropertyChanged of an update to the dictionary.
        /// </summary>
        private void NotifyObserversOfChange()
        {
            var collectionHandler = this.CollectionChanged;
            var propertyHandler = this.PropertyChanged;
            if (collectionHandler != null || propertyHandler != null)
            {
                this._context.Post(s =>
                                   {
                                       if (collectionHandler != null)
                                       {
                                           collectionHandler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                                       }
                                       if (propertyHandler != null)
                                       {
                                           propertyHandler(this, new PropertyChangedEventArgs("Count"));
                                           propertyHandler(this, new PropertyChangedEventArgs("Keys"));
                                           propertyHandler(this, new PropertyChangedEventArgs("Values"));
                                       }
                                   }, null);
            }
        }

        /// <summary>Attempts to add an item to the dictionary, notifying observers of any changes.</summary>
        /// <param name="item">The item to be added.</param>
        /// <returns>Whether the add was successful.</returns>
        private bool TryAddWithNotification(KeyValuePair<TKey, TValue> item)
        {
            return this.TryAddWithNotification(item.Key, item.Value);
        }

        /// <summary>Attempts to add an item to the dictionary, notifying observers of any changes.</summary>
        /// <param name="key">The key of the item to be added.</param>
        /// <param name="value">The value of the item to be added.</param>
        /// <returns>Whether the add was successful.</returns>
        private bool TryAddWithNotification(TKey key, TValue value)
        {
            var result = this._dictionary.TryAdd(key, value);
            if (result)
            {
                this.NotifyObserversOfChange();
            }
            return result;
        }

        /// <summary>Attempts to remove an item from the dictionary, notifying observers of any changes.</summary>
        /// <param name="key">The key of the item to be removed.</param>
        /// <param name="value">The value of the item removed.</param>
        /// <returns>Whether the removal was successful.</returns>
        private bool TryRemoveWithNotification(TKey key, out TValue value)
        {
            var result = this._dictionary.TryRemove(key, out value);
            if (result)
            {
                this.NotifyObserversOfChange();
            }
            return result;
        }

        /// <summary>Attempts to add or update an item in the dictionary, notifying observers of any changes.</summary>
        /// <param name="key">The key of the item to be updated.</param>
        /// <param name="value">The new value to set for the item.</param>
        /// <returns>Whether the update was successful.</returns>
        private void UpdateWithNotification(TKey key, TValue value)
        {
            this._dictionary[key] = value;
            this.NotifyObserversOfChange();
        }

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.TryAddWithNotification(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) this._dictionary).Clear();
            this.NotifyObserversOfChange();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>) this._dictionary).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) this._dictionary).CopyTo(array, arrayIndex);
        }

        int ICollection<KeyValuePair<TKey, TValue>>.Count
        {
            get
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>) this._dictionary).Count;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>) this._dictionary).IsReadOnly;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue temp;
            return this.TryRemoveWithNotification(item.Key, out temp);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>) this._dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>) this._dictionary).GetEnumerator();
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            this.TryAddWithNotification(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return this._dictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return this._dictionary.Keys;
            }
        }

        public bool Remove(TKey key)
        {
            TValue temp;
            return this.TryRemoveWithNotification(key, out temp);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._dictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get
            {
                return this._dictionary.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return this._dictionary[key];
            }
            set
            {
                this.UpdateWithNotification(key, value);
            }
        }

        #endregion
    }
}