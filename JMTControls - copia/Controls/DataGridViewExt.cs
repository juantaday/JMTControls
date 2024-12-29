using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMControls.Controls
{
    public  class DataGridViewExt : DataGridView
    {
       public  DataGridViewExt()
        {
            this.CellContentClick += CellContentClicked;
        }

        public event CellButtonClickEventHandler CellButtonClick;

        public delegate void CellButtonClickEventHandler(DataGridView sender, DataGridViewCellEventArgs e);

        private void CellContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (this.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                CellButtonClick?.Invoke(this, e);
            }
        }
    }

}
