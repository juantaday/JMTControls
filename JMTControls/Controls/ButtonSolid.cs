using JMControls.Enums;
using JMControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMControls.Controls
{
    public partial class ButtonSolid : Control, IButtonControl
    {
        #region Variables
        int radius;
        bool transparency;
        MouseState state;
        RoundedRectangleF roundedRect;
        Color _backColorNormal, _backColorHover, _backColorEnter, _backColorDown;
        private Color strokeColor;
        private bool stroke;
        private Color _borderColorHover;
        private Color _borderColorActive;
        private Color _borderColorIdle;
        private Color borderColorDisable;
        private int borderThickness;
        DialogResult dialogoResult;
        private BorderStyle _borderStyle;
        private StringAlignment _txtAlignmentHorizontal;
        private StringAlignment _txtAlignmentVertical;
        private ContentAlignment _txtAling;
        private Image _image;
        private ImageAlinement _imageAlinement;
        private Point _imageLocation;

        #endregion
        #region AltoButton
        public ButtonSolid()
        {

            InitializeComponent();
            Width = 120;
            Height = 80;
            stroke = false;
            strokeColor = Color.Gray;

            _backColorNormal = Color.FromArgb(44, 188, 210);
            _backColorHover = Color.FromArgb(33, 167, 188);
            _backColorEnter = Color.FromArgb(64, 168, 183);
            _backColorDown = Color.FromArgb(36, 164, 183);

            //border color
            _borderColorIdle = Color.FromArgb(192, 0, 192);
            _borderColorHover = Color.FromArgb(0, 0, 92);
            _borderColorActive = Color.Navy;

            radius = 8;
            roundedRect = new RoundedRectangleF(Width, Height, radius);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            ForeColor = Color.Black;
            Font = new System.Drawing.Font("Comic Sans MS", 10, FontStyle.Bold);
            state = MouseState.Leave;

            transparency = false;
            dialogoResult = DialogResult.None;
            borderThickness = 1;
            _borderStyle = BorderStyle.FixedSingle;

            this.Image = Properties.Resources.MiniApp;
            this.TextAling = ContentAlignment.BottomCenter;
            this.ImageAlinement = ImageAlinement.Center;
            this.ImageLocation = new System.Drawing.Point(20, 10);


        }
        #endregion
        #region Events
        protected override void OnPaint(PaintEventArgs e)
        {
            #region Transparency
            if (transparency)
                Transparenter.MakeTransparent(this, e.Graphics);
            #endregion

            #region Drawing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            roundedRect = new RoundedRectangleF(Width, Height, radius);
            e.Graphics.FillRectangle(Brushes.Transparent, this.ClientRectangle);

            int R1 = (_backColorEnter.R + _backColorNormal.R) / 2;
            int G1 = (_backColorEnter.G + _backColorNormal.G) / 2;
            int B1 = (_backColorEnter.B + _backColorNormal.B) / 2;

            int R2 = (_backColorDown.R + _backColorHover.R) / 2;
            int G2 = (_backColorDown.G + _backColorHover.G) / 2;
            int B2 = (_backColorDown.B + _backColorHover.B) / 2;

            Rectangle rect = new Rectangle(0, 0, Width, Height);

            if (this.Enabled)
            {
                if (state == MouseState.Leave)
                {
                    using (LinearGradientBrush inactiveGB = new LinearGradientBrush(rect, _backColorNormal, _backColorNormal, 90f))
                        e.Graphics.FillPath(inactiveGB, roundedRect.Path);

                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorIdle, BorderThickness))
                        using (GraphicsPath path = new RoundedRectangleF(Width - (radius > 0 ? 0 : 1), Height - (radius > 0 ? 0 : 1), radius).Path)
                            e.Graphics.DrawPath(pen, path);
                }

                else if (state == MouseState.Enter)
                {
                    using (LinearGradientBrush activeGB = new LinearGradientBrush(rect, _backColorEnter, _backColorEnter, 90f))
                        e.Graphics.FillPath(activeGB, roundedRect.Path);

                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorHover, BorderThickness))
                        using (GraphicsPath path = new RoundedRectangleF(Width - (radius > 0 ? 0 : 1), Height - (radius > 0 ? 0 : 1), radius).Path)
                            e.Graphics.DrawPath(pen, path);
                }


                else if (state == MouseState.Down)
                {
                    using (LinearGradientBrush activeGB = new LinearGradientBrush(rect, _backColorDown, _backColorDown, 90f))
                        e.Graphics.FillPath(activeGB, roundedRect.Path);

                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorActive, BorderThickness))
                        using (GraphicsPath path = new RoundedRectangleF(Width - (radius > 0 ? 0 : 1), Height - (radius > 0 ? 0 : 1), radius).Path)
                            e.Graphics.DrawPath(pen, path);

                }


            }
            else
            {
                Color linear1 = Color.FromArgb(190, 190, 190);
                Color linear2 = Color.FromArgb(210, 210, 210);
                using (LinearGradientBrush inactiveGB = new LinearGradientBrush(rect, linear1, linear2, 90f))
                {
                    e.Graphics.FillPath(inactiveGB, roundedRect.Path);
                    e.Graphics.DrawPath(new Pen(inactiveGB), roundedRect.Path);
                }

                if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                    using (Pen pen = new Pen(BorderColorDisable, BorderThickness))
                    using (GraphicsPath path = new RoundedRectangleF(Width - (radius > 0 ? 0 : 1), Height - (radius > 0 ? 0 : 1), radius).Path)
                        e.Graphics.DrawPath(pen, path);
            }


            #endregion

            #region Draw image
            if (_imageAlinement == ImageAlinement.Center)
                e.Graphics.DrawImage(_image,
                   (this.Width - _image.Width) / 2,
                   (this.Height - _image.Height) / 2,
                   _image.Width,
                   _image.Height);
            else
            {
                e.Graphics.DrawImage(_image,   
                    _imageLocation.X,
                   _imageLocation.Y,
                  _image.Width,
                  _image.Height);

            }
            #endregion


            #region Text Drawing
            using (StringFormat sf = new StringFormat()
            {
                LineAlignment = this._txtAlignmentVertical,
                Alignment = this._txtAlignmentHorizontal,
            })

            using (Brush brush = new SolidBrush(ForeColor))
                e.Graphics.DrawString(Text, Font, brush, this.ClientRectangle, sf);
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

        /// <summary>
        /// el evento se genera cuando hace click sobre la imagen del button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
            Capture = false;
            state = MouseState.Down;
            base.OnMouseDown(e);
            Invalidate();

        }
        /// <summary>
        /// the event is generated when you click and release the mouse on the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (state != MouseState.Leave)
                state = MouseState.Enter;
            base.OnMouseUp(e);
            Invalidate();

        }

        /// <summary>
        /// Handle clicks from PictureBox and Header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {
            state = MouseState.Enter;
            base.OnMouseEnter(e);
            Invalidate();
        }

        /// <summary>
        /// Handle clicks from PictureBox and Header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {
            state = MouseState.Leave;
            base.OnMouseLeave(e);
            Invalidate();
        }


        private void _pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_image != null)
            {
                var g = e.Graphics;

                // -- Optional -- //
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                // -- Optional -- //
                g.DrawImage(_image,
                    (this.Width - _image.Width)/2,
                    (this.Height - _image.Height)/2,
                    _image.Width,
                    _image.Height);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///Get or Set stroke
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set stroke")]
        [Browsable(true)]

        public bool Stroke
        {
            get { return stroke; }
            set
            {
                stroke = value;
                Invalidate();
            }
        }

        /// <summary>
        ///Get or Set border color active
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set border color active")]
        [Browsable(true)]
        public Color BorderColorActive
        {
            get { return _borderColorActive; }
            set
            {
                _borderColorActive = value;
                Invalidate();
            }

        }
        /// <summary>
        ///Get or Set border color disable
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set border color disable")]
        [Browsable(true)]

        public Color BorderColorDisable
        {
            get { return borderColorDisable; }
            set
            {
                borderColorDisable = value;
                Invalidate();
            }

        }
        /// <summary>
        ///Get or Set border color Idle
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set border color Idle")]
        [Browsable(true)]
        public Color BorderColorIdle
        {
            get { return _borderColorIdle; }
            set
            {
                _borderColorIdle = value;
                Invalidate();
            }

        }

        /// <summary>
        ///Get or Set border color hover
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set border color hover")]
        [Browsable(true)]

        public Color BorderColorHover
        {
            get { return _borderColorHover; }
            set
            {
                _borderColorHover = value;
                Invalidate();
            }

        }
        /// <summary>
        ///Get or Set stroke color
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set stroke color")]
        [Browsable(true)]

        public Color StrokeColor
        {
            get { return strokeColor; }
            set
            {
                strokeColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///Get or Set border style
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set border style")]
        [Browsable(true)]
        public BorderStyle BorderStyle
        {
            get => _borderStyle; set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        /// <summary>
        ///Get or Set border size
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set border size")]
        [Browsable(true)]

        public int BorderThickness
        {
            get => borderThickness;
            set
            {
                borderThickness = value;
                Invalidate();
            }
        }

        /// <summary>
        ///Get or Set border radios
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set border radios")]
        [Browsable(true)]
        public int Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                Invalidate();
            }
        }
        /// <summary>
        ///Get or Set background color
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set background color")]
        [Browsable(true)]
        public Color BackColorNormal
        {
            get
            {
                return _backColorNormal;
            }
            set
            {
                _backColorNormal = value;
                Invalidate();
            }
        }
        /// <summary>
        ///Get or Set background color when mous hover
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set background color when mous hover")]
        [Browsable(true)]
        public Color BackColorHover
        {
            get
            {
                return _backColorHover;
            }
            set
            {
                _backColorHover = value;
                Invalidate();
            }
        }
        /// <summary>
        ///Get or Set background color when enter mous
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set background color when enter mous")]
        [Browsable(true)]
        public Color BackColorEnter
        {
            get
            {
                return _backColorEnter;
            }
            set
            {
                _backColorEnter = value;
                Invalidate();
            }
        }
        /// <summary>
        ///Get or Set background color when clicked
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set background color when clicked")]
        [Browsable(true)]
        public Color BackColorDown
        {
            get
            {
                return _backColorDown;
            }
            set
            {
                _backColorDown = value;
                Invalidate();
            }
        }
        /// <summary>
        /// Transparency button
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set button transparents")]
        [Browsable(true)]
        public bool Transparency
        {
            get
            {
                return transparency;
            }
            set
            {
                transparency = value;
            }
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                Invalidate();
            }
        }

        public DialogResult DialogResult
        {
            get
            {
                return dialogoResult;
            }
            set
            {
                dialogoResult = value;
            }
        }

        public void NotifyDefault(bool value)
        {
        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Image location
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set image location in button")]
        [Browsable(true)]
        public Point ImageLocation { get => _imageLocation; 
            set {
                _imageLocation = value;
                Invalidate();
            }
        }


        /// <summary>
        /// Image in button
        /// </summary>
        [Category("JMControls")]
        [Description("Get or Set image in button")]
        [Browsable(true)]
        public Image Image { get => _image;
            set
            {
                _image = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Image alinemenet
        /// </summary>
        [Category("JMControls")]
        [Description("Aliniment image in button")]
        [Browsable(true)]
        public ImageAlinement ImageAlinement { get => _imageAlinement;
            set {
                _imageAlinement = value;
                Invalidate();
            }
           
        }
    


        public ContentAlignment TextAling { get=> _txtAling; 
            set {
                _txtAling = value;

                switch (_txtAling)
                {
                    case ContentAlignment.TopLeft:
                        this._txtAlignmentVertical = StringAlignment.Near;
                        this._txtAlignmentHorizontal = StringAlignment.Near;
                        break;
                    case ContentAlignment.TopCenter:
                        this._txtAlignmentVertical = StringAlignment.Near;
                        this._txtAlignmentHorizontal = StringAlignment.Center;
                        break;
                    case ContentAlignment.TopRight:
                        this._txtAlignmentVertical = StringAlignment.Near;
                        this._txtAlignmentHorizontal = StringAlignment.Far;
                        break;
                    case ContentAlignment.MiddleLeft:
                        this._txtAlignmentVertical = StringAlignment.Center;
                        this._txtAlignmentHorizontal = StringAlignment.Near;
                        break;
                    case ContentAlignment.MiddleCenter:
                        this._txtAlignmentVertical = StringAlignment.Center;
                        this._txtAlignmentHorizontal = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleRight:
                        this._txtAlignmentVertical = StringAlignment.Center;
                        this._txtAlignmentHorizontal = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomLeft:
                        this._txtAlignmentVertical = StringAlignment.Far;
                        this._txtAlignmentHorizontal = StringAlignment.Near;
                        break;
                    case ContentAlignment.BottomCenter:
                        this._txtAlignmentVertical = StringAlignment.Far;
                        this._txtAlignmentHorizontal = StringAlignment.Center;
                        break;
                    case ContentAlignment.BottomRight:
                        this._txtAlignmentVertical = StringAlignment.Far;
                        this._txtAlignmentHorizontal = StringAlignment.Far;
                        break;
                    default:
                        break;
                }

                Invalidate();
            }
        }

        #endregion

    }

}
