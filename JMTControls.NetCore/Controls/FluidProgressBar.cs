
namespace JMTControls.NetCore.Controls
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    /// <summary>
    /// Custom progress bar with immediate, fluid updates and modern design
    /// </summary>
    public class FluidProgressBar : Control
    {
        private int _value = 0;
        private int _minimum = 0;
        private int _maximum = 100;
        private Color _progressColor = Color.FromArgb(0, 122, 204); // Modern blue
        private Color _backGroundColor = Color.FromArgb(240, 240, 240);
        private int _borderRadius = 3;
        private bool _showPercentage = false;

        public FluidProgressBar()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(316, 6);
            BackColor = Color.White; // Changed from Transparent to White for designer compatibility
        }

        /// <summary>
        /// Current progress value (0-100 by default)
        /// </summary>
        public int Value
        {
            get => _value;
            set
            {
                if (value < _minimum) value = _minimum;
                if (value > _maximum) value = _maximum;

                if (_value != value)
                {
                    _value = value;
                    Invalidate(); // Immediate repaint - no delay
                }
            }
        }

        /// <summary>
        /// Minimum value (default: 0)
        /// </summary>
        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                if (_value < _minimum) _value = _minimum;
                Invalidate();
            }
        }

        /// <summary>
        /// Maximum value (default: 100)
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                if (_value > _maximum) _value = _maximum;
                Invalidate();
            }
        }

        /// <summary>
        /// Progress bar fill color
        /// </summary>
        public Color ProgressColor
        {
            get => _progressColor;
            set
            {
                _progressColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Background color of the progress bar
        /// </summary>
        public Color BarBackColor
        {
            get => _backGroundColor;
            set
            {
                _backGroundColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Border radius for rounded corners
        /// </summary>
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Show percentage text on the bar
        /// </summary>
        public bool ShowPercentage
        {
            get => _showPercentage;
            set
            {
                _showPercentage = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Clear with control's BackColor instead of parent's
            g.Clear(BackColor);

            // Calculate progress width
            float percentage = (float)(_value - _minimum) / (_maximum - _minimum);
            int progressWidth = (int)(Width * percentage);

            // Draw background
            using (GraphicsPath bgPath = GetRoundedRectangle(0, 0, Width, Height, _borderRadius))
            using (SolidBrush bgBrush = new SolidBrush(_backGroundColor))
            {
                g.FillPath(bgBrush, bgPath);
            }

            // Draw progress
            if (progressWidth > 0)
            {
                using (GraphicsPath progressPath = GetRoundedRectangle(0, 0, progressWidth, Height, _borderRadius))
                using (SolidBrush progressBrush = new SolidBrush(_progressColor))
                {
                    g.FillPath(progressBrush, progressPath);
                }
            }

            // Draw percentage text (optional)
            if (_showPercentage)
            {
                string percentText = $"{(int)(percentage * 100)}%";
                using (Font font = new Font("Segoe UI", 8f, FontStyle.Regular))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    SizeF textSize = g.MeasureString(percentText, font);
                    float x = (Width - textSize.Width) / 2;
                    float y = (Height - textSize.Height) / 2;
                    g.DrawString(percentText, font, textBrush, x, y);
                }
            }
        }

        private GraphicsPath GetRoundedRectangle(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(new Rectangle(x, y, width, height));
                return path;
            }

            int diameter = radius * 2;
            Rectangle arc = new Rectangle(x, y, diameter, diameter);

            // Top left
            path.AddArc(arc, 180, 90);

            // Top right
            arc.X = x + width - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right
            arc.Y = y + height - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left
            arc.X = x;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }

}
