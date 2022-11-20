using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMControls.ExpandCollapsePanel
{

    [Designer(typeof(System.Windows.Forms.Design.ScrollableControlDesigner))]
    public class TabPageAc : System.Windows.Forms.TabPage
    {
        #region Instance Members

        internal bool preventClosing = false;
        private bool _isClosable = true;
        private string _text = null;
        private Image _image;
        private Point _imageLocation = new Point(15, 5);
        #endregion

        #region Constructor

        public TabPageAc()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ContainerControl, true);

            this.Font = new Font("Arial", 10f);
            this.BackColor = Color.White;
        }

        public TabPageAc(string text)
            : this()
        {
            this.Text = text;
        }

        #endregion

        #region Destructor

        ~TabPageAc()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Property

        /// <summary>
        /// Image in tab control tab
        /// </summary>
        [Description("Image in tab control tab")]
        [DefaultValue(null)]
        [Browsable(true)]
        public Image Image
        {
            get { return _image; }
            set
            {
                if (!value.Equals(_image))
                {
                    _image = value;

                    //if (this.Parent != null)
                    //{
                    //    this.Parent.Invalidate();
                    //    this.Parent.Update();
                    //}
                }
            }
        }
        /// <summary>
        /// Image location in tab control 
        /// </summary>
        [Description("Image location in tab control ")]
        [Browsable(true)]
        public Point ImageLocation
        {
            get { return _imageLocation; }
            set
            {
                if (!value.Equals(_imageLocation))
                {
                    _imageLocation = value;

                    if (this.Parent != null)
                    {
                        this.Parent.Invalidate();
                        this.Parent.Update();
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the tab page is closable or not.
        /// </summary>
        [Description("Determines whether the tab page is closable or not")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool IsClosable
        {
            get { return _isClosable; }
            set
            {
                if (!value.Equals(_isClosable))
                {
                    _isClosable = value;

                    if (this.Parent != null)
                    {
                        this.Parent.Invalidate();
                        this.Parent.Update();
                    }
                }
            }
        }

        public new string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value != null && !value.Equals(_text))
                {
                    base.Text = value;
                    base.Text = base.Text.Trim();
                    base.Text = base.Text.PadRight(base.Text.Length + 2);
                    _text = base.Text.TrimEnd();
                }
            }
        }

        [DefaultValue(true)]
        [Browsable(true)]
        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
            }
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return this.Text;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            if (this.Parent != null)
                this.Parent.Invalidate();
        }

        #endregion
    }
}
