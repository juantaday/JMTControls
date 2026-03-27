using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace JMTControls.NetCore.Implementation
{

    public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged, new()
    {
        private string propertyName;
        private NotifyCollectionChangedAction action;

        private bool initialize = false;
        private bool hasChanged = false;
        private bool _isHandlingPropertyChange = false;

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

        public object GetCurrentModel
        {
            get
            {
                return this.currentModel;
            }
        }

        public string GetOnEvenPropertyName
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

        void ObservableCollectionEx_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

            if (_isHandlingPropertyChange) return;
            try
            {
                _isHandlingPropertyChange = true;

                this.currentModel = sender;
                this.propertyName = e.PropertyName;

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));

                if (initialize)
                {
                    // Nota: "IsCahged" tiene typo en el original — verificar si existe en el modelo
                    var prop = sender.GetType()
                                     .GetProperties()
                                     .FirstOrDefault(x => x.Name == "IsCahged");
                    prop?.SetValue(sender, true);
                    hasChanged = true;
                }
            }
            finally
            {
                _isHandlingPropertyChange = false;
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
}
