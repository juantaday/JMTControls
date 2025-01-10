namespace JMTControls.NetCore.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using JMTControls.NetCore.Enums;
    using JMTControls.NetCore.Helpers;
   


    [DefaultEvent("TextChanged")]
    public class TextBoxRounded : Control
    {

       
        private TextBox textBox;
        private Button searchButton;
        private PictureBox iconPictureBox;
        private ToolTip _toolTip1;

        private string placeholderText = "JMTControls TextBoxRounded..";
        private Color placeholderColor = Color.FromArgb(180,180,180);
        private bool isPlaceholder;
        private bool _visibleButton = true;
        private Image buttonImage = Properties.Resources.searchImage_16;
        private Image iconImage = Properties.Resources.calendarDark;
        private TypeDataEnum _typeData = TypeDataEnum.VarChar;
        private int decimalPosition = 2;
        private int _paddingLefth = 5;
        private MouseState state;
        private MouseState statetText;
        private BorderStyle borderStyle = BorderStyle.FixedSingle;

        public TextBoxRounded()
        {
          

            textBox = new TextBox();
            searchButton = new Button();
            iconPictureBox = new PictureBox();

            _toolTip1 = new System.Windows.Forms.ToolTip();

            state = MouseState.Leave;
            statetText = MouseState.Leave;

            textBox.BorderStyle = BorderStyle.None;
            textBox.Location = new Point(30, 5);
            textBox.Width = this.Width - 60;

            searchButton.Size = new Size(20, 20);
            searchButton.Location = new Point(this.Width - 25, 5);
            searchButton.FlatStyle = FlatStyle.Flat;
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.BackColor = this.BackColor;
            searchButton.Image =  buttonImage;
            searchButton.BackColor = Color.Transparent;
            searchButton.Visible = _visibleButton;

            iconPictureBox.Size = new Size(20, 20);
            iconPictureBox.Location = new Point(7, 5); // Ajuste inicial
            iconPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            iconPictureBox.Image =  iconImage;
            iconPictureBox.BackColor = Color.Transparent;
            iconPictureBox.Visible = false;

            this.Controls.Add(textBox);
            this.Controls.Add(searchButton);
            this.Controls.Add(iconPictureBox);
            this.Font  = new Font("Arial", 12);

           
            AdjustHeight();

            // Agregar eventos de mouse
            textBox.Enter += RemovePlaceholder;
            textBox.Leave += SetPlaceholder;

            textBox.MouseLeave += TextBox_MouseLeave;
            textBox.MouseMove += TextBox_MouseMove;
            iconPictureBox.MouseMove += TextBox_MouseMove;
            searchButton.MouseMove += TextBox_MouseMove;
            this.MouseMove += Control_MouseMove;

            textBox.TextChanged += TextBox_TextChanged;
            textBox.FontChanged += TextBox_FontChanged;
            textBox.KeyPress += TextBox1_KeyPress;

            this.BackColor = Color.White;
        }


        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (state == MouseState.Leave || textBox.Focused)
                return;

            state = MouseState.Leave;
            Invalidate();
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {

            if (statetText == MouseState.Down)
            {
                state = MouseState.Down;
            }
            else
            {
                if (state == MouseState.Enter)
                    return;

                state = MouseState.Enter;
                base.OnMouseEnter(e);
                Invalidate();
            }
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            if (textBox.Focused)
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
                state = MouseState.Leave;
                base.OnMouseEnter(e);
                Invalidate();
            }
        }

        private void TextBox_MouseLeave(object sender, EventArgs e)
        {
            if (statetText == MouseState.Leave)
            {
                state = MouseState.Leave;
                base.OnMouseEnter(e);
                Invalidate();
            }
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = false;
            state = MouseState.Down;
            base.OnMouseDown(e);
            Invalidate();
        }



        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            this.OnTextChanged(e);
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            statetText = MouseState.Leave;
            state = MouseState.Leave;
            if (string.IsNullOrEmpty(textBox.Text) && (this.placeholderText?? "").Length > 0)
            {
                isPlaceholder = true;
                textBox.Text = placeholderText;
                textBox.ForeColor = placeholderColor;
            }
            else {
                isPlaceholder = false;
            }
            Invalidate();
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            statetText = MouseState.Down;
            state = MouseState.Down;

            if (isPlaceholder)
            {
                isPlaceholder = false;
                textBox.Text = "";
                textBox.ForeColor = this.ForeColor;
            }

            Invalidate();
        }

        private void TextBox_FontChanged(object sender, EventArgs e)
        {
            AdjustHeight();
        }


        private bool _isUpdatingHeight;

        private void UpdateControlHeight()
        {
            if (_isUpdatingHeight) return;

            _isUpdatingHeight = true;

            try
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBox.MinimumSize = new Size(0, txtHeight);
                this.Height = 4 + textBox.Height + this.Padding.Top + this.Padding.Bottom;
            }
            finally
            {
                _isUpdatingHeight = false;
            }
        }


        private void AdjustHeight()
        {
            if (this.Width == 0) return;

            // Establece la altura mínima
            int minHeight = TextRenderer.MeasureText("Text", this.Font).Height + 18;

            // Si Multiline es false, ajusta automáticamente la altura
            if (!textBox.Multiline)
            {
                this.Height = minHeight;
                textBox.Height = textBox.PreferredHeight;
            }
            else
            {
                // Si Multiline es true, permite que el usuario redimensione el alto
                this.Height = Math.Max(this.Height, minHeight);
                textBox.Height = this.Height - 10;
            }

            // Ajuste del tamaño y la posición de los controles internos
            searchButton.Height = textBox.Height;
            iconPictureBox.Height = textBox.Height;

            int offset = Math.Max(BorderThickness, BorderRadius / 2);
            searchButton.Location = new Point(this.Width - searchButton.Width - offset - BorderThickness - 2, (this.Height - searchButton.Height) / 2);

            if (iconPictureBox.Visible)
            {
                iconPictureBox.Location = new Point(offset + BorderThickness, (this.Height - iconPictureBox.Height) / 2);
                textBox.Location = new Point(iconPictureBox.Right + _paddingLefth, (this.Height - textBox.Height) / 2);
                textBox.Width = this.Width - iconPictureBox.Width - searchButton.Width - (2 * offset) - (2 * BorderThickness) - 5;
            }
            else
            {
                textBox.Location = new Point(offset + BorderThickness + _paddingLefth, (this.Height - textBox.Height) / 2);
                textBox.Width = this.Width - searchButton.Width - (2 * offset) - (2 * BorderThickness) - 5;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width <= 0 || Height <= 0) return;
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Rellenar el fondo con el color del control padre
            g.Clear(this.Parent.BackColor);

            if (BorderStyle != BorderStyle.None && BorderThickness > 0)
            {
                using (Pen pen = new Pen(GetBorderColor(), BorderThickness))
                {
                    using (GraphicsPath path = GetRoundedRectanglePath(Width, Height, BorderRadius))
                    {
                        using (SolidBrush brush = new SolidBrush(this.BackColor))
                        {
                            g.FillPath(brush, path);
                        }
                        g.DrawPath(pen, path);
                    }
                }
            }
           
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

        /// <summary>
        /// Gets or sets a value indicating whether the button is visible.
        /// </summary>
        public bool VisibleButton
        {
            get => _visibleButton;

            set
            {
                if (_visibleButton != value)
                {
                    _visibleButton = value;
                    if (searchButton != null)
                    {
                        searchButton.Visible = _visibleButton;
                    }
                    Invalidate();
                }
            }
        }


        /// <summary>
        /// Gets or sets the tooltip text for the button.
        /// </summary>
        public string ToolTipButton
        {
            get => _toolTipButton;

            set
            {
                _toolTipButton = string.IsNullOrEmpty(value) ? string.Empty : value;
                _toolTip1.SetToolTip(searchButton, _toolTipButton);
                OnToolTipButtonChanged(EventArgs.Empty);
            }
        }

        // Campo privado para almacenar el valor de la propiedad.
        private string _toolTipButton = string.Empty;

        public event EventHandler ToolTipButtonChanged;

        protected virtual void OnToolTipButtonChanged(EventArgs e)
        {
            ToolTipButtonChanged?.Invoke(this, e);
        }


        public Button Button
        {
            get => searchButton;
            set => searchButton = value;    
        }


        [Category("Behavior")]
        [Browsable(true)]
        public CharacterCasing CharacterCasing
        {
            get => _characterCasing;
            set
            {
               textBox.CharacterCasing  =  _characterCasing = value;
               Invalidate();
            }
        }

        private CharacterCasing _characterCasing;

        [Category("Appearance")]
        public int BorderRadius { get; set; } = 14;

        [Category("Appearance")]
        public int BorderThickness { get; set; } = 2;

        [Category("Appearance")]
        public Color BorderColorIdle { get; set; } = Color.Gray;

        [Category("Appearance")]
        public Color BorderColorActive { get; set; } = Color.Red;

        [Category("Appearance")]
        public Color BorderColorHover { get; set; } = Color.Orange;

        [Category("Appearance")]
        public Color BorderColorDisable { get; set; } = Color.LightGray;


        [Category("Appearance")]
        public BorderStyle BorderStyle
        {
            get => borderStyle; set
            {
                borderStyle = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public string PlaceHolderText
        {
            get { return placeholderText; }
            set
            {
                placeholderText = value;
                SetPlaceholder(null, new EventArgs());
            }
        }

        [Category("Appearance")]
        public Color PlaceHolderColor
        {
            get { return placeholderColor; }
            set
            {
                placeholderColor = value;
                if (isPlaceholder)
                {
                    textBox.ForeColor = placeholderColor;
                }
            }
        }

        [Category("Appearance")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                textBox.BackColor = value;
                searchButton.BackColor = value;
                iconPictureBox.BackColor = value;
                this.Invalidate(); // Redibuja el control para aplicar el nuevo color de fondo
            }
        }

        [Category("Appearance")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                textBox.Font = value;
                searchButton.Font = value;
                iconPictureBox.Font = value;
                AdjustHeight();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Browsable(true)]
        public new  Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                textBox.ForeColor = value;
                searchButton.ForeColor = value;
                iconPictureBox.ForeColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Image ButtonImage
        {
            get { return buttonImage; }
            set
            {
                buttonImage = value;
                searchButton.Image = value;
            }
        }

        [Category("Appearance")]
        public Image IconLeft
        {
            get { return iconImage; }
            set
            {
                iconImage = value;
                iconPictureBox.Image = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color IconLeftBackColor
        {
            get { return iconPictureBox.BackColor; }
            set
            {
                iconPictureBox.BackColor = value;
                Invalidate();
            }
        }


        [Category("Appearance")]
        [Browsable(true)]
        public bool  IconLeftVisible
        {
            get { return iconPictureBox.Visible; }
            set
            {
                iconPictureBox.Visible = value;
                AdjustHeight();
                Invalidate();
            }
        }
     


        [Category("Behavior")]
        public bool UseSystemPasswordChar
        {
            get { return textBox.UseSystemPasswordChar; }
            set { textBox.UseSystemPasswordChar = value; }
        }

        [Category("Behavior")]
        public char PasswordChar
        {
            get { return textBox.PasswordChar; }
            set { textBox.PasswordChar = value == '\0' ? '\0' : '*'; }
        }

        [Category("Behavior")]
        public int MaxLength
        {
            get { return textBox.MaxLength; }
            set { textBox.MaxLength = value; }
        }

        [Category("Behavior")]
        public int TextLength
        {
            get { return textBox.TextLength; }
        }

        [Category("Behavior")]
        public HorizontalAlignment TextAlign
        {
            get { return textBox.TextAlign; }
            set { textBox.TextAlign = value; }
        }

        [Category("Behavior")]
        public bool ReadOnly
        {
            get { return textBox.ReadOnly; }
            set { textBox.ReadOnly = value; }
        }

        [Category("Behavior")]
        [Browsable(true)]
        [DefaultValue("")]
        public new string Text
        {
            get { return isPlaceholder ? string.Empty : textBox.Text; }
            set
            {
                if (textBox.Text != value) {
                    textBox.Text = value;
                    SetPlaceholder(null, null);
                }
                
            }
        }

        [Category("Behavior")]
        public bool Multiline
        {
            get => textBox.Multiline;
            set
            {
                textBox.Multiline = value;
                AdjustHeight();
                Invalidate();
            }
        }


        public new void Focus()
        {
            textBox.Focus();
        }

        public void SelectAll()
        {
            textBox.SelectAll();
        }

        public new void Select()
        {
            textBox.Select();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustHeight();
        }

        [Category("Action")]
        public event EventHandler ButtonClick
        {
            add { searchButton.Click += value; }
            remove { searchButton.Click -= value; }
        }


        public new event KeyPressEventHandler KeyPress
        {
            add { textBox.KeyPress += value; }
            remove { textBox.KeyPress -= value; }
        }

        public new event KeyEventHandler KeyUp
        {
            add { textBox.KeyUp += value; }
            remove { textBox.KeyUp -= value; }
        }

        public new event KeyEventHandler KeyDown
        {
            add { textBox.KeyDown += value; }
            remove { textBox.KeyDown -= value; }
        }

        public new event EventHandler TextChanged
        {
            add { textBox.TextChanged += value; }
            remove { textBox.TextChanged -= value; }
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
                        this.Text = "";
                        break;
                    case TypeDataEnum.Decimal:
                        this.Text = "";
                        break;
                    case TypeDataEnum.VarChar:
                        this.Text = "";
                        break;
                    case TypeDataEnum.DateTime:
                        this.Text = DateTime.Now.ToString();
                        break;
                    default:
                        break;
                }

                Invalidate();
            }
        }

        public int DecimalPosition
        {
            get => decimalPosition;
            set => decimalPosition = value;
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
                        if (e.KeyChar.ToString().Equals(".") && this.Text.Contains("."))
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
                            if (decimalPosition > 0 && textBox.Text.Contains("."))
                            {
                                var positionEdit = textBox.SelectionStart;
                                var lengthSelect = textBox.SelectionLength;
                                var indexOf = textBox.Text.IndexOf(".") + 1;
                                var dcmLength = textBox.Text.Substring(indexOf, (textBox.Text.Length - indexOf)).Length;

                                if ((positionEdit > indexOf) && ((dcmLength - lengthSelect) >= decimalPosition))
                                {
                                    e.Handled = true;
                                    return;
                                }
                            }
                        }
                        break;
                    case TypeDataEnum.VarChar:
                        // No se necesita validación adicional para texto
                        break;
                    case TypeDataEnum.DateTime:
                        // No se necesita validación adicional para DateTime
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                // Manejo de excepciones
            }
        }


        public AutoCompleteMode AutoCompleteMode
        {
            get => textBox.AutoCompleteMode;
            set
            {
                textBox.AutoCompleteMode = value;
                Invalidate();
            }
        }

        public AutoCompleteSource AutoCompleteSource
        {
            get => textBox.AutoCompleteSource;
            set
            {
                textBox.AutoCompleteSource = value;
                Invalidate();
            }

        }

        public int SelectionLength
        {
            get => textBox.SelectionLength;
            set
            {
                textBox.SelectionLength = value;
            }
        }

        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get => textBox.AutoCompleteCustomSource;
            set
            {
                textBox.AutoCompleteCustomSource = value;
                Invalidate();
            }

        }

        [Browsable(false)]
        public string SelectedText
        {
            get => textBox.SelectedText;
            set => textBox.SelectedText = value;
        }
    
        public new bool Enabled
        {
            get=> textBox.Enabled;
            set {
                if (base.Enabled != value) { 
                    base.Enabled = value;
                    textBox.Enabled = value;
                }
            }
        }

        // Bandera para detectar llamadas redundantes
        private bool disposed = false;

        // Método Dispose
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Liberar textBox
                    if (textBox != null)
                    {
                        textBox.Dispose();
                        textBox = null;
                    }

                    // disposing Button
                    if (searchButton != null)
                    {
                        searchButton.Dispose();
                        searchButton = null;
                    }

                    // disposing Iconimage
                    if (iconImage != null)
                    {
                        iconImage.Dispose();
                        iconImage = null;
                    }
                }

                // Llamar a la implementación base
                base.Dispose(disposing);

                disposed = true;
            }
        }


    }

}
