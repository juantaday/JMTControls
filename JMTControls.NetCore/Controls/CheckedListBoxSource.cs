using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class CheckedListBoxSource : CheckedListBox
    {
        private IEnumerable _dataSource;
        private bool multiChecked = true;
        private bool Changering = false;

        public CheckedListBoxSource()
        {

        }

        public new IEnumerable DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;

                this.Items.Clear();
                if (_dataSource == null) return;

                foreach (var item in _dataSource)
                {
                    this.Items.Add(item);
                }
            }
        }

        public List<T> GetListItemsChecked<T>() where T : class, new()
        {
            if (_dataSource == null) return null;
            if (this.Items.Count == 0) return null;

            List<T> x2 = new List<T>();
            return this.CheckedItems.Cast<T>().ToList();

        }

        public bool MultiCheck
        {
            get => multiChecked;
            set
            {
                multiChecked = value;
                Invalidate();
            }
        }

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            if (Changering) return;
            if (!multiChecked && ice.NewValue == CheckState.Checked)
            {
                Changering = true;

                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.GetItemChecked(i) && i != ice.Index)
                    {
                        this.SetItemChecked(i, false);
                    }
                }
            }
            Changering = false;
            base.OnItemCheck(ice);
            Invalidate();
        }


        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (!multiChecked)
            {
                this.SetItemChecked(this.SelectedIndex, true);
            }
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

    }

}
