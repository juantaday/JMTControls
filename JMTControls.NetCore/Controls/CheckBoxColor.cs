using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class CheckBoxColor : CheckBox
    {
        private bool _readyOnly = false;
        private bool alreadyChanged = false;
        private Color _colorChecked = Color.Green;
        private bool _showUncheckedSymbol = true;
        private Color _uncheckedSymbolColor = Color.Red;

        public CheckBoxColor()
        {
            Appearance = Appearance.Button;
            FlatStyle = FlatStyle.Flat;
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
                pevent.Graphics.DrawString(Text, Font, brush, 25, 4);

            Point pt = new Point(4, 4);
            Rectangle rect = new Rectangle(pt, new Size(16, 16));

            pevent.Graphics.FillRectangle(Brushes.Beige, rect);

            using (Font wing = new Font("Wingdings", 14f))
            {
                if (Checked)
                {
                    using (SolidBrush brush = new SolidBrush(this.ColorChecked))
                        pevent.Graphics.DrawString("ü", wing, brush, 2, 4); // ✔
                }
                else if (ShowUncheckedSymbol)
                {
                    using (SolidBrush brush = new SolidBrush(this.UncheckedSymbolColor))
                        pevent.Graphics.DrawString("û", wing, brush, 2, 4); // ✘
                }
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

        public bool ReadOnly
        {
            get => _readyOnly;
            set => _readyOnly = value;
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Color of the checked state")]
        public Color ColorChecked
        {
            get => _colorChecked;
            set
            {
                if (_colorChecked != value)
                {
                    _colorChecked = value;
                    Invalidate();
                }
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Color of the symbol when unchecked")]
        public Color UncheckedSymbolColor
        {
            get => _uncheckedSymbolColor;
            set
            {
                _uncheckedSymbolColor = value;
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Whether to show a symbol when unchecked")]
        public bool ShowUncheckedSymbol
        {
            get => _showUncheckedSymbol;
            set
            {
                _showUncheckedSymbol = value;
                Invalidate();
            }
        }

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
