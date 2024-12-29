using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMControls.Controls
{
    public class CheckBoxColor : CheckBox
    {
        private bool _readyOnly= false;
        private bool alreadyChanged = false;

        public CheckBoxColor()
        {
            Appearance = System.Windows.Forms.Appearance.Button;
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            TextAlign = ContentAlignment.MiddleRight;
            FlatAppearance.BorderSize = 1;
            FlatAppearance.BorderColor = Color.Black;
            AutoSize = false;
            Height = 16;
            Width = 16;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            pevent.Graphics.Clear(BackColor);

           
            using (SolidBrush brush = new SolidBrush(ForeColor))
                pevent.Graphics.DrawString(Text, Font, brush,25,4);

            Point pt = new Point(4, 4);
            Rectangle rect = new Rectangle(pt, new Size(16, 16));

            pevent.Graphics.FillRectangle(Brushes.Beige, rect);

            if (Checked)
            {
                using (SolidBrush brush = new SolidBrush(Color.Red))
                using (Font wing = new Font("Wingdings", 12f))
                    pevent.Graphics.DrawString("ü", wing, brush, 2, 4);
            }
            pevent.Graphics.DrawRectangle(Pens.DarkSlateBlue, rect);

            Rectangle fRect = ClientRectangle;

            if (Focused)
            {
                fRect.Inflate(-1, -1);
                using (Pen pen = new Pen(Brushes.Red) { DashStyle = DashStyle.Solid })
                    pevent.Graphics.DrawRectangle(pen, fRect);
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            this.FlatAppearance.BorderColor = Color.Red;
            this.FlatAppearance.BorderSize = 1;
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            this.FlatAppearance.BorderColor = Color.Black;
            this.FlatAppearance.BorderSize = 1;
            base.OnLeave(e);
        }

        public bool ReadOnly { get => _readyOnly ; set => _readyOnly = value; }

        protected override void OnCheckedChanged(EventArgs e)
        {
            if (ReadOnly && !alreadyChanged)
            {
                alreadyChanged = true;  
                this.Checked = !this.Checked;
            }
            alreadyChanged = false;
            base.OnCheckedChanged(e);
        }
    }
}
