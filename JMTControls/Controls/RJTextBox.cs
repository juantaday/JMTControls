using JMControls.Enums;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
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
        public   new string Text
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

        public CharacterCasing CharacterCasing { get=>textBox1.CharacterCasing;
            set {

                textBox1.CharacterCasing =value;
                base.Invalidate();
            }
        }

        public int MaxLength { get=>textBox1.MaxLength; set=> textBox1.MaxLength= value;}

        public bool ReadOnly { get=>textBox1.ReadOnly; set=>textBox1.ReadOnly = value ;}
        public HorizontalAlignment TextAlign { get=>textBox1.TextAlign;

            set {
                textBox1.TextAlign = value;
                base.Invalidate();
            } 
        }

        public TypeDataEnum TypeData { get=>_typeData;

            set
            {
                _typeData = value;
                textBox1.Text = string.Empty;
                base.Invalidate();
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

        [Category("Action"), Description("Se genera cuando cambia el texto..")]
        public new event EventHandler TextChanged
        {
            add { textBox1.TextChanged += value; }
            remove { textBox1.TextChanged -= value; }
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

        public void SelectAll()
        {
            textBox1.SelectAll();
        }

        public RJTextBox()
        {
            this.InitializeComponent();
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
            this.textBox1 = new TextBox();
            base.SuspendLayout();
            this.textBox1.BorderStyle = BorderStyle.None;
            this.textBox1.Dock = DockStyle.Fill;
            this.textBox1.Location = new Point(10, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(230, 15);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += TextBox1_TextChanged;

            baseText = string.Empty;
            textBox1.Text = baseText;
            this.Text = baseText;

            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = SystemColors.Window;
            base.Controls.Add(this.textBox1);
            this.Font = new Font("Microsoft Sans Serif", 9.5f);
            this.ForeColor = Color.FromArgb(64, 64, 64);
            base.Margin = new Padding(4);
            base.Name = "RJTextBox";
            base.Padding = new Padding(10, 7, 10, 7);
            base.Size = new Size(250, 30);
            base.ResumeLayout(false);
            base.PerformLayout();
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
            if (isPlaceholder && placeholderText != "" && this.baseText.Equals(placeholderText))
            {
                isPlaceholder = false;
                textBox1.Text = "";
                textBox1.ForeColor = this.ForeColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = true;
            }
        }

        private void SetPlaceholder()
        {
            if ((!string.IsNullOrWhiteSpace(this.textBox1.Text) ? false : this.placeholderText != ""))
            {
                this.isPlaceholder = true;
                this.textBox1.Text = this.placeholderText;
                this.textBox1.ForeColor = this.placeholderColor;
                if (this.isPasswordChar)
                {
                    this.textBox1.UseSystemPasswordChar = false;
                }
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
            this.OnKeyPress(e);
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

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            baseText = textBox1.Text;

            if (string.IsNullOrEmpty(textBox1.Text) ||
                isPlaceholder)
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