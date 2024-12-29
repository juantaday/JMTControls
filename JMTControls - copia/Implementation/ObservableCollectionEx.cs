using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JMControls.Implementation
{

    public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged, new()
    {
        private string propertyName;
        private NotifyCollectionChangedAction action;
        private bool initialize = false;
        private bool hasChanged = false;
        private object currentModel;
        // this collection also reacts to changes in its components' properties
        T entidad;
        public ObservableCollectionEx() : base()
        {
            entidad = new T();
            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ObservableCollectionEx_CollectionChanged);
        }

        public bool HasChanged
        {
            get { return hasChanged; }
        }

        public object GetCurrentModel {
            get
            {
                return this.currentModel;
            }
        }

        public string  GetOnEvenPropertyName
        {
            get
            {
                return this.propertyName;
            }
        }

        public NotifyCollectionChangedAction GetAction
        {
            get
            {
                return this.action;
            }
        }
        public void Begin()
        {
            initialize = true;
        }

        public bool IsInitialized { get => this.initialize; }

        void ObservableCollectionEx_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.action = e.Action;
            
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (T item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                    if (initialize)
                    {
                        hasChanged = true;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (T item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                    if (initialize)
                    {
                        var p = item.GetType().GetProperties().FirstOrDefault(x => x.Name == "IsCahged");
                        if (p != null)
                        {
                            item.GetType().GetProperty(p.Name)
                                  .SetValue(item, true);
                        }

                        hasChanged = true;
                    }
                }
            }
        }

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //This will get called when the property of an object inside the collection changes - note you must make it a 'reset' - dunno why
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

            this.currentModel = sender;
            this.propertyName = e.PropertyName;

            OnCollectionChanged(args);
            if (initialize)
            {
                var p = sender.GetType().GetProperties().FirstOrDefault(x => x.Name == "IsCahged");
                if (p != null)
                {
                    sender.GetType().GetProperty(p.Name)
                          .SetValue(sender, true);
                }

                hasChanged = true;
            }

        }
    }

    public class SpecialObservableCollection<T> : ObservableCollection<T>
    {
        public SpecialObservableCollection()
        {
            this.CollectionChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AddOrRemoveListToPropertyChanged(e.NewItems, true);
            AddOrRemoveListToPropertyChanged(e.OldItems, false);
        }

        private void AddOrRemoveListToPropertyChanged(IList list, Boolean add)
        {
            if (list == null) { return; }
            foreach (object item in list)
            {
                INotifyPropertyChanged o = item as INotifyPropertyChanged;
                if (o != null)
                {
                    if (add) { o.PropertyChanged += ListItemPropertyChanged; }
                    if (!add) { o.PropertyChanged -= ListItemPropertyChanged; }
                }
                else
                {
                    throw new Exception("INotifyPropertyChanged is required");
                }
            }
        }

        void ListItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnListItemChanged(this, e);
        }

        public delegate void ListItemChangedEventHandler(object sender, PropertyChangedEventArgs e);

        public event ListItemChangedEventHandler ListItemChanged;

        private void OnListItemChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (ListItemChanged != null) { this.ListItemChanged(this, e); }
        }


    }
    public class ListGeneric<T> : System.Collections.Generic.List<T> where T : class, INotifyPropertyChanged, new()
    {
        private bool initialize = false;
        private bool hasChanged = false;
        private T item;
        private List<T> list = new List<T>();

        public ListGeneric()
        {

        }
        public new void Add(T item)
        {
            list.Add(item);
            OnPropertyChanged("Add");
        }

        public new void Remove(T item)
        {
            list.Remove(item);
            OnPropertyChanged("Remove");
        }

        public new void Clear()
        {
            list.Clear();
            OnPropertyChanged("Clear");
        }

        public void Begin()
        {
            initialize = true;
        }
        public bool HasChanged
        {
            get { return true; }
        }

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (initialize)
            {
                hasChanged = true;
            }
        }
        protected void SetValue<R>(ref R backingField, R value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<R>.Default.Equals(backingField, value))
            {
                return;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
        }
        #endregion


    }
}
