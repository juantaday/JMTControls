using JMControls.Enums;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace JMControls.Controls
{
    [DefaultEvent("TextChanged")]
    public class RJTextBox : UserControl
    {
        private string baseText;

        private Color borderColor = Color.MediumSlateBlue;

        private Color borderFocusColor = Color.HotPink;

        private int borderSize = 2;

        private bool underlinedStyle = false;

        private bool isFocused = false;

        private int borderRadius = 0;

        private Color placeholderColor = Color.DarkGray;

        private string placeholderText = "";

        private bool isPlaceholder = false;

        private bool isPasswordChar = false;

        private IContainer components = null;

        private TextBox textBox1;
        private int decimalPosition = 2;
        private TypeDataEnum _typeData;

        [Category("RJ Code Advance")]
        public CharacterCasing CharacterCasin
        {
            get { return textBox1.CharacterCasing; }
            set { textBox1.CharacterCasing = value; }
        }

        [Category("RJ Code Advance")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.textBox1.BackColor = value;
            }
        }

        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                this.borderColor = value;
                base.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color BorderFocusColor
        {
            get
            {
                return this.borderFocusColor;
            }
            set
            {
                this.borderFocusColor = value;
            }
        }

        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get
            {
                return this.borderRadius;
            }
            set
            {
                if (value >= 0)
                {
                    this.borderRadius = value;
                    base.Invalidate();
                }
            }
        }

        [Category("RJ Code Advance")]
        public int BorderThickness
        {
            get
            {
                return this.borderSize;
            }
            set
            {
                if (value >= 1)
                {
                    this.borderSize = value;
                    base.Invalidate();
                }
            }
        }

        [Category("RJ Code Advance")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                this.textBox1.Font = value;
                if (base.DesignMode)
                {
                    this.UpdateControlHeight();
                }
            }
        }

        [Category("RJ Code Advance")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                this.textBox1.ForeColor = value;
            }
        }

        [Category("RJ Code Advance")]
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

        [Category("RJ Code Advance")]
        public bool PasswordChar
        {
            get
            {
                return this.isPasswordChar;
            }
            set
            {
                this.isPasswordChar = value;
                if (!this.isPlaceholder)
                {
                    this.textBox1.UseSystemPasswordChar = value;
                }
            }
        }

        [Category("RJ Code Advance")]
        public Color PlaceHolderColor
        {
            get
            {
                return this.placeholderColor;
            }
            set
            {
                this.placeholderColor = value;
                if (this.isPlaceholder)
                {
                    this.textBox1.ForeColor = value;
                }
            }
        }

        [Category("RJ Code Advance")]
        public string PlaceHolderText
        {
            get
            {
                return this.placeholderText;
            }
            set
            {
                this.placeholderText = value;
                this.textBox1.Text = "";
                baseText = this.textBox1.Text;
                this.SetPlaceholder();
            }
        }


        [Browsable(true)]
        public new string Text
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


        [Category("RJ Code Advance")]
        public bool UnderlinedStyle
        {
            get
            {
                return this.underlinedStyle;
            }
            set
            {
                this.underlinedStyle = value;
                base.Invalidate();
            }
        }

        public CharacterCasing CharacterCasing
        {
            get => textBox1.CharacterCasing;
            set
            {

                textBox1.CharacterCasing =value;
                base.Invalidate();
            }
        }

        public int MaxLength { get => textBox1.MaxLength; set => textBox1.MaxLength= value; }

        public bool ReadOnly { get => textBox1.ReadOnly; set => textBox1.ReadOnly = value; }

        public HorizontalAlignment TextAlign
        {
            get => textBox1.TextAlign;

            set
            {
                textBox1.TextAlign = value;
                base.Invalidate();
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

        [Browsable(true)]
        [Category("Action"), Description("Se genera cuando cambia el texto..")]
        public new event EventHandler TextChanged
        {
            add { textBox1.TextChanged += value; }
            remove { textBox1.TextChanged -= value; }
        }


        [Browsable (true)]
        [Category("Action")]
        [Description("Se produce cuando se presiona una tecla mientras el control tiene el foco.")]
        public new event KeyPressEventHandler KeyPress
        {
            add { textBox1.KeyPress += value; }
            remove { textBox1.KeyPress -= value; }
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

        public decimal GetValue
        {
            get
            {

                decimal.TryParse(textBox1.Text, out valueDecimal);
                return valueDecimal;
            }
        }
        private decimal valueDecimal = 0;
        public bool HasValuedDecimal
        {
            get
            {

                return decimal.TryParse(textBox1.Text, out valueDecimal);

            }
        }

        public void SelectAll()
        {
            textBox1.SelectAll();
        }

        public RJTextBox()
        {
            this.InitializeComponent();


            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;
            textBox1.KeyPress += textBox1_KeyPress;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.Click += textBox1_Click;
            textBox1.MouseDoubleClick += textBox1_MouseDoubleClick;
           
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
            textBox1.SelectAll();
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
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

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(10, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(230, 15);
            this.textBox1.TabIndex = 0;
            // 
            // RJTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RJTextBox";
            this.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.Size = new System.Drawing.Size(250, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.UpdateControlHeight();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            if (this.borderRadius <= 1)
            {
                using (Pen pen = new Pen(this.borderColor, (float)this.borderSize))
                {
                    base.Region = new Region(base.ClientRectangle);
                    pen.Alignment = PenAlignment.Inset;
                    if (this.isFocused)
                    {
                        pen.Color = this.borderFocusColor;
                    }
                    if (!this.underlinedStyle)
                    {
                        graphics.DrawRectangle(pen, 0f, 0f, (float)base.Width - 0.5f, (float)base.Height - 0.5f);
                    }
                    else
                    {
                        graphics.DrawLine(pen, 0, base.Height - 1, base.Width, base.Height - 1);
                    }
                }
            }
            else
            {
                Rectangle clientRectangle = base.ClientRectangle;
                Rectangle rectangle = Rectangle.Inflate(clientRectangle, -this.borderSize, -this.borderSize);
                int num = (this.borderSize > 0 ? this.borderSize : 1);
                using (GraphicsPath figurePath = this.GetFigurePath(clientRectangle, this.borderRadius))
                {
                    using (GraphicsPath graphicsPath = this.GetFigurePath(rectangle, this.borderRadius - this.borderSize))
                    {
                        using (Pen pen1 = new Pen(base.Parent.BackColor, (float)num))
                        {
                            using (Pen pen2 = new Pen(this.borderColor, (float)this.borderSize))
                            {
                                base.Region = new Region(figurePath);
                                if (this.borderRadius > 15)
                                {
                                    this.SetTextBoxRoundedRegion();
                                }
                                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                pen2.Alignment = PenAlignment.Center;
                                if (this.isFocused)
                                {
                                    pen2.Color = this.borderFocusColor;
                                }
                                if (!this.underlinedStyle)
                                {
                                    graphics.DrawPath(pen1, figurePath);
                                    graphics.DrawPath(pen2, graphicsPath);
                                }
                                else
                                {
                                    graphics.DrawPath(pen1, figurePath);
                                    graphics.SmoothingMode = SmoothingMode.None;
                                    graphics.DrawLine(pen2, 0, base.Height - 1, base.Width, base.Height - 1);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (base.DesignMode)
            {
                this.UpdateControlHeight();
            }
        }

        private void RemovePlaceholder()
        {
            if ( !string.IsNullOrEmpty(placeholderText) && this.baseText.ToUpper().Equals(placeholderText.ToUpper()))
            {
                isPlaceholder = false;
                textBox1.Text = "";
                textBox1.ForeColor = this.ForeColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = true;
            }
            else  {
                textBox1.ForeColor = this.ForeColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = true;
            }
        }

        private void SetPlaceholder()
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(placeholderText))
            {
                this.isPlaceholder = true;
                this.textBox1.Text = this.placeholderText;
                this.textBox1.ForeColor = this.placeholderColor;
                if (this.isPasswordChar)
                {
                    this.textBox1.UseSystemPasswordChar = false;
                }
            }
            else {
                this.isPlaceholder = false;
            }
        }

        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath figurePath;
            if (!this.Multiline)
            {
                figurePath = this.GetFigurePath(this.textBox1.ClientRectangle, this.borderSize * 2);
                this.textBox1.Region = new Region(figurePath);
            }
            else
            {
                figurePath = this.GetFigurePath(this.textBox1.ClientRectangle, this.borderRadius - this.borderSize);
                this.textBox1.Region = new Region(figurePath);
            }
            figurePath.Dispose();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.isFocused = true;
            base.Invalidate();
            this.RemovePlaceholder();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            this.isFocused = false;
            base.Invalidate();
            this.SetPlaceholder();
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox1.Text) ||( this.placeholderText.ToUpper().Equals(textBox1.Text.ToUpper())))
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


    }
}