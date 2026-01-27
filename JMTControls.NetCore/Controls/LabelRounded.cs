using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    /// <summary>
    /// Label personalizado con bordes redondeados, colores configurables e icono opcional
    /// </summary>
    [ToolboxBitmap(typeof(Label))]
    [DefaultEvent("Click")]
    public class LabelRounded : Control
    {
        #region Private Fields
        private int _borderRadius = 10;
        private int _borderThickness = 0;
        private Color _borderColor = Color.FromArgb(226, 232, 240);
        private Color _backgroundColor = Color.White;
        private Color _textColor = Color.FromArgb(51, 65, 85);
        private Color _iconColor = Color.FromArgb(99, 102, 241);
        private Image? _icon;
        private IconAlignment _iconAlignment = IconAlignment.Left;
        private int _iconSize = 24;
        private int _iconSpacing = 10;
        private bool _showShadow = false;
        private Color _shadowColor = Color.FromArgb(50, 0, 0, 0);
        private int _shadowDepth = 3;
        private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;
        private Font _titleFont;
        private Font _valueFont;
        private string _title = string.Empty;
        private string _value = string.Empty;
        private bool _isTwoLineMode = false;
        private Color _valueColor = Color.FromArgb(15, 23, 42);
        #endregion

        #region Properties

        [Category("Appearance")]
        [Description("Radio de las esquinas redondeadas")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Grosor del borde")]
        public int BorderThickness
        {
            get => _borderThickness;
            set
            {
                _borderThickness = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Color del borde")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Color de fondo del control")]
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Color del texto")]
        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                Invalidate();
            }
        }

        [Category("Icon")]
        [Description("Imagen del icono")]
        public Image? Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                Invalidate();
            }
        }

        [Category("Icon")]
        [Description("Color del icono (si es una fuente de iconos)")]
        public Color IconColor
        {
            get => _iconColor;
            set
            {
                _iconColor = value;
                Invalidate();
            }
        }

        [Category("Icon")]
        [Description("Alineación del icono")]
        public IconAlignment IconAlignment
        {
            get => _iconAlignment;
            set
            {
                _iconAlignment = value;
                Invalidate();
            }
        }

        [Category("Icon")]
        [Description("Tamaño del icono")]
        public int IconSize
        {
            get => _iconSize;
            set
            {
                _iconSize = Math.Max(8, value);
                Invalidate();
            }
        }

        [Category("Icon")]
        [Description("Espaciado entre el icono y el texto")]
        public int IconSpacing
        {
            get => _iconSpacing;
            set
            {
                _iconSpacing = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Mostrar sombra")]
        public bool ShowShadow
        {
            get => _showShadow;
            set
            {
                _showShadow = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Color de la sombra")]
        public Color ShadowColor
        {
            get => _shadowColor;
            set
            {
                _shadowColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Profundidad de la sombra")]
        public int ShadowDepth
        {
            get => _shadowDepth;
            set
            {
                _shadowDepth = Math.Max(0, Math.Min(10, value));
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Alineación del texto")]
        public ContentAlignment TextAlign
        {
            get => _textAlign;
            set
            {
                _textAlign = value;
                Invalidate();
            }
        }

        [Category("Two-Line Mode")]
        [Description("Activar modo de dos líneas (título y valor)")]
        public bool IsTwoLineMode
        {
            get => _isTwoLineMode;
            set
            {
                _isTwoLineMode = value;
                Invalidate();
            }
        }

        [Category("Two-Line Mode")]
        [Description("Texto del título (primera línea)")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value ?? string.Empty;
                Invalidate();
            }
        }

        [Category("Two-Line Mode")]
        [Description("Texto del valor (segunda línea)")]
        public string Value
        {
            get => _value;
            set
            {
                _value = value ?? string.Empty;
                Invalidate();
            }
        }

        [Category("Two-Line Mode")]
        [Description("Fuente del título")]
        public Font TitleFont
        {
            get => _titleFont;
            set
            {
                _titleFont = value ?? this.Font;
                Invalidate();
            }
        }

        [Category("Two-Line Mode")]
        [Description("Fuente del valor")]
        public Font ValueFont
        {
            get => _valueFont;
            set
            {
                _valueFont = value ?? new Font(this.Font.FontFamily, this.Font.Size + 4, FontStyle.Bold);
                Invalidate();
            }
        }

        [Category("Two-Line Mode")]
        [Description("Color del valor")]
        public Color ValueColor
        {
            get => _valueColor;
            set
            {
                _valueColor = value;
                Invalidate();
            }
        }

        #endregion

        #region Constructor

        public LabelRounded()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.SetStyle(ControlStyles.UserPaint |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.SupportsTransparentBackColor, true);

            this.BackColor = Color.Transparent;
            this.ForeColor = _textColor;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            _titleFont = new Font("Segoe UI", 8F, FontStyle.Regular);
            _valueFont = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.Size = new Size(200, 80);
            this.Padding = new Padding(12);
        }

        #endregion

        #region Override Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Calcular rectángulo principal
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            // Ajustar por sombra
            if (_showShadow)
            {
                rect = new Rectangle(0, 0, this.Width - _shadowDepth - 1, this.Height - _shadowDepth - 1);
                DrawShadow(g, rect);
            }

            // Dibujar fondo
            DrawBackground(g, rect);

            // Dibujar borde
            if (_borderThickness > 0)
            {
                DrawBorder(g, rect);
            }

            // Dibujar contenido
            if (_isTwoLineMode)
            {
                DrawTwoLineContent(g, rect);
            }
            else
            {
                DrawSingleLineContent(g, rect);
            }
        }

        #endregion

        #region Drawing Methods

        private void DrawShadow(Graphics g, Rectangle rect)
        {
            Rectangle shadowRect = new Rectangle(
                rect.X + _shadowDepth,
                rect.Y + _shadowDepth,
                rect.Width,
                rect.Height);

            using (GraphicsPath shadowPath = GetRoundedRectangle(shadowRect, _borderRadius))
            using (SolidBrush shadowBrush = new SolidBrush(_shadowColor))
            {
                g.FillPath(shadowBrush, shadowPath);
            }
        }

        private void DrawBackground(Graphics g, Rectangle rect)
        {
            using (GraphicsPath path = GetRoundedRectangle(rect, _borderRadius))
            using (SolidBrush brush = new SolidBrush(_backgroundColor))
            {
                g.FillPath(brush, path);
            }
        }

        private void DrawBorder(Graphics g, Rectangle rect)
        {
            Rectangle borderRect = new Rectangle(
                rect.X + _borderThickness / 2,
                rect.Y + _borderThickness / 2,
                rect.Width - _borderThickness,
                rect.Height - _borderThickness);

            using (GraphicsPath path = GetRoundedRectangle(borderRect, _borderRadius))
            using (Pen pen = new Pen(_borderColor, _borderThickness))
            {
                g.DrawPath(pen, path);
            }
        }

        private void DrawSingleLineContent(Graphics g, Rectangle rect)
        {
            Rectangle contentRect = new Rectangle(
                rect.X + this.Padding.Left,
                rect.Y + this.Padding.Top,
                rect.Width - this.Padding.Horizontal,
                rect.Height - this.Padding.Vertical);

            // Calcular espacio para icono
            Rectangle iconRect = Rectangle.Empty;
            Rectangle textRect = contentRect;

            if (_icon != null)
            {
                iconRect = CalculateIconRectangle(contentRect);
                textRect = CalculateTextRectangle(contentRect, iconRect);
            }

            // Dibujar icono
            if (_icon != null && !iconRect.IsEmpty)
            {
                DrawIcon(g, iconRect);
            }

            // Dibujar texto
            if (!string.IsNullOrEmpty(this.Text))
            {
                DrawText(g, this.Text, this.Font, _textColor, textRect, _textAlign);
            }
        }

        private void DrawTwoLineContent(Graphics g, Rectangle rect)
        {
            Rectangle contentRect = new Rectangle(
                rect.X + this.Padding.Left,
                rect.Y + this.Padding.Top,
                rect.Width - this.Padding.Horizontal,
                rect.Height - this.Padding.Vertical);

            // Calcular posiciones
            int totalHeight = (int)(g.MeasureString(_title, _titleFont).Height + 
                                   g.MeasureString(_value, _valueFont).Height + 5);
            
            int startY = contentRect.Y + (contentRect.Height - totalHeight) / 2;

            // Dibujar título
            if (!string.IsNullOrEmpty(_title))
            {
                Rectangle titleRect = new Rectangle(contentRect.X, startY, contentRect.Width, 
                    (int)g.MeasureString(_title, _titleFont).Height);
                DrawText(g, _title, _titleFont, _textColor, titleRect, ContentAlignment.TopCenter);
            }

            // Dibujar valor
            if (!string.IsNullOrEmpty(_value))
            {
                Rectangle valueRect = new Rectangle(contentRect.X, 
                    startY + (int)g.MeasureString(_title, _titleFont).Height + 5, 
                    contentRect.Width, 
                    (int)g.MeasureString(_value, _valueFont).Height);
                DrawText(g, _value, _valueFont, _valueColor, valueRect, ContentAlignment.TopCenter);
            }
        }

        private Rectangle CalculateIconRectangle(Rectangle contentRect)
        {
            int iconX = 0, iconY = 0;

            switch (_iconAlignment)
            {
                case IconAlignment.Left:
                    iconX = contentRect.X;
                    iconY = contentRect.Y + (contentRect.Height - _iconSize) / 2;
                    break;

                case IconAlignment.Right:
                    iconX = contentRect.Right - _iconSize;
                    iconY = contentRect.Y + (contentRect.Height - _iconSize) / 2;
                    break;

                case IconAlignment.Top:
                    iconX = contentRect.X + (contentRect.Width - _iconSize) / 2;
                    iconY = contentRect.Y;
                    break;

                case IconAlignment.Bottom:
                    iconX = contentRect.X + (contentRect.Width - _iconSize) / 2;
                    iconY = contentRect.Bottom - _iconSize;
                    break;

                case IconAlignment.Center:
                    iconX = contentRect.X + (contentRect.Width - _iconSize) / 2;
                    iconY = contentRect.Y + (contentRect.Height - _iconSize) / 2;
                    break;
            }

            return new Rectangle(iconX, iconY, _iconSize, _iconSize);
        }

        private Rectangle CalculateTextRectangle(Rectangle contentRect, Rectangle iconRect)
        {
            Rectangle textRect = contentRect;

            switch (_iconAlignment)
            {
                case IconAlignment.Left:
                    textRect = new Rectangle(
                        iconRect.Right + _iconSpacing,
                        contentRect.Y,
                        contentRect.Width - _iconSize - _iconSpacing,
                        contentRect.Height);
                    break;

                case IconAlignment.Right:
                    textRect = new Rectangle(
                        contentRect.X,
                        contentRect.Y,
                        contentRect.Width - _iconSize - _iconSpacing,
                        contentRect.Height);
                    break;

                case IconAlignment.Top:
                    textRect = new Rectangle(
                        contentRect.X,
                        iconRect.Bottom + _iconSpacing,
                        contentRect.Width,
                        contentRect.Height - _iconSize - _iconSpacing);
                    break;

                case IconAlignment.Bottom:
                    textRect = new Rectangle(
                        contentRect.X,
                        contentRect.Y,
                        contentRect.Width,
                        contentRect.Height - _iconSize - _iconSpacing);
                    break;

                case IconAlignment.Center:
                    // En modo center, el texto se superpone con el icono
                    textRect = contentRect;
                    break;
            }

            return textRect;
        }

        private void DrawIcon(Graphics g, Rectangle iconRect)
        {
            if (_icon != null)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(_icon, iconRect);
            }
        }

        private void DrawText(Graphics g, string text, Font font, Color color, Rectangle rect, ContentAlignment align)
        {
            using (SolidBrush brush = new SolidBrush(color))
            using (StringFormat sf = new StringFormat())
            {
                sf.LineAlignment = GetVerticalAlignment(align);
                sf.Alignment = GetHorizontalAlignment(align);
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.NoWrap;

                g.DrawString(text, font, brush, rect, sf);
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            // Limitar el radio al tamaño del rectángulo
            if (diameter > rect.Width) diameter = rect.Width;
            if (diameter > rect.Height) diameter = rect.Height;

            Rectangle arc = new Rectangle(rect.Location, new Size(diameter, diameter));

            // Esquina superior izquierda
            path.AddArc(arc, 180, 90);

            // Esquina superior derecha
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Esquina inferior derecha
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Esquina inferior izquierda
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private StringAlignment GetHorizontalAlignment(ContentAlignment align)
        {
            switch (align)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    return StringAlignment.Near;

                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    return StringAlignment.Center;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    return StringAlignment.Far;

                default:
                    return StringAlignment.Near;
            }
        }

        private StringAlignment GetVerticalAlignment(ContentAlignment align)
        {
            switch (align)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    return StringAlignment.Near;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    return StringAlignment.Center;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    return StringAlignment.Far;

                default:
                    return StringAlignment.Center;
            }
        }

        #endregion
    }

    #region Enums

    public enum IconAlignment
    {
        Left,
        Right,
        Top,
        Bottom,
        Center
    }

    #endregion
}
