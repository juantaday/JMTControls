using JMControls.Enums;
using JMControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMControls.Controls
{
    public class TextBoxRounded : Control
    {
        int borderRadius = 12;
        private Pen pensBorder;
        private string placeholderText;
        private string baseText;
        private string _ToolTipButtonToString;
        private TextBox textBox1;

        GraphicsPath innerRect;
        Color fillColor;

        private bool _VisibleButton;
        private readonly Button _button;
        private ToolTip _ToolTip1;
        private BorderStyle borderStyle;
        private int borderThickness;
        private Color borderColorHover;
        private Color borderColorActive;
        private Color borderColorIdle;
        private Color borderColorDisable;
        private Color placeholderColor;
        private Color _foreColor;
        private bool isPasswordChar;
        private bool isPlaceholder;
        private bool isFocused = false;

        MouseState state;
        MouseState statetText;

        public TextBoxRounded()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            state = MouseState.Leave;
            statetText = MouseState.Leave;

            pensBorder = new Pen(Color.Gray);
            borderStyle = BorderStyle.FixedSingle;
            borderThickness = 1;
            borderColorIdle = Color.Teal;
            placeholderColor = Color.DarkGray;
            fillColor = Color.White;
            _VisibleButton = true;

            textBox1 = new TextBox {
                Parent = this,
                Location = new Point(2, 0),
                BorderStyle = BorderStyle.None,
                 TextAlign = HorizontalAlignment.Left,
                BackColor = fillColor,
               };



            Controls.Add(textBox1);

           

            FillColor = Color.Transparent;
            ForeColor = Color.FromArgb(60,60,60) ;
            fillColor = Color.White;
           
            this.BorderColorDisable = Color.FromArgb(160, 160, 160);
            Text = null;
            Font = new Font("Comic Sans MS", 11);
            Size = new Size(135, 33);
            DoubleBuffered = true;
           


            textBox1.TextChanged += box_TextChanged;
            textBox1.MouseDoubleClick += box_MouseDoubleClick;
            textBox1.Enter += Box_Enter;
            textBox1.Leave += Box_Leave;
            textBox1.MouseDown += Box_MouseDown;
            textBox1.MouseMove += Box_MouseMove; ;

            baseText = "";
            textBox1.Text = baseText;

            
            this.TabStop = false;


            _ToolTip1 = new ToolTip();

            _button = new Button
            {
                Cursor = Cursors.Default,
                TabStop = false,
                Image = Properties.Resources.zoom_Grin_24,
                ImageAlign = ContentAlignment.MiddleRight,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = 32};

            _button.Parent = this;
            _button.FlatAppearance.BorderSize = 0;
            _button.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            _button.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 200, 200);
            _button.BackColor = Color.Transparent;

            this.Controls.Add(_button);
            this.Padding = new Padding(1, 1, 1, 1);
            _button.GotFocus += _button_GotFocus; ;
            _button.BringToFront();
        }

        private void _button_GotFocus(object sender, EventArgs e)
        {
        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [Category("Action")]
        public event EventHandler ButtonClick
        {
            add { _button.Click += value; }
            remove { _button.Click -= value; }
        }

        [Category("Action"), Description("Se genera cuando presiona una tecla en el texto")]
        public event KeyPressEventHandler KeyPressTextBox
        {
            add { textBox1.KeyPress += value; }
            remove { textBox1.KeyPress -= value; }
        }


        private Image _buttonImage;

        [Category("Appearance"), Description("Imagen del botón")]
        public Image ButtonImage
        {
            get
            {
                return _buttonImage;
            }
            set
            {
                _buttonImage = value;
                if (_buttonImage == null)
                    _button.Image = Properties.Resources.ellipsis1;
                else
                    _button.Image = _buttonImage;
                _button.ImageAlign = ContentAlignment.MiddleRight;
            }
        }


        [Category("Appearance"), Description("Alint tex")]
        public HorizontalAlignment TextAlign
        {
            get
            { return textBox1.TextAlign; }

            set { textBox1.TextAlign = value; }
        }

        public  void SetFocus()
        {
            textBox1.Focus();
        }


        public bool VisibleButton
        {
            get
            {
                return _VisibleButton;
            }

            set
            {
                _VisibleButton = value;
                _button.Visible = _VisibleButton;
            }
        }

        public Button GetButton
        {
            get
            {
                return _button;
            }
        }

        public virtual System.Windows.Forms.DockStyle DockButton
        {
            get
            {
                return _button.Dock;
            }

            set
            {
                _button.Dock = value;
            }
        }


        public int MaxLenght { get =>textBox1.MaxLength; set => textBox1.MaxLength= value;}

        public int WidthButton
        {
            get
            {
                return _button.Width;
            }

            set
            {
                _button.Width = value;
            }
        }

        public Button Button
        {
            get
            {
                return _button;
            }

        }

        protected override void OnParentFontChanged(EventArgs e)
        {
            this.textBox1.Font = base.Font;
            this._button.Font = base.Font;
            base.OnParentFontChanged(e);
        }


        [Category("Action"), Description("Se genera cuado hace click con el maus..")]
        public new event EventHandler Click
        {
            add { textBox1.Click += value; }
            remove { textBox1.Click -= value; }
        }


        [Category("Action"), Description("Se genera cuando cambia el texto..")]
        public new event EventHandler TextChanged
        {
            add { textBox1.TextChanged += value; }
            remove { textBox1.TextChanged -= value; }
        }


        void box_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
            textBox1.SelectAll();
        }

        void box_TextChanged(object sender, EventArgs e)
        {
          
            if (string.IsNullOrEmpty(textBox1.Text) || 
                isPlaceholder && statetText !=MouseState.Down)
            {
                textBox1.ForeColor = this.placeholderColor;
          
            }
            else {
                textBox1.ForeColor = this.ForeColor;
            }

            baseText = textBox1.Text;

            base.OnTextChanged(e);
        }

        public  void SelectAll()
        {
            textBox1.SelectAll();
        }


        public void SelectecText(int star, int length)
        {
            try
            {
                this.textBox1.Select(star, length);
            }
            catch (Exception)
            {

            }
        }
        public void Focus_()
        {
            this.textBox1.Focus();
        }





        public int SelectionLength_
        {
            get { return textBox1.SelectionLength; }

        }



        void box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                textBox1.SelectionStart = 0;
                textBox1.SelectionLength = Text.Length;
            }
        }

        /// <summary>
        /// Evento que se genera cuando el control es activo y preciona una tecla
        /// </summary>
        /// <remarks>este eventa en controlado porTextBoxRounded </remarks>
        [Category("Action"),
         Description("Evento que se genera cuando el control es activo y preciona una tecla."),
         DefaultValue(false)]
        public event KeyEventHandler KeyDownInTextBox
        {
            add { textBox1.KeyDown += value; }
            remove { textBox1.KeyDown -= value; }
        }

        /// <summary>
        /// Evento que se genera cuando el control es activo y preciona una tecla
        /// </summary>
        /// <remarks>este eventa en controlado porTextBoxRounded </remarks>
        [Category("Action"),
         Description("Evento que se genera cuando el control es activo y preciona y suelta una tecla."),
         DefaultValue(false)]
        public event KeyEventHandler KeyKeyUpInTextBox
        {
            add { textBox1.KeyUp += value; }
            remove { textBox1.KeyUp -= value; }
        }


        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            float single = (float)radius * 2f;
            graphicsPath.StartFigure();
            graphicsPath.AddArc((float)rect.X, (float)rect.Y, single, single, 180f, 90f);
            graphicsPath.AddArc((float)rect.Right - single, (float)rect.Y, single, single, 270f, 90f);
            graphicsPath.AddArc((float)rect.Right - single, (float)rect.Bottom - single, single, single, 0f, 90f);
            graphicsPath.AddArc((float)rect.X, (float)rect.Bottom - single, single, single, 90f, 90f);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (string.IsNullOrEmpty(baseText))
            {
                textBox1.ForeColor = this.PlaceHolderColor;
                textBox1.Text=  this.PlaceHolderText;
            }
            else
            {
                textBox1.ForeColor = this.ForeColor;
                textBox1.Text = baseText;
            }
      
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            textBox1.Font = Font;
            Invalidate();
        }
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            textBox1.ForeColor = ForeColor;
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            innerRect = new RoundedRectangleF(this.Width - 1, this.Height - 1, BorderRadius, 1f, 1f).Path;


            if (textBox1.Height >= Height - (int)(BorderThickness * 2.8))
                Height = textBox1.Height + (int)(BorderThickness * 2.8);

            int borderDelimi =  9 + (int)(BorderThickness *1);
            if (BorderThickness == 0) borderDelimi = 10;


            textBox1.Width = (this.Width - 2) -( borderDelimi + (int)(BorderRadius*0.3));


            var fonWith = textBox1.Font.Size;
            int locationy = BorderRadius > 0 ? (int)(BorderRadius * .2) + (int)(fonWith * 0.25) + (int)(BorderThickness * 0.9) : (int)(BorderThickness * 2.5);
            if (BorderThickness == 0) locationy += 5;
            
            textBox1.Location = new Point(locationy+1, Height / 2 - textBox1.Font.Height / 2);



            
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;


            //rellena el color de fondo
            using (SolidBrush brush = new SolidBrush(FillColor))
                e.Graphics.FillPath(brush, innerRect);


            if (this.Enabled)
            {
                if (state == MouseState.Leave)
                {
                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorIdle, BorderThickness))
                        {
                            if (BorderRadius == 0)
                            {
                                Rectangle path = new Rectangle(new Point(borderThickness, borderThickness), new Size { Height = (this.Height-BorderThickness * 2), Width = this.Width -( BorderThickness*2) });
                                e.Graphics.DrawRectangle(pen, path);

                            }
                            else
                            {
                                using (GraphicsPath path = new RoundedRectangleF(this.Width - 1, this.Height - 1, BorderRadius, 1, 1).Path)
                                {
                                    e.Graphics.DrawPath(pen, path);

                                }
                            }
                        }
     
                }

                else if (state == MouseState.Enter)
                {
                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorHover, BorderThickness))
                        {
                            if (BorderRadius == 0)
                            {
                                Rectangle path = new Rectangle(new Point(borderThickness, borderThickness), new Size { Height = (this.Height - BorderThickness * 2), Width = this.Width - (BorderThickness * 2) });
                                e.Graphics.DrawRectangle(pen, path);

                            }
                            else
                            {
                                using (GraphicsPath path = new RoundedRectangleF(this.Width - 1, this.Height - 1, BorderRadius, 1, 1).Path)
                                {
                                    e.Graphics.DrawPath(pen, path);

                                }
                            }
                        }
                }


                else if (state == MouseState.Down)
                {

                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorActive, BorderThickness))
                        {
                            if (BorderRadius == 0)
                            {
                                Rectangle path = new Rectangle(new Point(borderThickness, borderThickness), new Size { Height = (this.Height - BorderThickness * 2), Width = this.Width - (BorderThickness * 2) });
                                e.Graphics.DrawRectangle(pen, path);
                            }
                            else
                            {
                                using (GraphicsPath path = new RoundedRectangleF(this.Width - 1, this.Height - 1, BorderRadius, 1, 1).Path)
                                {
                                    e.Graphics.DrawPath(pen, path);
                                }
                            }
                        }

                }

            }
            else
            {

                if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                    using (Pen pen = new Pen(BorderColorDisable, BorderThickness))
                    using (GraphicsPath path = new RoundedRectangleF(Width - (BorderRadius > 0 ? 0 : 1), Height - (BorderRadius > 0 ? 0 : 1), BorderRadius).Path)
                        e.Graphics.DrawPath(pen, path);
            }

            Transparenter.MakeTransparent(this, e.Graphics);
            base.OnPaint(e);
        }


        #region Methods

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (base.DesignMode)
            {
                this.UpdateControlHeight();
            }
        }

        public string SelectedText {
            get
            {
                return textBox1.SelectedText;
            }
        }


        public override string Text { get => this.Texts; set => this.Texts = value; }

        public  string  Texts {
            get
            {
                  return baseText;
            }
            set
            {
                textBox1.Text = value;
                baseText = value;
                SetPlaceholder();
            }
        }


        private void Box_Enter(object sender, EventArgs e)
        {
            statetText = MouseState.Down;
            state = MouseState.Down;

            isFocused = true;
            this.Invalidate();
            RemovePlaceholder();
        }
        private void Box_Leave(object sender, EventArgs e)
        {
            statetText = MouseState.Leave;
            state = MouseState.Leave;
            isFocused = false;
            Invalidate();
            SetPlaceholder();
           
        }

        private void Box_MouseMove(object sender, MouseEventArgs e)
        {
            if (statetText == MouseState.Down)
            {
                state = MouseState.Down;

            }
            else {
                state = MouseState.Enter;
                base.OnMouseEnter(e);
                Invalidate();
            }

        }

        public new  string  Name { get { return base.Name; }
            set {
                base.Name = value;
                textBox1.Name = value;
            } 
        }


        public Color PlaceHolderColor { get => placeholderColor; set => placeholderColor = value; }


        public string PlaceHolderText { get=>placeholderText;
            set {

                placeholderText = value;
               // textBox1.Text = "";
                SetPlaceholder();
            }
        }


        private void Box_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        
        protected override void OnMouseEnter(EventArgs e)
        {
            if (textBox1.Focused) {
                statetText = MouseState.Down;
                state = MouseState.Down;
                base.OnMouseEnter(e);
                Invalidate();
            }
            else if (statetText == MouseState.Down)
            {
                state = MouseState.Down;
                base.OnMouseEnter(e);
                Invalidate();
            }
            else {
                state = MouseState.Enter;
                base.OnMouseEnter(e);
                Invalidate();
            }
           
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (statetText == MouseState.Leave)
            {
                state = MouseState.Leave;
                base.OnMouseEnter(e);
                Invalidate();
            }
            
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


        #region Properties


        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                textBox1.BackColor = value;
            }
        }

        public Color BorderColorActive
        {
            get { return borderColorActive; }
            set
            {
                borderColorActive = value;
                Invalidate();
            }

        }

        public Color BorderColorDisable
        {
            get { return borderColorDisable; }
            set
            {
                borderColorDisable = value;
                Invalidate();
            }

        }
       
        public Color BorderColorHover
        {
            get { return borderColorHover; }
            set
            {
                borderColorHover = value;
                Invalidate();
            }

        }

        public BorderStyle BorderStyle { get=>borderStyle; set {
                borderStyle = value;
                Invalidate();
            }
        }

        public int BorderThickness { get=> borderThickness; set {
                borderThickness = value;
                Invalidate();
            }
        }


        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                if (value >= 0)
                {
                    borderRadius = value;
                    if (borderRadius == 0)
                        this.Padding = new Padding(1);
                    else if (borderRadius < 5)
                        this.Padding = new Padding(3, 2, 3, 2);
                    else if (borderRadius < 9)
                        this.Padding = new Padding(5, 3, 5, 3);
                    else if (borderRadius < 12)
                        this.Padding = new Padding(6, 4, 6, 4);
                    else
                        this.Padding = new Padding(9, 5, 7, 5);

                    UpdateControlHeight();
                    this.Invalidate();//Redraw control
                }
            }
        }


        private void UpdateControlHeight()
        {

            if (!this.textBox1.Multiline)
            {
                Size size = TextRenderer.MeasureText("Text", this.Font);
                int height = size.Height + 1;
                this.textBox1.Multiline = true;
                this.textBox1.MinimumSize = new Size(0, height);
                this.textBox1.Multiline = false;
                int num = this.textBox1.Height;
                Padding padding = base.Padding;
                int top = num + padding.Top;
                padding = base.Padding;
                base.Height = top + padding.Bottom;
            }
        }

        public Color BorderColorIdle
        {
            get { return borderColorIdle; }

            set
            {
                borderColorIdle = value;
                Invalidate();
            }
        }
        public Color FillColor
        {
            get => fillColor;
            
            set
            {
                fillColor = value;
                if (value!=Color.Transparent)
                {
                    this.textBox1.BackColor = value;
                }
             
                Invalidate();
            }
        }

        public bool PasswordChar
        {
            get { return isPasswordChar; }
            set
            {
                isPasswordChar = value;
                if (!isPlaceholder)
                    textBox1.UseSystemPasswordChar = value;
            }
        }

        public bool Multiline
        {
            get
            {
                return this.textBox1.Multiline;
            }
            set
            {
                this.textBox1.Multiline = value;
            }
        }

        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath figurePath;
            if (!this.Multiline)
            {
                figurePath = this.GetFigurePath(this.textBox1.ClientRectangle, this.BorderThickness * 2);
                this.textBox1.Region = new Region(figurePath);
            }
            else
            {
                figurePath = this.GetFigurePath(this.textBox1.ClientRectangle, this.borderRadius - this.BorderThickness);
                this.textBox1.Region = new Region(figurePath);
            }
            figurePath.Dispose();
        }

        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(placeholderText))
            {
                isPlaceholder = true;
                textBox1.Text = placeholderText;
                textBox1.ForeColor = placeholderColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = false;
            }
            else {
                textBox1.ForeColor = this._foreColor;
            }
        }

        private void RemovePlaceholder()
        {
            if (isPlaceholder && placeholderText != "" &&  this.baseText .Equals(placeholderText))
            {
                isPlaceholder = false;
                textBox1.Text = "";
                textBox1.ForeColor = this.ForeColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = true;
            }
        }


        public string ToolTipButton {

            get { return _ToolTipButtonToString; }

            set {
                if (value == null || string.IsNullOrEmpty(value))
                {
                    _ToolTipButtonToString = string.Empty;
                    _ToolTip1.SetToolTip(_button, _ToolTipButtonToString);

                }
                else {
                    _ToolTipButtonToString = value ;
                    _ToolTip1.SetToolTip(_button, _ToolTipButtonToString);
                }

            }

        }

        public bool ReadOnly
        {
            get
            {
                return textBox1.ReadOnly;
            }
            set
            {
                textBox1.ReadOnly = value;
            }
        }

        public override Color ForeColor
        { 
            get => _foreColor;
            set {
                this._foreColor = value;
                this.textBox1.ForeColor = value;
              }
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
    
}
