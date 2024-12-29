using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMControls.Tools
{
	public sealed class GridTheme
	{
		#region CustomGrid
		static System.Windows.Forms.DataGridViewCellStyle dateCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight };
		static System.Windows.Forms.DataGridViewCellStyle amountCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" };
		static System.Windows.Forms.DataGridViewCellStyle gridCellStyle = new System.Windows.Forms.DataGridViewCellStyle
		{
			Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
			BackColor = System.Drawing.Color.FromArgb(System.Convert.ToInt32(System.Convert.ToByte(79)), System.Convert.ToInt32(System.Convert.ToByte(129)), System.Convert.ToInt32(System.Convert.ToByte(189))),
			Font = new System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0)),
			ForeColor = System.Drawing.SystemColors.ControlLightLight,
			SelectionBackColor = System.Drawing.SystemColors.Highlight,
			SelectionForeColor = System.Drawing.SystemColors.HighlightText,
			WrapMode = System.Windows.Forms.DataGridViewTriState.True
		};
		static System.Windows.Forms.DataGridViewCellStyle gridCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle
		{
			Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
			BackColor = System.Drawing.SystemColors.ControlLightLight,
			Font = new System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0)),
			ForeColor = System.Drawing.SystemColors.ControlText,
			SelectionBackColor = System.Drawing.Color.FromArgb(System.Convert.ToInt32(System.Convert.ToByte(155)), System.Convert.ToInt32(System.Convert.ToByte(187)), System.Convert.ToInt32(System.Convert.ToByte(89))),
			SelectionForeColor = System.Drawing.SystemColors.HighlightText,
			WrapMode = System.Windows.Forms.DataGridViewTriState.False
		};
		static System.Windows.Forms.DataGridViewCellStyle gridCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle
		{
			Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
			BackColor = System.Drawing.Color.Lavender,
			Font = new System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0)),
			ForeColor = System.Drawing.SystemColors.WindowText,
			SelectionBackColor = System.Drawing.Color.FromArgb(System.Convert.ToInt32(System.Convert.ToByte(155)), System.Convert.ToInt32(System.Convert.ToByte(187)), System.Convert.ToInt32(System.Convert.ToByte(89))),
			SelectionForeColor = System.Drawing.SystemColors.HighlightText,
			WrapMode = System.Windows.Forms.DataGridViewTriState.True
		};
		public static void ApplyGridTheme(DataGridView grid)
		{
			grid.AllowUserToAddRows = false;
			grid.AllowUserToDeleteRows = false;
			grid.BackgroundColor = System.Drawing.SystemColors.Window;
			grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			grid.ColumnHeadersDefaultCellStyle = gridCellStyle;
			grid.ColumnHeadersHeight = 32;
			grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			grid.DefaultCellStyle = gridCellStyle2;
			grid.EnableHeadersVisualStyles = false;
			grid.GridColor = System.Drawing.SystemColors.GradientInactiveCaption;
			grid.ReadOnly = true;
			grid.RowHeadersVisible = true;
			grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			grid.RowHeadersDefaultCellStyle = gridCellStyle3;
			grid.Font = gridCellStyle.Font;
			grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			grid.BorderStyle = BorderStyle.FixedSingle;
			grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}
		public static void ApplyGridThemeEdit(DataGridView grid)
		{
			grid.AllowUserToAddRows = false;
			grid.AllowUserToDeleteRows = false;
			grid.BackgroundColor = System.Drawing.SystemColors.Window;
			grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			grid.ColumnHeadersDefaultCellStyle = gridCellStyle;
			grid.ColumnHeadersHeight = 32;
			grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			grid.DefaultCellStyle = gridCellStyle2;
			grid.EnableHeadersVisualStyles = false;
			grid.GridColor = System.Drawing.SystemColors.GradientInactiveCaption;
			grid.ReadOnly = false;
			grid.RowHeadersVisible = true;
			grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			grid.RowHeadersDefaultCellStyle = gridCellStyle3;
			grid.Font = gridCellStyle.Font;
			grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

		}
		public static void SetGridRowHeader(DataGridView dgv, bool hSize = false)
		{
			dgv.TopLeftHeaderCell.Value = "NO ";
			dgv.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
			foreach (DataGridViewColumn cCol in dgv.Columns)
			{
				if (cCol.ValueType.ToString() == typeof(DateTime).ToString())
				{
					cCol.DefaultCellStyle = dateCellStyle;
				}
				else if (cCol.ValueType.ToString() == typeof(decimal).ToString() || cCol.ValueType.ToString() == typeof(double).ToString())
				{
					cCol.DefaultCellStyle = amountCellStyle;
				}
			}
			if (hSize)
			{
				dgv.RowHeadersWidth = dgv.RowHeadersWidth + 16;
			}
			dgv.AutoResizeColumns();
		}
		public static void RowPostPaint_HeaderCount(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			//set rowheader count
			DataGridView grid = (DataGridView)sender;
			string rowIdx = System.Convert.ToString((e.RowIndex + 1).ToString());
			var centerFormat = new StringFormat();
			centerFormat.Alignment = StringAlignment.Center;
			centerFormat.LineAlignment = StringAlignment.Center;
			Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top,
				grid.RowHeadersWidth, e.RowBounds.Height - grid.Rows[e.RowIndex].DividerHeight);
			e.Graphics.DrawString(rowIdx, grid.Font, SystemBrushes.ControlText,
				headerBounds, centerFormat);
		}


		#endregion
	}
}
