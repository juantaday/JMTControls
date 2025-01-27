using JMTControls.NetCore.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
	public class DetailControl : TabControl
	{
		#region Variables
		internal List<DataGridView> childGrid = new List<DataGridView>();
		internal DataSet _cDataset;
		#endregion
		#region Populate Childview
		public void Add(string tableName, string pageCaption)
		{
			TabPage tPage = new TabPage() { Text = pageCaption };
			this.TabPages.Add(tPage);
			DataGridView newGrid = new DataGridView() 
			{ 
				Dock = DockStyle.Fill, 
				DataSource = new DataView(_cDataset.Tables[tableName]),
				SelectionMode = DataGridViewSelectionMode.FullRowSelect
			};

			tPage.Controls.Add(newGrid);
			GridTheme.ApplyGridTheme(newGrid);
			GridTheme.SetGridRowHeader(newGrid);
			if (newGrid.Columns.Count > 0)
			{
				newGrid.Columns[0].Visible = false;
			}
			newGrid.RowPostPaint += GridTheme.RowPostPaint_HeaderCount;
			childGrid.Add(newGrid);
		}

        public void RemoveChildren()
        {
            if (this.TabPages?.Count > 0)
            {
                this.TabPages.Clear();
            }
        }

        #endregion
    }

}
