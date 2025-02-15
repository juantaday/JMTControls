using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMTControls.NetCore.ExpandCollapsePanel
{
    /// <summary>
    /// Button with two states: expanded/collapsed
    /// </summary>
    [DefaultEvent("ExpandCollapse")]
    public partial class ExpandCollapseButton : UserControl
    {
        private Color _BackColorHover = Color.FromArgb(40, 40, 40);
        private Color _BackColor = Color.FromArgb(60, 60, 60);
        private int _padingLeftIcon;
        private Point _location;
        /// <summary>
        /// Image displays expanded state of button
        /// </summary>
        private Image _expanded;
        /// <summary>
        /// Image displays collapsed state of button
        /// </summary>
        private Image _collapsed;

        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        private bool _isExpanded;

        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Color de fondo cuado pasa el mause sobre el button.")]
        [Browsable(true)]
        public Color BackColorNormal
        {
            get
            {
                _BackColor = this.BackColor;
                return _BackColor;
            }
            set
            {
                this.BackColor = value;
                _BackColor = value;
            }
        }


        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Color de fondo cuado pasa el mause sobre el button.")]
        [Browsable(true)]
        public Color BackColorHover
        {
            get { return _BackColorHover; }
            set
            {
                _BackColorHover = value;
            }
        }

        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Location the image in button")]
        [Browsable(true)]
        public Point ButtonImgLocation
        {
            get { return _imagePicture.Location; }
            set
            {
                _imagePicture.Location = value;
            }
        }


        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        /// logoPicture
        [Category("ExpandCollapseButton")]
        [Description("Visible icon button")]
        [Browsable(true)]
        public bool VisibleIcon
        {
            get => logoPicture.Visible;
            set
            {
                logoPicture.Visible = value;
            }
        }


        /// (true - expanded, false - collapsed)
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Image picture")]
        [Browsable(true)]
        public Image ImagePicture
        {
            get { return _imagePicture.Image; }
            set
            {
                _imagePicture.Image = value;
            }
        }

        /// (true - expanded, false - collapsed)
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Image size")]
        [Browsable(true)]
        public Size ImageSize
        {
            get { return _imagePicture.Size; }
            set
            {
                _imagePicture.Size = value;
            }
        }

        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Image location")]
        [Browsable(true)]
        public Point ImageLocation
        {
            get { return _imagePicture.Location; }
            set
            {
                _imagePicture.Location = value;
            }
        }


        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Browsable(true)]
        [Category("ExpandCollapseButton")]
        [Description("Expand or collapse button.")]
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnExpandCollapse();
            }
        }

        /// <summary>
        /// Header
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Header")]
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return lblHeader.Text;
            }
            set
            {
                lblHeader.Text = value;
            }
        }

        /// <summary>
        /// Font used for displays header text
        /// </summary>
        public override Font Font
        {
            get
            {
                return lblHeader.Font;
            }
            set
            {
                lblHeader.Font = value;
            }
        }


        /// <summary>
        /// Font used for displays header text
        /// </summary>
        public new Point Location
        {
            get => _location;

            set
            {
                _location = value;
                base.Location = _location;
            }
        }


        /// <summary>
        /// Foreground color used for displays header text
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return lblHeader.ForeColor;
            }
            set
            {
                lblHeader.ForeColor = value;
            }
        }


        /// <summary>
        /// Occurs when the button has expanded or collapsed
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Occurs when the button has expanded or collapsed.")]
        [Browsable(true)]
        public event EventHandler<ExpandCollapseEventArgs> ExpandCollapse;



        [Category("ExpandCollapsePanel")]
        [Description("Ocurre cuando finaliza la expancion")]
        [Browsable(true)]
        public event EventHandler<ExpandCollapseEventArgs> ExpandFinally;


        public ExpandCollapseButton()
        {
            InitializeComponent();

            _padingLeftIcon = 5;

            #region initialize expanded/collapsed state bitmaps:
            InitButtonStyle(ExpandButtonStyle.Arrow);
            InitButtonSize(ExpandButtonSize.Normal);
            #endregion
            // initial state of panel - collapsed
            _isExpanded = false;
        }

        /// <summary>
        /// Occurs when the button has expanded or collapsed
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Padding left logo.")]
        [Browsable(true)]
        public int PaddingLeftIcon { get => _padingLeftIcon; set => _padingLeftIcon = value; }

        #region ExpandButtonStyles
        /// <summary>
        /// Visual styles of the expand-collapse button.
        /// </summary>
        public enum ExpandButtonStyle
        {
            Classic,
            Circle,
            MagicArrow,
            Triangle,
            FatArrow,
            Hambur,
            Arrow,
        }
        private ExpandButtonStyle _expandButtonStyle = ExpandButtonStyle.Circle;

        /// <summary>
        /// Visual style of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Visual style of the expand-collapse button.")]
        [Browsable(true)]
        public ExpandButtonStyle ButtonStyle
        {
            get { return _expandButtonStyle; }
            set
            {
                if (_expandButtonStyle != value)
                {
                    InitButtonStyle(value);
                }
            }
        }

        private void InitButtonStyle(ExpandButtonStyle style)
        {
            _expandButtonStyle = style;

            switch (_expandButtonStyle)
            {

                case ExpandButtonStyle.MagicArrow:
                    var bmp = Properties.Resources.Upload;
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    logoPicture.Image = bmp;
                    break;
                case ExpandButtonStyle.Circle:
                    bmp = Properties.Resources.icon_expand;
                    logoPicture.Image = bmp;
                    break;
                case ExpandButtonStyle.Triangle:
                    logoPicture.Image = Properties.Resources._1downarrow1;
                    break;
                case ExpandButtonStyle.FatArrow:
                    bmp = Properties.Resources.up_256;
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    logoPicture.Image = bmp;
                    break;
                case ExpandButtonStyle.Classic:
                    bmp = Properties.Resources.icon_struct_hide_collapsed;
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    logoPicture.Image = bmp;
                    break;
                case ExpandButtonStyle.Hambur:
                    bmp = Properties.Resources.hamburger_22_Down_white;
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    logoPicture.Image = bmp;
                    break;
                case ExpandButtonStyle.Arrow:
                    bmp = Properties.Resources.Arrow3;
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    logoPicture.Image = bmp;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("style");
            }

            // collapsed bitmap:
            _collapsed = logoPicture.Image;

            // expanded bitmap is rotated collapsed bitmap:
            _expanded = MakeGrayscale3(logoPicture.Image);
            _expanded.RotateFlip(RotateFlipType.Rotate180FlipNone);


            // finally set appropriate bitmap for current state
            logoPicture.Image = _isExpanded ? _expanded : _collapsed;
        }
        #endregion ExpandButtonStyles

        #region ExpandButtonSizes
        /// <summary>
        /// Size presets of the expand-collapse button.
        /// </summary>
        public enum ExpandButtonSize
        {
            Small,
            Normal,
            Large
        }
        private ExpandButtonSize _expandButtonSize = ExpandButtonSize.Normal;

        /// <summary>
        /// Size preset of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Size preset of the expand-collapse button.")]
        [Browsable(true)]
        public ExpandButtonSize ButtonSize
        {
            get { return _expandButtonSize; }
            set
            {
                if (_expandButtonSize != value)
                {
                    InitButtonSize(value);
                }
            }
        }

        public override Color BackColor { get => _BackColor; set { base.BackColor = _BackColor; } }
        /// <summary>
        /// Header title location
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Location title")]
        [Browsable(true)]
        public Point HeaderTitleLocation
        {
            get => lblHeader.Location;
            set
            {
                lblHeader.Location = value;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            this._imagePicture.Visible = this._imagePicture.Image != null;

            SetAlignmentIcon(_iconAlinement);
            base.OnPaint(e);
        }
        /// <summary>
        /// Resize and arrange child controls according to ButtonSize preset
        /// </summary>
        /// <param name="size">ButtonSize preset</param>
        private void InitButtonSize(ExpandButtonSize size)
        {
            _expandButtonSize = size;

            switch (_expandButtonSize)
            {
                case ExpandButtonSize.Small:
                    //logoPicture.Location = new Point(0, 3);
                    logoPicture.Size = new Size(16, 16);
                    // lblLine.Location = new Point(20, 18);
                    // lblHeader.Location = new Point(20, 1);
                    break;
                case ExpandButtonSize.Normal:
                    // logoPicture.Location = new Point(0, 3);
                    logoPicture.Size = new Size(24, 24);
                    // lblLine.Location = new Point(30, 22);
                    // lblHeader.Location = new Point(30, 3);
                    break;
                case ExpandButtonSize.Large:
                    // logoPicture.Location = new Point(0, 3);
                    logoPicture.Size = new Size(35, 35);
                    // lblLine.Location = new Point(41, 28);
                    //lblHeader.Location = new Point(41, 3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // after resize all child controls - do resize for entire ExpandCollapseButton control:
            Height = logoPicture.Location.Y + logoPicture.Height + 10;
        }
        #endregion ExpandButtonSizes

        #region ExpandButtonAlinements

        public enum ExpandIconAlignment
        {
            Left,
            Right
        }

        private ExpandIconAlignment _iconAlinement = ExpandIconAlignment.Right;
        /// <summary>
        /// Visual style of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Alinement icon in button")]
        [Browsable(true)]
        public ExpandIconAlignment IconAlignment
        {
            get { return _iconAlinement; }
            set
            {
                if (_iconAlinement != value)
                {
                    _iconAlinement = value;
                    SetAlignmentIcon(_iconAlinement);
                }
            }
        }

        private void SetAlignmentIcon(ExpandIconAlignment alignment)
        {

            switch (alignment)
            {

                case ExpandIconAlignment.Left:
                    this.logoPicture.Location = new Point(_padingLeftIcon, this.logoPicture.Location.Y);
                    break;

                case ExpandIconAlignment.Right:
                    this.logoPicture.Location = new Point(this.Width - (this.logoPicture.Width + 10), this.logoPicture.Location.Y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Alignment");
            }
        }
        #endregion

        /// <summary>
        /// Handle clicks from PictureBox and Header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {
            this.BackColor = _BackColorHover;
        }



        /// <summary>
        /// Handle clicks from PictureBox and Header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {
            this.BackColor = _BackColor;
        }


        /// <summary>
        /// Handle clicks from PictureBox and Header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseDown(object sender, EventArgs e)
        {
            // just invert current state
            IsExpanded = !IsExpanded;
        }

        /// <summary>
        /// Handle state changing
        /// </summary>
        protected virtual void OnExpandCollapse()
        {
            // set appropriate bitmap
            logoPicture.Image = _isExpanded ? _expanded : _collapsed;

            //lblHeader.ForeColor = _isExpanded ? Color.DarkGray : Color.SteelBlue;

            // and fire the event:
            EventHandler<ExpandCollapseEventArgs> handler = ExpandCollapse;
            if (handler != null)
                handler(this, new ExpandCollapseEventArgs(_isExpanded));

            EventHandler<ExpandCollapseEventArgs> handfinally = ExpandFinally;
            if (ExpandFinally != null)
                handler(this, new ExpandCollapseEventArgs(_isExpanded));
        }

        /// <summary>
        /// Utillity method for createing a grayscale copy of image
        /// </summary>
        /// <param name="original">original image</param>
        /// <returns>grayscale copy of image</returns>
        public static Bitmap MakeGrayscale3(Image original)
        {
            // create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            // get a graphics object from the new image
            using (var g = Graphics.FromImage(newBitmap))
            {

                // create the grayscale ColorMatrix
                var colorMatrix = new ColorMatrix(
                    new float[][]
                        {
                            new float[] {.3f, .3f, .3f, 0, 0},
                            new float[] {.59f, .59f, .59f, 0, 0},
                            new float[] {.11f, .11f, .11f, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        });

                // create some image attributes
                var attributes = new ImageAttributes();

                // set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                // draw the original image on the new image
                // using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

                // dispose the Graphics object
                g.Dispose();
            }

            return newBitmap;
        }
    }
}
