using JMTControls.NetCore.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class GroupBoxLiner : System.Windows.Forms.GroupBox
    {
        private Color _borderColor = Color.Black;
        private int radius;
        private int borderThickness;
        public GroupBoxLiner()
        {
            BorderRadius = 8;
            BorderThickness = 1;

        }
        public Color BorderColor
        {
            get { return this._borderColor; }
            set { this._borderColor = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //get the text size in groupbox
            Size tSize = TextRenderer.MeasureText(this.Text, this.Font);
            int _Y = tSize.Height / 2;


            Rectangle borderRect = new Rectangle(new Point(2,_Y), 
                new Size {
                    Height  = this.Height -(3+ _Y), 
                    Width=this.Width - (3 + borderThickness)
                });
          

            using (Pen pen = new Pen(BorderColor, BorderThickness))
            {
                if (BorderRadius == 0)
                {
                    Rectangle path = new Rectangle(
                        new Point(borderThickness, _Y+ borderThickness),
                        new Size {
                            Height = borderRect.Height - (BorderThickness * 2), 
                            Width = borderRect.Width - (BorderThickness * 2)
                        });

                    e.Graphics.DrawRectangle(pen, path);

                }
                else
                {
                    using (GraphicsPath path = new MakeRoundedRect(borderRect, this.BorderRadius, this.BorderRadius, false, true, false, true).Path)
                    {
                        e.Graphics.DrawPath(pen, path);

                    }
                }
            }


            Rectangle textRect = e.ClipRectangle;
            textRect.X = (textRect.X + 6);
            textRect.Width = tSize.Width+2;
            textRect.Height = tSize.Height;
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect);
        }

        public int BorderRadius
        {
            get { return radius; }

            set
            {
                radius = value;
                Invalidate();
            }
        }

        public int BorderThickness
        {
            get => borderThickness; set
            {
                borderThickness = value;
                Invalidate();
            }
        }

    }
}
