using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMControls.ExpandCollapsePanel
{
    /// <summary>
    /// The ExpandCollapsePanel control displays a header that has a collapsible window that displays content.
    /// </summary>
    [DefaultEvent("ExpandCollapse")]
    [Designer(typeof(ExpandCollapsePanelDesigner))]
    public partial class AccordionCotrol : UserControl
    {


        /// <summary>
        /// Last stored size of panel's parent control
        /// <remarks>used for handling panel's Anchor property sets to Bottom when panel collapsed
        /// in OnSizeChanged method</remarks>
        /// </summary>
        private Size _previousParentSize = Size.Empty;

        /// <summary>
        /// Enable pretty simple animation of panel on expanding or collapsing
        /// </summary>
        private bool _useAnimation = true;
        /// <summary>
        /// Height of panel in expanded state
        /// </summary>
        private int _expandedWidth;

        /// <summary>
        /// Height of panel in collapsed state
        /// </summary>
        private readonly int _collapsedHeight;

        /// <summary>
        /// is expand panel
        /// </summary>
        private bool _isExpanded;
        /// <summary>
        /// Occurs when the panel has expanded or collapsed
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("Occurs when the panel has expanded or collapsed.")]
        [Browsable(true)]
        public event EventHandler<ExpandCollapseEventArgs> ExpandCollapse;


        [Category("ExpandCollapsePanel")]
        [Description("Ocurre cuando finaliza la expancion")]
        [Browsable(true)]
        public event EventHandler<ExpandCollapseEventArgs> ExpandFinally;


        private ExpandCollapseEventArgs args;

        public AccordionCotrol()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            InitializeComponent();

            _collapsedHeight = 50;
            var hi = Width;
            _btnExpandCollapse.ExpandCollapse += BtnExpandCollapseExpandCollapse;

            _expandedWidth = this.Width;
        }


        /// <summary>
        /// Enable pretty simple animation of panel on expanding or collapsing
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("Enable pretty simple animation of panel on expanding or collapsing.")]
        [Browsable(true)]
        public bool UseAnimation
        {
            get { return _useAnimation; }
            set { _useAnimation = value; }
        }


        /// <summary>
        /// Size preset of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("Set or get the image in button")]
        [Browsable(true)]
        public Image ButtonImage
        {
            get { return _btnExpandCollapse.ImagePicture; }
            set { _btnExpandCollapse.ImagePicture = value; }
        }

        /// <summary>
        /// Size preset of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("This is location the image in button")]
        [Browsable(true)]
        public Point ButtonImageLocation
        {
            get { return _btnExpandCollapse.ImageLocation; }
            set { _btnExpandCollapse.ImageLocation = value; }
        }


        /// <summary>
        /// Size preset of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("This is anchor the button")]
        [Browsable(true)]
        public AnchorStyles ButtonAnchor
        {
            get => _btnExpandCollapse.Anchor;
            set => _btnExpandCollapse.Anchor = value;
        }

        /// <summary>
        /// Size preset of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("Visible icon button")]
        [Browsable(true)]
        public bool VisibleIconButton
        {
            get { return _btnExpandCollapse.VisibleIcon; }
            set { _btnExpandCollapse.VisibleIcon = value; }
        }


        /// <summary>
        /// This is class button
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("This is backcolo from button")]
        [Browsable(true)]
        public Color ButtonBackColor
        {
            get { return _btnExpandCollapse.BackColorNormal; }
            set
            {
                _btnExpandCollapse.BackColorNormal = value;
            }
        }

        /// <summary>
        /// This is class button
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("ButtonB ack color Hover")]
        [Browsable(true)]
        public Color ButtonBackColorHover
        {
            get { return _btnExpandCollapse.BackColorHover; }
            set
            {
                _btnExpandCollapse.BackColorHover = value;
            }
        }



        /// <summary>
        /// This is class button
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("Header Height")]
        [Browsable(true)]
        public int HeaderHeight
        {
            get => this.panelHeader.Height;
            set => this.panelHeader.Height = value;

        }
        /// <summary>
        /// This is class button
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("Header Height")]
        [Browsable(true)]
        public PictureBox ImageControl
        {
            get => this._pictureBox;
            set => this._pictureBox = value;

        }

        //_pictureBox

        /// <summary>
        /// Handle button expand-collapse state changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExpandCollapseExpandCollapse(object sender, ExpandCollapseEventArgs e)
        {
            args = e;

            if (e.IsExpanded) // if button is expanded now
            {
                Expand(); // expand the panel
            }
            else
            {
                Collapse(); // collapse the panel
            }

            // Retrieve expand-collapse state changed event for panel
            EventHandler<ExpandCollapseEventArgs> handler = ExpandCollapse;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// Expand panel content
        /// </summary>
        protected virtual void Expand()
        {
            // if animation enabled
            if (UseAnimation)
            {
                // set internal state for Expanding
                _internalPanelState = InternalPanelState.Expanding;
                // start animation now..
                StartAnimation();
                Invalidate();
            }
            else // no animation, just expand immediately
            {
                // set internal state to Normal
                _internalPanelState = InternalPanelState.Normal;
                // resize panel
                Size = new Size(Size.Width, _expandedWidth);

            }
        }

        /// <summary>
        /// Collapse panel content
        /// </summary>
        protected virtual void Collapse()
        {
            // if panel is completely expanded (animation on expanding is ended or no animation at all) 
            // *we don't want store half-expanded panel height
            if (_internalPanelState == InternalPanelState.Normal)
            {
                // store current panel height in expanded state
                _expandedWidth = Size.Width;
            }

            // if animation enabled
            if (UseAnimation)
            {
                // set internal state for Collapsing
                _internalPanelState = InternalPanelState.Collapsing;
                // start animation now..
                StartAnimation();
            }
            else // no animation, just collapse immediately
            {
                // set internal state to Normal
                _internalPanelState = InternalPanelState.Normal;
                // resize panel
                Size = new Size(Size.Width, _collapsedHeight);
            }
        }

        // degree to adjust the height of the panel when animating
        private int _animationHeightAdjustment = 0;

        /// </summary>
        private void StartAnimation()
        {
            _animationHeightAdjustment = 1;
            animationTimer.Interval = 50;
            animationTimer.Enabled = true;
            Invalidate();
        }

        private void animationTimer_Tick(object sender, System.EventArgs e)
        {
            //	---------------------------------------------------------------
            //	Gradually reduce the interval between timer events so that the
            //	animation begins slowly and eventually accelerates to completion
            //	---------------------------------------------------------------
            if (animationTimer.Interval > 10)
            {
                animationTimer.Interval -= 10;
            }
            else
            {
                _animationHeightAdjustment += 2;
            }


            switch (_internalPanelState)
            {
                case InternalPanelState.Expanding:

                    if ((Width + _animationHeightAdjustment) < _expandedWidth)
                    {
                        Width += _animationHeightAdjustment;
                    }
                    else
                    {

                        Width = _expandedWidth;
                        _internalPanelState = InternalPanelState.Normal;
                    }

                    break;

                case InternalPanelState.Collapsing:
                    // still something to collapse
                    if ((Width - _animationHeightAdjustment) > _collapsedHeight)
                    {
                        Width -= _animationHeightAdjustment;

                    }
                    else
                    {

                        Width = _collapsedHeight;
                        _internalPanelState = InternalPanelState.Normal;
                    }
                    break;

                default:
                    return;
            }

            // set the opacity for all the controls on the XPPanel
            //SetControlsOpacity(currOpacity);

            // are we done?
            if (_internalPanelState == InternalPanelState.Normal)
            {
                animationTimer.Enabled = false;
                EventHandler<ExpandCollapseEventArgs> handler = ExpandFinally;
                if (handler != null)
                    handler(this, args);
            }

            Invalidate();
        }



        /// <summary>
        /// Set flag for expand or collapse panel content
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("Expand or collapse panel content. " +
                     "\r\nAttention, for correct work with resizing child controls," +
                     " please set IsExpanded to \"false\" in code (for example in your Form class constructor after InitializeComponent method) and not in Forms Designer!")]
        [Browsable(true)]
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                    _isExpanded = value;
            }
        }


        /// <summary>
        /// Handle panel resize event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // we always manually scale expand-collapse button for filling the horizontal space of panel:
            panelHeader.Size = new Size(ClientSize.Width - panelHeader.Margin.Left - panelHeader.Margin.Right, panelHeader.Height);


            // ignore height changing from animation timer
            if (_internalPanelState != InternalPanelState.Normal)
                return;

            #region Handling panel's Anchor property sets to Bottom when panel collapsed

            if (!IsExpanded // if panel collapsed
                && ((Anchor & AnchorStyles.Bottom) != 0) //and panel's Anchor property sets to Bottom
                && Size.Height != _collapsedHeight // and panel height is changed (it could happens only if parent control just has resized)
                && Parent != null) // and panel has the parent control
            {
                // main, calculate the parent control resize diff and add it to expandedHeight value:
                _expandedWidth += Parent.Width - _previousParentSize.Width;

                // reset resized height (by base.OnSizeChanged anchor.Bottom handling) to collapsedHeight value:
                Size = new Size(Size.Width, _collapsedHeight);
            }

            // store previous size of parent control (however we need only height)
            if (Parent != null)
                _previousParentSize = Parent.Size;
            #endregion
        }


        private void AccordionCotrol_Load(object sender, EventArgs e)
        {

        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            panelHeader.SendToBack();
            base.OnControlAdded(e);
        }

        /// <summary>
        /// Internal state of panel used for checking that panel is animating now
        /// </summary>
        private InternalPanelState _internalPanelState;

        /// <summary>
        /// Internal state of panel
        /// </summary>
        private enum InternalPanelState
        {
            /// <summary>
            /// No animation, completely expanded or collapsed
            /// </summary>
            Normal,
            /// <summary>
            /// Expanding animation
            /// </summary>
            Expanding,
            /// <summary>
            /// Collapsing animation
            /// </summary>
            Collapsing
        }

    }
}
