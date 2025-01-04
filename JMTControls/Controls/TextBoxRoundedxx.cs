using JMControls.Enums;
using JMControls.Helpers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace JMControls.Controls
{
    [DefaultEvent("TextChanged")]
    public class TextBoxRoundedxx : Control
    {
        int borderRadius = 8;
        private string oldText = string.Empty;
        private Pen pensBorder;
        private string placeholderText;
        private string baseText;
        private string _ToolTipButtonToString;
        private TextBox textBox1;

        GraphicsPath innerRect;
        private Color fillColor = Color.White;

        private bool _visibleButton;
        private readonly Button _button;
        private ToolTip _ToolTip1;
        private BorderStyle borderStyle;
        private int borderThickness = 1;
        private Color borderColorHover = Color.Orange;
        private Color borderColorActive = Color.Red;
        private Color borderColorIdle = Color.DeepPink;
        private Color borderColorDisable = Color.DimGray;
        private Color placeholderColor = Color.FromArgb(180,180,180);
        private Color _foreColor = Color.Black;
        private bool isPasswordChar;
        private bool isPlaceholder;
        private bool isFocused = false;

        MouseState state;
        MouseState statetText;

        private Image _buttonImage;
        private PictureBox _iconPictureLeft;
        private Image _iconLeft;
        private bool _autosize;
        private int decimalPosition = 2;
        private TypeDataEnum _typeData = TypeDataEnum.VarChar;
        private Padding _textBoxPadding = new Padding(15, 5, 5, 5);
        private Color _iconLeftBackColor = Color.Transparent;

        public TextBoxRoundedxx()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            state = MouseState.Leave;
            statetText = MouseState.Leave;

            pensBorder = new Pen(Color.Gray);
            borderStyle = BorderStyle.FixedSingle;

            _visibleButton = true;

            textBox1 = new TextBox
            {
                Parent = this,
                Location = new Point(2, 0),
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Left,
                BackColor = fillColor,
                ForeColor = _foreColor
            };


            _iconPictureLeft  = new PictureBox
            {
                Size = new Size(16, 16),
                BackColor = _iconLeftBackColor,
                Location = new Point(2, 2),
                Visible   = false,
            };

            Font = new Font("Comic Sans MS", 11);
            Size = new Size(135, 33);
            DoubleBuffered = true;

            textBox1.TextChanged += box_TextChanged;
            textBox1.MouseDoubleClick += box_MouseDoubleClick;
            textBox1.Enter += Box_Enter;
            textBox1.Leave += Box_Leave;
            textBox1.MouseDown += Box_MouseDown;
            textBox1.MouseMove += Box_MouseMove;
            textBox1.KeyPress += TextBox1_KeyPress;

            _ToolTip1 = new System.Windows.Forms.ToolTip();

            _button = new Button
            {
                Cursor = Cursors.Default,
                TabStop = false,
                Image = Properties.Resources.zoom_Grin_24,
                ImageAlign = ContentAlignment.MiddleRight,
                FlatStyle = FlatStyle.Flat,
                Width = 32,
                BackColor = Color.Transparent,
                FlatAppearance =
                    {
                        BorderSize = 0,
                        MouseOverBackColor = Color.FromArgb(224, 224, 224),
                        MouseDownBackColor = Color.FromArgb(200, 200, 200)
                    }
            };

            _button.Parent = this;
            _button.FlatAppearance.BorderSize = 0;
            _button.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            _button.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 200, 200);
            _button.BackColor = Color.Transparent;
            _button.Image  = Properties.Resources.zoom_Grin_24;

            baseText = string.Empty;
            textBox1.Text = baseText;

            this.Text = baseText;
            this.TabStop = false;
            this.BorderColorDisable = Color.FromArgb(160, 160, 160);
            this.Controls.Add(textBox1);
            this.Controls.Add(_button);
            this.Controls.Add(_iconPictureLeft);
            base.Padding = new Padding(15, 5, 5, 5);
            _button.GotFocus += _button_GotFocus; ;
            _button.BringToFront();

            _button.Invalidate();
            _button.Visible = true;

            // Forzar un redibujado inicial
            Invalidate();
         

        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Equals("\b"))
                return;
            try
            {
                switch (_typeData)
                {
                    case TypeDataEnum.Numeric:
                        var resgN = Regex.Match(e.KeyChar.ToString(), @"[0-9]").Success;
                        if (!resgN)
                        {
                            e.Handled = true;
                            return;
                        }
                        break;
                    case TypeDataEnum.Decimal:

                        if (e.KeyChar.ToString().Equals(".") && textBox1.Text.Contains("."))
                        {
                            e.Handled = true;
                            return;
                        }
                        else if (e.KeyChar.ToString().Equals(".") && decimalPosition > 0)
                        {
                            return;
                        }
                        else if (e.KeyChar.ToString().Equals(".") && decimalPosition <= 0)
                        {
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            var resg = Regex.Match(e.KeyChar.ToString(), @"[0-9]").Success;
                            if (!resg)
                            {
                                e.Handled = true;
                                return;
                            }
                            if (decimalPosition > 0 && textBox1.Text.Contains("."))
                            {
                                var positionEdit = textBox1.SelectionStart;
                                var lenthSelect = textBox1.SelectionLength;
                                var indexOf = textBox1.Text.IndexOf(".") + 1;
                                var dcmLength = textBox1.Text.Substring(indexOf, (textBox1.Text.Length - indexOf)).Length;

                                if ((positionEdit > indexOf) && ((dcmLength - lenthSelect) >= decimalPosition))
                                {
                                    e.Handled = true;
                                    return;
                                }

                            }

                        }

                        break;
                    case TypeDataEnum.VarChar:
                        break;
                    case TypeDataEnum.DateTime:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
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
        public new event KeyPressEventHandler KeyPress
        {
            add { textBox1.KeyPress += value; }
            remove { textBox1.KeyPress -= value; }
        }




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
                    _button.Image = Properties.Resources.zoom_Grin_24;
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

        public bool VisibleButton
        {
            get => _visibleButton;

            set
            {
                _visibleButton = value;
                _button.Visible = _visibleButton;
                Invalidate();
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


        public int MaxLenght { get => textBox1.MaxLength; set => textBox1.MaxLength = value; }

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

        [Browsable(true)]
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
                isPlaceholder && statetText != MouseState.Down)
            {
                textBox1.ForeColor = this.placeholderColor;

            }
            else
            {
                textBox1.ForeColor = this.ForeColor;
            }

            baseText = textBox1.Text;

            base.OnTextChanged(e);
        }

        public void SelectAll()
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
        public new void Focus()
        {
            this.textBox1.Focus();
        }

        public int TextLength { get => this.textBox1.TextLength; }


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
         Description("Evento que se genera cuando el control es activo y preciona una tecla.")]
        public new  event KeyEventHandler KeyDown
        {
            add { textBox1.KeyDown += value; }
            remove { textBox1.KeyDown -= value; }
        }

        /// <summary>
        /// Evento que se genera cuando el control es activo y preciona una tecla
        /// </summary>
        /// <remarks>este eventa en controlado porTextBoxRounded </remarks>
        [Category("Action"),
         Description("Evento que se genera cuando el control es activo y preciona y suelta una tecla.")]
        public new  event KeyEventHandler KeyUp
        {
            add { textBox1.KeyUp += value; }
            remove { textBox1.KeyUp -= value; }
        }

        
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (string.IsNullOrEmpty(baseText))
            {
                textBox1.ForeColor = this.PlaceHolderColor;
                textBox1.Text = this.PlaceHolderText;
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
            textBox1.ForeColor = this.ForeColor; // Sincroniza el color
            this.Invalidate(); // Redibuja el control
        }


        public Button Button
        {
            get
            {
                return _button;
            }

        }

        public Padding TextBoxPadding {
            get => _textBoxPadding;
            set
            {
                if (_textBoxPadding != value) {
                    _textBoxPadding = value;
                    Invalidate();
                }
                 
            }   
        }

        public Image IconLeft
        {
            get => _iconLeft;
            set
            {
                if (_iconLeft != value)  // Solo actualiza si hay un cambio
                {
                    _iconLeft = value;
                    Invalidate();
                }
            }
        }

        public Color IconLeftBackColor
        {
            get => _iconLeftBackColor;
            set
            {
                if (_iconLeftBackColor != value)  // Solo actualiza si hay un cambio
                {
                    _iconLeftBackColor = value;
                    Invalidate();
                }
            }
        }

        public bool IconLeftVisible
        {  // Corregido el error tipográfico "IconLefthVisible"
            get => _iconPictureLeft.Visible;
            set
            {
                if (_iconPictureLeft.Visible != value)  // Solo actualiza si hay un cambio
                {
                    _iconPictureLeft.Visible = value;
                    Invalidate();
                }
            }
        }

        // IconLocationLeft
        public Point IconLocationLeft
        {
            get => _iconPictureLeft.Location;
            set
            {
                if (_iconPictureLeft.Location != value)  // Solo actualiza si hay un cambio
                {
                    _iconPictureLeft.Location = value;
                    Invalidate();
                }
            }
        }

        // IconSizeLeft
        public Size IconSizeLeft
        {
            get => _iconPictureLeft.Size;
            set
            {
                if (_iconPictureLeft.Size != value)  // Solo actualiza si hay un cambio
                {
                    _iconPictureLeft.Size = value;
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width <= 0 || Height <= 0) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath innerRect = GetRoundedRectanglePath(Width, Height, BorderRadius))
            using (Brush fillBrush = new SolidBrush(FillColor))
            {
                e.Graphics.FillPath(fillBrush, innerRect);
                if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                {
                    using (Pen borderPen = new Pen(GetBorderColor(), BorderThickness))
                    {
                        e.Graphics.DrawPath(borderPen, innerRect);
                    }
                }
            }

            ConfigureInternalElements();
            DrawIcon(e.Graphics);
            Transparenter.MakeTransparent(this, e.Graphics);
            base.OnPaint(e);
        }



        private GraphicsPath GetRoundedRectanglePath(int width, int height, int radius)
        {
            return new RoundedRectangleF(width - 1, height - 1, radius, 1f, 1f).Path;
        }

        private Color GetBorderColor()
        {
            switch (state)
            {
                case MouseState.Leave:
                    return BorderColorIdle;
                case MouseState.Enter:
                    return BorderColorHover;
                case MouseState.Down:
                    return BorderColorActive;
                default:
                    return BorderColorIdle;
            }
        }

        private void ConfigureInternalElements()
        {
            System.Diagnostics.Debug.Write("cuantas veces entra en este metodo..");
            int padding = (int)(BorderThickness * 1.5);
            int iconSize = Height - padding * 3;
            int buttonSize = iconSize;
            int buttonX = Width - buttonSize - padding;
            int buttonY = (Height / 2) - (buttonSize / 2);

            // Configuración inicial del botón
            _button.Size = new Size(buttonSize, buttonSize);
            _button.Location = new Point(buttonX, buttonY);

            // Configuración inicial del ícono
            _iconPictureLeft.Size = new Size(iconSize, iconSize);
            _iconPictureLeft.Location = new Point(padding, Height / 2 - iconSize / 2);

            // Configuración inicial del TextBox
            int textBoxX = padding + iconSize + padding;
            int textBoxWidth = Width - textBoxX - buttonSize - padding * 2;

            textBox1.Size = new Size(textBoxWidth, Height - padding * 2);
            textBox1.Location = new Point(textBoxX, Height / 2 - textBox1.Height / 2);
        }


        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            ConfigureInternalElements(); // Asegura que los elementos internos estén configurados.
            Invalidate(true); // Fuerza un redibujado completo.
        }

        private void DrawIcon(Graphics graphics)
        {
            if (_iconLeft != null && _iconPictureLeft.Visible)
            {
                graphics.DrawImage(_iconLeft, _iconPictureLeft.Bounds);
            }
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

        public string SelectedText
        {
            get
            {
                return textBox1.SelectedText;
            }
        }

        public  new   string Text
        {
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

        public decimal GetValue  
        {
            get {
              
                decimal.TryParse(textBox1.Text, out valueDecimal);
                return valueDecimal;
            }
        }
        private decimal valueDecimal =0;
        public bool HasValuedDecimal
        {
            get
            {

                return decimal.TryParse(textBox1.Text, out valueDecimal);
 
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
            else
            {
                state = MouseState.Enter;
                base.OnMouseEnter(e);
                Invalidate();
            }

        }

        public new string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                textBox1.Name = value;
            }
        }


        public Color PlaceHolderColor { get => placeholderColor; set => placeholderColor = value; }


        public string PlaceHolderText
        {
            get => placeholderText;
            set
            {

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
            if (textBox1.Focused)
            {
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
            else
            {
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

        public BorderStyle BorderStyle
        {
            get => borderStyle; set
            {
                borderStyle = value;
                Invalidate();
            }
        }

        public int BorderThickness
        {
            get => borderThickness; set
            {
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
                    //if (borderRadius == 0)
                    //    this.Padding = new Padding(1);
                    //else if (borderRadius < 5)
                    //    this.Padding = new Padding(3, 2, 3, 2);
                    //else if (borderRadius < 9)
                    //    this.Padding = new Padding(5, 3, 5, 3);
                    //else if (borderRadius < 12)
                    //    this.Padding = new Padding(6, 4, 6, 4);
                    //else
                    //    this.Padding = new Padding(8, 5, 7, 5);

                    UpdateControlHeight();
                    this.Invalidate();//Redraw control
                }
            }
        }


        private void UpdateControlHeight()
        {
            if (!this.Multiline)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new Size(0, txtHeight);
                textBox1.Multiline = false;

                this.Height = 4 + textBox1.Height + this.Padding.Top + this.Padding.Bottom;
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
                if (value != Color.Transparent)
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
                textBox1.UseSystemPasswordChar = value;
            }
        }

        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(placeholderText))
            {
                isPlaceholder = true;
                textBox1.Text = placeholderText;
                textBox1.ForeColor = placeholderColor;
                textBox1.UseSystemPasswordChar = false;
            }
            else
            {
                textBox1.ForeColor = this._foreColor;
                textBox1.UseSystemPasswordChar = isPasswordChar;
            }
        }

        private void RemovePlaceholder()
        {
            if (isPlaceholder && placeholderText != "" && this.baseText.ToUpper().Equals(placeholderText.ToUpper()))
            {
                isPlaceholder = false;
                textBox1.Text = "";
                textBox1.ForeColor = this.ForeColor;
                textBox1.UseSystemPasswordChar = isPasswordChar;
            }
            else {
                textBox1.UseSystemPasswordChar = isPasswordChar;
            }
        }


        public string ToolTipButton
        {

            get { return _ToolTipButtonToString; }

            set
            {
                if (value == null || string.IsNullOrEmpty(value))
                {
                    _ToolTipButtonToString = string.Empty;
                    _ToolTip1.SetToolTip(_button, _ToolTipButtonToString);

                }
                else
                {
                    _ToolTipButtonToString = value;
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
            get => base.ForeColor;
            set
            {
                base.ForeColor = value; // Asegúrate de establecer el valor en la clase base.
                _foreColor = value;
                textBox1.ForeColor = value; // Propaga el color al control interno.
                this.Invalidate(); // Redibuja el control para reflejar el cambio.
            }
        }


        public bool Autosize { get => _autosize; set => _autosize = value; }

        public CharacterCasing CharacterCasing { get => textBox1.CharacterCasing; set => textBox1.CharacterCasing = value; }

        public int DecimalPosition
        {
            get => decimalPosition;
            set
            {
                decimalPosition = value;
                textBox1.Text = string.Empty;
                Invalidate();

            }

        }

        public bool Multiline
        {
            get => textBox1.Multiline;
            set
            {
                textBox1.Multiline = value;
                Invalidate();
            }

        }

        public int SelectionLength
        {
            get => textBox1.SelectionLength;
            set
            {
                textBox1.SelectionLength = value;
                textBox1.Text = string.Empty;
                Invalidate();
            }

        }

        public TypeDataEnum TypeData
        {
            get => _typeData;
            set
            {
                _typeData = value;
                switch (_typeData)
                {
                    case TypeDataEnum.Numeric:
                        textBox1.Text = "";
                        break;
                    case TypeDataEnum.Decimal:
                        textBox1.Text = "";
                        break;
                    case TypeDataEnum.VarChar:
                        textBox1.Text = "";
                        break;
                    case TypeDataEnum.DateTime:
                        textBox1.Text = DateTime.Now.ToString();
                        break;
                    default:
                        break;
                }

                Invalidate();
            }

        }

        public AutoCompleteMode AutoCompleteMode
        {
            get => textBox1.AutoCompleteMode;
            set
            {
                textBox1.AutoCompleteMode = value;
                Invalidate();
            }

        }

        public AutoCompleteSource AutoCompleteSource
        {
            get => textBox1.AutoCompleteSource;
            set
            {
                textBox1.AutoCompleteSource = value;
                Invalidate();
            }

        }

        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get => textBox1.AutoCompleteCustomSource;
            set
            {
                textBox1.AutoCompleteCustomSource = value;
                Invalidate();
            }

        }


        #endregion


    }

}
