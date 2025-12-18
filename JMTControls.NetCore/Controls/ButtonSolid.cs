using JMTControls.NetCore.Helpers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public partial class ButtonSolid : Control, IButtonControl
    {
        #region Variables
        private int radius;
        private bool transparency;
        private MouseState state;
        private RoundedRectangleF roundedRect;
        private Color backColorNormal, backColorHover, backColorEnter, backColorDown;
        private Color strokeColor;
        private bool stroke;
        private Color borderColorHover;
        private Color borderColorActive;
        private Color borderColorIdle;
        private Color borderColorDisable;
        private int borderThickness;
        private DialogResult dialogoResult;
        private BorderStyle borderStyle;
        private StringAlignment txtAlignmentHorizontal;
        private StringAlignment txtAlignmentVertical;
        private ContentAlignment txtAlignment;
        private Image image;
        private ContentAlignment imageAlignment;
        private Point imageLocation;
        private Size imageSize;
        private bool autoSizeImage;
        private int imagePadding;
        #endregion

        #region Constructor
        public ButtonSolid()
        {
            InitializeComponent();

            // Dimensiones iniciales
            Width = 120;
            Height = 80;

            // Configuración de trazo
            stroke = false;
            strokeColor = Color.Gray;

            // Colores de fondo
            backColorNormal = Color.FromArgb(44, 188, 210);
            backColorHover = Color.FromArgb(33, 167, 188);
            backColorEnter = Color.FromArgb(64, 168, 183);
            backColorDown = Color.FromArgb(36, 164, 183);

            // Colores de borde
            borderColorIdle = Color.FromArgb(192, 0, 192);
            borderColorHover = Color.FromArgb(0, 0, 92);
            borderColorActive = Color.Navy;
            borderColorDisable = Color.FromArgb(150, 150, 150);

            // Configuración de bordes
            radius = 8;
            borderThickness = 1;
            borderStyle = BorderStyle.FixedSingle;

            roundedRect = new RoundedRectangleF(Width, Height, radius);

            // Estilos de control
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            ForeColor = Color.Black;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            state = MouseState.Leave;
            transparency = false;
            dialogoResult = DialogResult.None;

            // Configuración de imagen
            image = Properties.Resources.MiniApp;
            imageAlignment = ContentAlignment.MiddleCenter;
            imageLocation = new Point(10, 10);
            imageSize = new Size(32, 32);
            autoSizeImage = false;
            imagePadding = 5;

            // Configuración de texto
            txtAlignment = ContentAlignment.MiddleCenter;
            txtAlignmentHorizontal = StringAlignment.Center;
            txtAlignmentVertical = StringAlignment.Center;
        }
        #endregion

        #region Events
        protected override void OnPaint(PaintEventArgs e)
        {
            #region Transparency
            if (transparency)
                Transparenter.MakeTransparent(this, e.Graphics);
            #endregion

            #region Drawing Setup
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            roundedRect = new RoundedRectangleF(Width, Height, radius);
            e.Graphics.Clear(Parent?.BackColor ?? Color.Transparent);

            Rectangle rect = new Rectangle(0, 0, Width, Height);
            #endregion

            #region Background Drawing
            if (Enabled)
            {
                Color fillColor = backColorNormal;
                Color borderColor = borderColorIdle;

                switch (state)
                {
                    case MouseState.Enter:
                        fillColor = backColorEnter;
                        borderColor = borderColorHover;
                        break;
                    case MouseState.Down:
                        fillColor = backColorDown;
                        borderColor = borderColorActive;
                        break;
                }

                using (SolidBrush brush = new SolidBrush(fillColor))
                {
                    e.Graphics.FillPath(brush, roundedRect.Path);
                }

                if (borderStyle != BorderStyle.None && borderThickness > 0)
                {
                    using (Pen pen = new Pen(borderColor, borderThickness))
                    {
                        pen.Alignment = PenAlignment.Inset;
                        using (GraphicsPath path = new RoundedRectangleF(
                            Width - 1,
                            Height - 1,
                            radius).Path)
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
            }
            else
            {
                Color disabledColor = Color.FromArgb(200, 200, 200);
                using (SolidBrush brush = new SolidBrush(disabledColor))
                {
                    e.Graphics.FillPath(brush, roundedRect.Path);
                }

                if (borderStyle != BorderStyle.None && borderThickness > 0)
                {
                    using (Pen pen = new Pen(borderColorDisable, borderThickness))
                    {
                        pen.Alignment = PenAlignment.Inset;
                        using (GraphicsPath path = new RoundedRectangleF(
                            Width - 1,
                            Height - 1,
                            radius).Path)
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
            }
            #endregion

            #region Draw Image
            if (image != null)
            {
                Rectangle imageRect = CalculateImageRectangle();

                // Aplicar región de recorte para respetar los bordes redondeados
                using (Region oldClip = e.Graphics.Clip.Clone())
                {
                    e.Graphics.SetClip(roundedRect.Path);

                    e.Graphics.DrawImage(
                        image,
                        imageRect,
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);

                    e.Graphics.Clip = oldClip;
                }
            }
            #endregion

            #region Text Drawing
            if (!string.IsNullOrEmpty(Text))
            {
                Rectangle textRect = CalculateTextRectangle();

                using (StringFormat sf = new StringFormat
                {
                    LineAlignment = txtAlignmentVertical,
                    Alignment = txtAlignmentHorizontal,
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                })
                using (Brush brush = new SolidBrush(Enabled ? ForeColor : Color.Gray))
                {
                    e.Graphics.DrawString(Text, Font, brush, textRect, sf);
                }
            }
            #endregion

            base.OnPaint(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            base.OnClick(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            state = MouseState.Enter;
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            state = MouseState.Leave;
            base.OnMouseLeave(e);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Capture = false;
            state = MouseState.Down;
            base.OnMouseDown(e);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (state != MouseState.Leave)
                state = MouseState.Enter;
            base.OnMouseUp(e);
            Invalidate();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Calcula el rectángulo para la imagen basándose en la alineación configurada
        /// </summary>
        private Rectangle CalculateImageRectangle()
        {
            if (image == null)
                return Rectangle.Empty;

            Size imgSize = autoSizeImage ? imageSize : image.Size;
            int x = imagePadding;
            int y = imagePadding;

            if (imageAlignment == ContentAlignment.MiddleCenter)
            {
                x = (Width - imgSize.Width) / 2;
                y = (Height - imgSize.Height) / 2;
            }
            else
            {
                // Cálculo horizontal
                switch (imageAlignment)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.BottomLeft:
                        x = imagePadding;
                        break;

                    case ContentAlignment.TopCenter:
                    case ContentAlignment.BottomCenter:
                        x = (Width - imgSize.Width) / 2;
                        break;

                    case ContentAlignment.TopRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.BottomRight:
                        x = Width - imgSize.Width - imagePadding;
                        break;
                }

                // Cálculo vertical
                switch (imageAlignment)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.TopRight:
                        y = imagePadding;
                        break;

                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        y = (Height - imgSize.Height) / 2;
                        break;

                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.BottomRight:
                        y = Height - imgSize.Height - imagePadding;
                        break;
                }
            }

            return new Rectangle(x, y, imgSize.Width, imgSize.Height);
        }

        /// <summary>
        /// Calcula el rectángulo para el texto considerando el espacio de la imagen
        /// </summary>
        private Rectangle CalculateTextRectangle()
        {
            Rectangle textRect = ClientRectangle;

            // Aplicar padding general
            textRect.Inflate(-imagePadding, -imagePadding);

            return textRect;
        }

        /// <summary>
        /// Convierte ContentAlignment a StringAlignment para alineación horizontal
        /// </summary>
        private StringAlignment GetHorizontalAlignment(ContentAlignment alignment)
        {
            switch (alignment)
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
                    return StringAlignment.Center;
            }
        }

        /// <summary>
        /// Convierte ContentAlignment a StringAlignment para alineación vertical
        /// </summary>
        private StringAlignment GetVerticalAlignment(ContentAlignment alignment)
        {
            switch (alignment)
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

        #region Properties

        [Category("JMTControls.NetCore")]
        [Description("Habilita o deshabilita el trazo del botón")]
        [Browsable(true)]
        public bool Stroke
        {
            get => stroke;
            set
            {
                stroke = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color del borde cuando el botón está activo (presionado)")]
        [Browsable(true)]
        public Color BorderColorActive
        {
            get => borderColorActive;
            set
            {
                borderColorActive = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color del borde cuando el botón está deshabilitado")]
        [Browsable(true)]
        public Color BorderColorDisable
        {
            get => borderColorDisable;
            set
            {
                borderColorDisable = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color del borde en estado normal (inactivo)")]
        [Browsable(true)]
        public Color BorderColorIdle
        {
            get => borderColorIdle;
            set
            {
                borderColorIdle = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color del borde cuando el mouse está sobre el botón")]
        [Browsable(true)]
        public Color BorderColorHover
        {
            get => borderColorHover;
            set
            {
                borderColorHover = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color del trazo del botón")]
        [Browsable(true)]
        public Color StrokeColor
        {
            get => strokeColor;
            set
            {
                strokeColor = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Estilo del borde del botón")]
        [Browsable(true)]
        public BorderStyle BorderStyle
        {
            get => borderStyle;
            set
            {
                borderStyle = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Grosor del borde en píxeles")]
        [Browsable(true)]
        public int BorderThickness
        {
            get => borderThickness;
            set
            {
                borderThickness = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Radio de las esquinas redondeadas")]
        [Browsable(true)]
        public int Radius
        {
            get => radius;
            set
            {
                radius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color de fondo en estado normal")]
        [Browsable(true)]
        public Color BackColorNormal
        {
            get => backColorNormal;
            set
            {
                backColorNormal = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color de fondo cuando el mouse está sobre el botón")]
        [Browsable(true)]
        public Color BackColorHover
        {
            get => backColorHover;
            set
            {
                backColorHover = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color de fondo cuando el mouse entra en el área del botón")]
        [Browsable(true)]
        public Color BackColorEnter
        {
            get => backColorEnter;
            set
            {
                backColorEnter = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Color de fondo cuando el botón es presionado")]
        [Browsable(true)]
        public Color BackColorDown
        {
            get => backColorDown;
            set
            {
                backColorDown = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Habilita transparencia en el botón")]
        [Browsable(true)]
        public bool Transparency
        {
            get => transparency;
            set
            {
                transparency = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Imagen que se muestra en el botón")]
        [Browsable(true)]
        public Image Image
        {
            get => image;
            set
            {
                image = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Alineación de la imagen dentro del botón")]
        [Browsable(true)]
        public ContentAlignment ImageAlignment
        {
            get => imageAlignment;
            set
            {
                imageAlignment = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Ubicación personalizada de la imagen (solo si no es Center)")]
        [Browsable(true)]
        public Point ImageLocation
        {
            get => imageLocation;
            set
            {
                imageLocation = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Tamaño de la imagen cuando AutoSizeImage está habilitado")]
        [Browsable(true)]
        public Size ImageSize
        {
            get => imageSize;
            set
            {
                imageSize = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Redimensiona automáticamente la imagen al tamaño especificado")]
        [Browsable(true)]
        public bool AutoSizeImage
        {
            get => autoSizeImage;
            set
            {
                autoSizeImage = value;
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Espaciado alrededor de la imagen en píxeles")]
        [Browsable(true)]
        public int ImagePadding
        {
            get => imagePadding;
            set
            {
                imagePadding = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("JMTControls.NetCore")]
        [Description("Alineación del texto dentro del botón")]
        [Browsable(true)]
        public ContentAlignment TextAling
        {
            get => txtAlignment;
            set
            {
                txtAlignment = value;
                txtAlignmentHorizontal = GetHorizontalAlignment(value);
                txtAlignmentVertical = GetVerticalAlignment(value);
                Invalidate();
            }
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public DialogResult DialogResult
        {
            get => dialogoResult;
            set => dialogoResult = value;
        }
        #endregion

        #region IButtonControl Implementation
        public void NotifyDefault(bool value)
        {
            // Implementación requerida por IButtonControl
        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }
        #endregion
    }
}