using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class SeparatorControl : Control
    {
        private Orientation _orientation = Orientation.Vertical;
        private Color _lineColor = Color.Gray;
        private int _lineWidth = 2;
        private DashStyle _lineStyle = DashStyle.Solid;

        public SeparatorControl()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.Size = new Size(2, 200); // Tamaño inicial
        }

        // personaliza Backcolor
        // como hacer que se haga el color del contenedor 
        // por defecto pon color transaparente 


        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(typeof(Color), "Transparent")]        
        public new  Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }   


        [Category("Appearance")]
        [Description("Define la orientación del separador.")]
        public Orientation LineOrientation
        {
            get => _orientation;
            set
            {
                if (_orientation != value)
                {
                    _orientation = value;
                    AdjustSize();
                    this.Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [Description("Color de la línea del separador.")]
        public Color LineColor
        {
            get => _lineColor;
            set
            {
                if (_lineColor != value)
                {
                    _lineColor = value;
                    this.Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [Description("Grosor de la línea del separador.")]
        public int LineThickness
        {
            get => _lineWidth;
            set
            {
                if (value > 0 && _lineWidth != value)
                {
                    _lineWidth = value;
                    AdjustSize();
                    this.Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [Description("Estilo de la línea del separador.")]
        public DashStyle LineStyle
        {
            get => _lineStyle;
            set
            {
                if (_lineStyle != value)
                {
                    _lineStyle = value;
                    this.Invalidate();
                }
            }
        }

        private void AdjustSize()
        {
            if (_orientation == Orientation.Vertical)
            {
                this.Width = _lineWidth;
            }
            else
            {
                this.Height = _lineWidth;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(_lineColor, _lineWidth))
            {
                pen.DashStyle = _lineStyle;
                if (_orientation == Orientation.Vertical)
                {
                    int x = this.Width / 2;
                    e.Graphics.DrawLine(pen, x, 0, x, this.Height);
                }
                else
                {
                    int y = this.Height / 2;
                    e.Graphics.DrawLine(pen, 0, y, this.Width, y);
                }
            }
        }
    }
}
