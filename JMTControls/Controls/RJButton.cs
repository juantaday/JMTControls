using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using JMControls.Enums;
using JMControls.Helpers;

namespace JMControls.Controls
{
	public class RJButton : Button
	{
		private int borderSize = 1;

		private int borderRadius = 12;

		private Color borderColor = Color.Red;

		[Category("RJ Code Advance")]
		public Color BackgroundColor
		{
			get
			{
				return this.BackColor;
			}
			set
			{
				this.BackColor = value;
			}
		}

		[Category("RJ Code Advance")]
		public Color BorderColor
		{
			get
			{
				return this.borderColor;
			}
			set
			{
				this.borderColor = value;
				base.Invalidate();
			}
		}

		[Category("RJ Code Advance")]
		public int BorderRadius
		{
			get
			{
				return this.borderRadius;
			}
			set
			{
				if (value > base.Height)
				{
					this.borderRadius = base.Height;
				}
				else
				{
					this.borderRadius = value;
				}
				base.Invalidate();
			}
		}

		[Category("RJ Code Advance")]
		public int BorderSize
		{
			get
			{
				return this.borderSize;
			}
			set
			{
				this.borderSize = value;
				base.Invalidate();
			}
		}

		[Category("RJ Code Advance")]
		public Color TextColor
		{
			get
			{
				return this.ForeColor;
			}
			set
			{
				this.ForeColor = value;
			}
		}

		public RJButton()
		{
			base.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			base.FlatAppearance.BorderSize = 0;
			base.Size = new System.Drawing.Size(150, 40);
			this.BackColor = Color.MediumSlateBlue;
			this.ForeColor = Color.White;
			base.Resize += new EventHandler(this.Button_Resize);
		}

		private void Button_Resize(object sender, EventArgs e)
		{
			if (this.borderRadius > base.Height)
			{
				this.borderRadius = base.Height;
			}
		}

		private void Container_BackColorChanged(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				base.Invalidate();
			}
		}

		private GraphicsPath GetFigurePath(Rectangle rect, float radius)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			float single = radius * 2f;
			graphicsPath.StartFigure();
			graphicsPath.AddArc((float)rect.X, (float)rect.Y, single, single, 180f, 90f);
			graphicsPath.AddArc((float)rect.Right - single, (float)rect.Y, single, single, 270f, 90f);
			graphicsPath.AddArc((float)rect.Right - single, (float)rect.Bottom - single, single, single, 0f, 90f);
			graphicsPath.AddArc((float)rect.X, (float)rect.Bottom - single, single, single, 90f, 90f);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.Parent.BackColorChanged += new EventHandler(this.Container_BackColorChanged);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			Rectangle clientRectangle = base.ClientRectangle;
			Rectangle rectangle = Rectangle.Inflate(clientRectangle, -this.borderSize, -this.borderSize);
			int num = 2;
			if (this.borderSize > 0)
			{
				num = this.borderSize;
			}
			if (this.borderRadius <= 2)
			{
				pevent.Graphics.SmoothingMode = SmoothingMode.None;
				base.Region = new System.Drawing.Region(clientRectangle);
				if (this.borderSize >= 1)
				{
					using (Pen pen = new Pen(this.borderColor, (float)this.borderSize))
					{
						pen.Alignment = PenAlignment.Inset;
						pevent.Graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
					}
				}
			}
			else
			{
				using (GraphicsPath figurePath = this.GetFigurePath(clientRectangle, (float)this.borderRadius))
				{
					using (GraphicsPath graphicsPath = this.GetFigurePath(rectangle, (float)(this.borderRadius - this.borderSize)))
					{
						using (Pen pen1 = new Pen(base.Parent.BackColor, (float)num))
						{
							using (Pen pen2 = new Pen(this.borderColor, (float)this.borderSize))
							{
								pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
								base.Region = new System.Drawing.Region(figurePath);
								pevent.Graphics.DrawPath(pen1, figurePath);
								if (this.borderSize >= 1)
								{
									pevent.Graphics.DrawPath(pen2, graphicsPath);
								}
							}
						}
					}
				}
			}
		}
	}
}
