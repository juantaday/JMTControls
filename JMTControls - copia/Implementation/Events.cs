using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMControls.Implementation
{
    public class Events
    {

       
    }

    public class ItemsChangingEventArgs {
        public ItemsChangingEventArgs(IEnumerable oldObjects, IEnumerable newObjects)
        {
            this.oldObjects = oldObjects;
            this.NewObjects = newObjects;
        }
        /// <summary>
        /// Gets the objects that were in the list before it change.
        /// For virtual lists, this will always be null.
        /// </summary>
        public IEnumerable OldObjects
        {
            get { return oldObjects; }
        }
        private IEnumerable oldObjects;

        /// <summary>
        /// Gets or sets the objects that will be in the list after it changes.
        /// </summary>
        public IEnumerable NewObjects;
    }
    public class CancellableEventArgs : EventArgs
    {
        /// <summary>
        /// Has this event been cancelled by the event handler?
        /// </summary>
        public bool Canceled;
    }
}
