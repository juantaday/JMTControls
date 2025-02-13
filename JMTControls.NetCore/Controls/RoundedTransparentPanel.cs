using System;

namespace JMTControls.NetCore.Controls
{

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class RoundedGradientPanel : Panel
    {
        private int _borderRadius = 20;
        private int _borderSize = 2;
        private Color _borderColor = Color.Gray;
        private Color _gradientStart = Color.White;
        private Color _gradientEnd = Color.Silver;

        public RoundedGradientPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.SupportsTransparentBackColor, true);

           base.BackColor = Color.Transparent;
        }

        // Propiedades existentes
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; UpdateRegion(); Invalidate(); }
        }

        public int BorderSize
        {
            get => _borderSize;
            set { _borderSize = value; Invalidate(); }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        // Nuevas propiedades para el degradado
        public Color GradientStartColor
        {
            get => _gradientStart;
            set { _gradientStart = value; Invalidate(); }
        }

        public Color GradientEndColor
        {
            get => _gradientEnd;
            set { _gradientEnd = value; Invalidate(); }
        }

        private void UpdateRegion()
        {
            using (var path = CreateRoundedPath(0, 0, Width, Height, _borderRadius))
            {
                Region = new Region(path);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Crear rectángulo para el degradado
            var gradientRect = new Rectangle(0, 0, Width, Height);

            // Dibujar fondo degradado
            using (var path = CreateRoundedPath(gradientRect, BorderRadius))
            using (var gradientBrush = new LinearGradientBrush(
                gradientRect,
                _gradientStart,
                _gradientEnd,
                LinearGradientMode.ForwardDiagonal)) // Diagonal superior izquierda a inferior derecha
            {
                e.Graphics.FillPath(gradientBrush, path);
            }

            // Dibujar borde
            if (_borderSize > 0)
            {
                int borderInset = _borderSize / 2;
                var borderRect = new Rectangle(
                    borderInset,
                    borderInset,
                    Width - _borderSize ,
                    Height - _borderSize);

                using (var borderPath = CreateRoundedPath(borderRect, _borderRadius - borderInset))
                using (var pen = new Pen(_borderColor, _borderSize))
                {
                    e.Graphics.DrawPath(pen, borderPath);
                }
            }
        }

        private GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            return CreateRoundedPath(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        private GraphicsPath CreateRoundedPath(int x, int y, int width, int height, int radius)
        {
            var path = new GraphicsPath();

            if (radius > 0)
            {
                path.AddArc(x, y, radius, radius, 180, 90);
                path.AddArc(x + width - radius, y, radius, radius, 270, 90);
                path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
                path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            }
            else
            {
                path.AddRectangle(new Rectangle(x, y, width, height));
            }

            path.CloseFigure();
            return path;
        }
    }

}
