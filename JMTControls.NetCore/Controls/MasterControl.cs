using JMTControls.NetCore.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
	public class MasterControl : DataGridView
	{
		#region Variables
		internal List<int> rowCurrent = new List<int>();
		internal System.Int32 rowDefaultHeight = 22;
		internal System.Int32 rowExpandedHeight = 250;
		internal System.Int32 rowDefaultDivider = 0;
		internal System.Int32 rowExpandedDivider = 300 - 22;
		internal System.Int32 rowDividerMargin = 5;
		internal bool collapseRow;
		public DetailControl childView; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
		private System.ComponentModel.IContainer components;

		//
		DataSet _cDataset;
		string _foreignKey;
		internal ImageList RowHeaderIconList;
		string _filterFormat;
		public enum rowHeaderIcons
		{
			expand = 0,
			collapse = 1
		}
		#endregion
		#region Initialze and Display
		public MasterControl(DataSet cDataset)
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			childView = new DetailControl() { Height = 300 - 22 - 5 * 2, Visible = false };

			this.Controls.Add(childView);
			InitializeComponent();
			_cDataset = cDataset;
			childView._cDataset = cDataset;
			GridTheme.ApplyGridTheme(this);
			Dock = DockStyle.Fill;

		}
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterControl));
            this.RowHeaderIconList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // RowHeaderIconList
            // 
            this.RowHeaderIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("RowHeaderIconList.ImageStream")));
            this.RowHeaderIconList.TransparentColor = System.Drawing.Color.Transparent;
            this.RowHeaderIconList.Images.SetKeyName(0, "expand.png");
            this.RowHeaderIconList.Images.SetKeyName(1, "collapse.png");
            // 
            // MasterControl
            // 
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		#region DataControl
		public void setParentSource(string tableName, string foreignKey)
		{
			this.DataSource = new DataView(_cDataset.Tables[tableName]);
			GridTheme.SetGridRowHeader(this);
			if (string.IsNullOrEmpty(foreignKey))
			{
				return;
			}
			_foreignKey = foreignKey;
			if (_cDataset.Tables[tableName].Columns[foreignKey].GetType().ToString() == typeof(int).ToString()
				|| _cDataset.Tables[tableName].Columns[foreignKey].GetType().ToString() == typeof(double).ToString()
				|| _cDataset.Tables[tableName].Columns[foreignKey].GetType().ToString() == typeof(decimal).ToString())
			{
				_filterFormat = foreignKey + "={0}";
			}
			else
			{
				_filterFormat = foreignKey + "='{0}'";
			}
		}
		#endregion
		#region GridEvents
		private void MasterControl_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			Rectangle rect = new Rectangle((rowDefaultHeight - 16) / 2, (rowDefaultHeight - 16) / 2, 16, 16);
			if (rect.Contains(e.Location))
			{
				if (rowCurrent.Contains(e.RowIndex))
				{
					rowCurrent.Clear();
					this.Rows[e.RowIndex].Height = rowDefaultHeight;
					this.Rows[e.RowIndex].DividerHeight = rowDefaultDivider;
				}
				else
				{
					if (!(rowCurrent.Count == 0))
					{
						var eRow = rowCurrent[0];
						rowCurrent.Clear();
						this.Rows[eRow].Height = rowDefaultHeight;
						this.Rows[eRow].DividerHeight = rowDefaultDivider;
						this.ClearSelection();
						collapseRow = true;
						this.Rows[eRow].Selected = true;
					}
					rowCurrent.Add(e.RowIndex);
					this.Rows[e.RowIndex].Height = rowExpandedHeight;
					this.Rows[e.RowIndex].DividerHeight = rowExpandedDivider;
				}
				this.ClearSelection();
				collapseRow = true;
				this.Rows[e.RowIndex].Selected = true;
			}
			else
			{
				collapseRow = false;
			}
		}
		private void MasterControl_RowPostPaint(DataGridView sender, DataGridViewRowPostPaintEventArgs e)
		{
			//set childview control
			var rect = new Rectangle(e.RowBounds.X + ((rowDefaultHeight - 16) / 2), e.RowBounds.Y + ((rowDefaultHeight - 16) / 2), 16, 16);
			if (collapseRow)
			{
				if (rowCurrent.Contains(e.RowIndex))
				{
					sender.Rows[e.RowIndex].DividerHeight = sender.Rows[e.RowIndex].Height - rowDefaultHeight;
					e.Graphics.DrawImage(RowHeaderIconList.Images[(int)rowHeaderIcons.collapse], rect);
					childView.Location = new Point(e.RowBounds.Left + sender.RowHeadersWidth, System.Convert.ToInt32(e.RowBounds.Top + rowDefaultHeight) + 5);
					childView.Width = e.RowBounds.Right - sender.RowHeadersWidth;
					childView.Height = sender.Rows[e.RowIndex].DividerHeight - 10;
					childView.Visible = true;
				}
				else
				{
					childView.Visible = false;
					e.Graphics.DrawImage(RowHeaderIconList.Images[(int)rowHeaderIcons.expand], rect);
				}
				collapseRow = false;
			}
			else
			{
				if (rowCurrent.Contains(e.RowIndex))
				{
					sender.Rows[e.RowIndex].DividerHeight = sender.Rows[e.RowIndex].Height - rowDefaultHeight;
					e.Graphics.DrawImage(RowHeaderIconList.Images[(int)rowHeaderIcons.collapse], rect);
					childView.Location = new Point(e.RowBounds.Left + sender.RowHeadersWidth, System.Convert.ToInt32(e.RowBounds.Top + rowDefaultHeight) + 5);
					childView.Width = e.RowBounds.Right - sender.RowHeadersWidth;
					childView.Height = sender.Rows[e.RowIndex].DividerHeight - 10;
					childView.Visible = true;
				}
				else
				{
					e.Graphics.DrawImage(RowHeaderIconList.Images[(int)rowHeaderIcons.expand], rect);
				}
			}

			GridTheme.RowPostPaint_HeaderCount(sender, e);
		}
		private void MasterControl_Scroll(object sender, ScrollEventArgs e)
		{
			if (!(rowCurrent.Count == 0))
			{
				collapseRow = true;
				this.ClearSelection();
				this.Rows[rowCurrent[0]].Selected = true;
			}
		}
		private void MasterControl_SelectionChanged(object sender, EventArgs e)
		{
			if (!(this.RowCount == 0))
			{
				if (rowCurrent.Contains(this.CurrentRow.Index))
				{
					foreach (DataGridView cGrid in childView.childGrid)
					{
						((DataView)cGrid.DataSource).RowFilter = string.Format(_filterFormat, this[_foreignKey, this.CurrentRow.Index].Value);
					}
				}
			}
		}

		#endregion
	}
}
