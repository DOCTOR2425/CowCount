using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CowCount
{
    [Serializable]
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        public ObservableCollectionEx()
        {
        }

        public ObservableCollectionEx(IEnumerable<T> list) : base(list)
        {
        }

        /// Adds the elements of the specified collection to the end of the
        /// <see cref="ObservableCollection{T}" />
        /// .
        public void AddRange(IEnumerable<T> items)
        {
            var itemsToAdd = items as T[] ?? items.ToArray();
            if (!itemsToAdd.Any())
            {
                return;
            }

            CheckReentrancy();
            foreach (var item in itemsToAdd)
            {
                Items.Add(item);
            }

            NotifyBindings();
        }

        // За основу взята реализация из Xamarin.CommunityToolkit:
        // https://github.com/xamarin/XamarinCommunityToolkit/blob/main/src/CommunityToolkit/Xamarin.CommunityToolkit/ObjectModel/ObservableRangeCollection.shared.cs
        // В предыдущей реализации метода при удалении любого количества возбуждается событие с флагом NotifyCollectionChangedAction.Reset,
        // что может перевести к полному перестроению коллекций, связанный с изменённой.
        // Что-бы этого избежать в данной реализации есть возможность использовать флаг NotifyCollectionChangedAction.Remove,
        // что позволяет уведомлять об удалении конкретных элементов в коллеции.
        // Флаг по умолчанию - NotifyCollectionChangedAction.Reset, благодаря этому, всё вызовы, в которых явно не указан Флаг Remove,
        // будут использовать старую логику удаления
        // Изменения относительно библиотечной реализации:
        //     - В слуачае флага Remove, события возбуждаются при каждом удалении, т.к. при удалении одновремнно нескольких элементов возбуждается исключение
        /// Removes the first occurence of each item in the specified collection from
        /// <see cref="ObservableCollection{T}" />
        /// .
        public void RemoveRange(IEnumerable<T> collection,
                                NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Reset)
        {
            if (notificationMode != NotifyCollectionChangedAction.Remove &&
                notificationMode != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException("Mode must be either Remove or Reset for RemoveRange.",
                    nameof(notificationMode));
            }

            ArgumentNullException.ThrowIfNull(collection);

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                var itemsToRemove = collection as T[] ?? collection.ToArray();
                var raiseEvents = false;

                foreach (var item in itemsToRemove)
                {
                    Items.Remove(item);
                    raiseEvents = true;
                }

                if (raiseEvents)
                {
                    RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Reset, null, -1);
                }

                return;
            }

            bool stay;
            var changedItems = new List<T>(collection);
            for (var i = 0; i < changedItems.Count; i += stay ? 0 : 1)
            {
                stay = false;

                if (!Items.Remove(changedItems[i]))
                {
                    changedItems
                        .RemoveAt(i); // Can't use a foreach because changedItems is intended to be (carefully) modified
                    stay = true;
                }
                else
                {
                    RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Remove, [changedItems[i]], -1);
                }
            }
        }

        /// Clears the current collection and replaces it with the specified collection.
        public void ReplaceRange(IEnumerable<T> items)
        {
            CheckReentrancy();
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }

            NotifyBindings();
        }

        public void NotifyBindings()
        {
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionReset();
        }

        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void RaiseChangeNotificationEvents(NotifyCollectionChangedAction action, List<T>? changedItems,
                                                   int startingIndex)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

            if (changedItems == null)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(action));
            }
            else
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItems,
                    startingIndex));
            }
        }
    }
}
