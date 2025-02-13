using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    #region AltoNMUpDown

    public class AltoNumericUpDown : Control
    {
        AltoButton btnUp = new AltoButton();
        AltoButton btnDown = new AltoButton();
        TextBox textbox = new TextBox();


        private bool isChanging;
        private decimal value;
        private decimal _maximum;
        private decimal _minimum;
        private Color signColor;
        bool dec;
        System.Windows.Forms.Timer timer;
        private int _decimalPlace = 3;

        #region Constructor

        public AltoNumericUpDown()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                        ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
                        ControlStyles.UserPaint, true);
            this.Width = 60;
            this.Height = 20;
            this.TabStop = false;


            signColor = Color.White;
            _minimum = 0;
            _maximum = 999999;

            decimal.TryParse("0.000", out value);

            textbox.Text = value.ToString();
            dec = false;

            btnDown.Active1 = btnDown.Active2 = btnUp.Active1 = btnUp.Active2 = Color.Gray;
            btnDown.Inactive1 = btnDown.Inactive2 = btnUp.Inactive1 = btnUp.Inactive2 = Color.LightSlateGray;
            btnDown.Radius = btnUp.Radius = 0;
            btnDown.Width = btnUp.Width = 20;
            btnDown.TabStop = false;
            btnDown.Parent = btnUp.Parent = this;
            btnDown.Paint += btnDown_Paint;


            btnUp.Stroke = btnDown.Stroke = true;
            btnUp.StrokeColor = btnDown.StrokeColor = Color.DarkGray;
            btnUp.TabStop = false;
            btnUp.Paint += btnUp_Paint;
            btnUp.Click += btnUp_Click;
            btnUp.MouseDown += btnUp_MouseDown;
            btnUp.MouseUp += btnUp_MouseUp;


            btnDown.MouseDown += btnDown_MouseDown;
            btnDown.MouseUp += btnDown_MouseUp;
            btnDown.Click += btnDown_Click;
            textbox.Parent = this;
            textbox.KeyDown += box_KeyDown;
            textbox.Location = new Point(3, 3);
            btnDown.Top = 0;
            textbox.BorderStyle = BorderStyle.None;
            Font = new Font("Comic Sans MS", 12);
            textbox.KeyPress += box_KeyPress;
            textbox.TextChanged += Box_TextChanged;
            this.Invalidate();
            timer = new System.Windows.Forms.Timer()
            {
                Interval = 400
            };
            timer.Tick += timer_Tick;
        }

        private void Box_TextChanged(object sender, EventArgs e)
        {
            if (isChanging) return;

            if (textbox.Text.Length > 0 && textbox.Text.Substring(textbox.Text.Length - 1, 1).Equals("."))
                return;

            decimal currentValue = Convert.ToDecimal(textbox.Text == "" ? "0" : textbox.Text);
            if (this.Value != currentValue)
                this.value = currentValue;
        }

        void box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                System.Media.SystemSounds.Asterisk.Play();
            }
        }



        void timer_Tick(object sender, EventArgs e)
        {
            if (dec)
            {
                if (value <= this.Minimum)
                {
                    timer.Stop();
                    return;
                }
                else
                {
                    value = value - 1;
                    this.textbox.Text = Math.Round(value, _decimalPlace).ToString();
                }

            }
            else
            {
                if (value >= this.Maximum)
                {
                    timer.Stop();
                    return;
                }
                else
                {
                    value = value + 1;
                    this.textbox.Text = Math.Round(value, _decimalPlace).ToString();
                }
            }

            if (timer.Interval >= 50)
                timer.Interval /= 2;

        }

        void btnDown_MouseUp(object sender, MouseEventArgs e)
        {
            timer.Interval = 400;
            timer.Stop();
            dec = false;
        }

        void btnDown_MouseDown(object sender, MouseEventArgs e)
        {
            btnDown.Focus();
            dec = true;
            value = decimal.Parse(textbox.Text);
            timer.Start();
        }

        void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            timer.Interval = 400;
            timer.Stop();
            dec = false;
        }

        void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            btnUp.Focus();
            dec = false;
            value = decimal.Parse(textbox.Text);
            timer.Start();
        }
        #endregion
        #region Press

        void btnDown_Click(object sender, EventArgs e)
        {

            if (value > _minimum)
            {
                value--;
                textbox.Text = value.ToString();
            }

        }

        void btnUp_Click(object sender, EventArgs e)
        {
            if (value < _maximum)
            {
                value++;
                textbox.Text = value.ToString();
            }

        }
        
        void box_KeyPress(object sender, KeyPressEventArgs e)
        {
            isChanging = true;

            if (this.ReadOnly)
                return;
                
            if (textbox.SelectedText.Length >= textbox.Text.Length)
                textbox.Text = "";
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
        && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;

            // only allow minus sign at the beginning
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
                e.Handled = true;

            var exist = (sender as TextBox).Text.IndexOf('.');
            if (e.KeyChar != '\b' && exist != -1)
            {
                string decimalReal = this.textbox.Text.Substring(exist + 1, this.textbox.Text.Length - (exist + 1));
                if (decimalReal.Length >= this._decimalPlace)
                    e.Handled = true;
            }


            isChanging = false;
        }


        [Category("Action"), Description("Se genera cuando cambia el texto..")]
        public new event EventHandler TextChanged
        {
            add { textbox.TextChanged += value; }
            remove { textbox.TextChanged -= value; }
        }


        [Category("Action"), Description("Se genera cuado preciona y sualta una tecla")]
        public new event KeyPressEventHandler KeyPress
        {
            add { textbox.KeyPress += value; }
            remove { textbox.KeyPress -= value; }
        }

        public new void Focus()
        {
            this.textbox.Focus();
        }

        public bool ReadOnly
        {
            get { return textbox.ReadOnly; }

            set
            {
                if (textbox.ReadOnly != value)
                {
                    this.textbox.ReadOnly = value;
                }
            }

        }


        protected override void OnResize(EventArgs e)
        {

            base.OnResize(e);
        }
        #endregion
        #region Paint

        void btnUp_Paint(object sender, PaintEventArgs e)
        {
            if (Height > 0)
            {
                textbox.Font = new System.Drawing.Font("Arial", Height * 0.5f);
                textbox.Location = new Point(3, Height / 2 - textbox.Height / 2);
            }
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            float w = btnUp.Width / 2 - 1;
            float h = btnUp.Height / 3;

            PointF p1 = new PointF(w / 2 - 2, h * 1.5f);
            PointF p2 = new PointF(3 * w / 2 + 2, h * 1.5f);
            PointF p11 = new PointF(w, h - 2);
            PointF p22 = new PointF(w, 2 * h + 2);
            using (Pen pen = new Pen(signColor, 3))
            {
                e.Graphics.DrawLine(pen, p1, p2);
                e.Graphics.DrawLine(pen, p11, p22);
            }
        }
        void btnDown_Paint(object sender, PaintEventArgs e)
        {
            float w = btnUp.Width / 3;
            float h = btnUp.Height / 3;
            PointF p1 = new PointF(btnDown.Width / 2 - w / 2, h * 1.5f);
            PointF p2 = new PointF(p1.X + w, h * 1.5f);
            using (Pen pen = new Pen(signColor, 3))
            {
                e.Graphics.DrawLine(pen, p1, p2);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            btnUp.Left = this.Width - btnUp.Width * 2 + 1;
            btnDown.Left = this.Width - btnDown.Width;
            btnDown.Top = 0;
            btnUp.Height = btnDown.Height = Height - 1;
            e.Graphics.FillRectangle(Brushes.White, 0, 0, Width, Height);
            e.Graphics.DrawRectangle(Pens.Gray, 0, 0, Width - 1, Height - 1);
            textbox.Width = this.Width - 2 * btnDown.Width - 4;
            using (Pen pen = new Pen(Color.Black, 2))
                base.OnPaint(e);
        }
        #endregion

        protected override void OnFontChanged(EventArgs e)
        {
            textbox.Font = Font;
            Height = Font.Height * 2;
            base.OnFontChanged(e);
        }
        public Color SignColor
        {
            get { return signColor; }
            set
            {
                signColor = value;
                Invalidate();
            }
        }

        public decimal Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    textbox.Text = value.ToString();
                    Invalidate();
                }

            }
        }

        public int DecimalPlace
        {
            get => _decimalPlace;
            set
            {
                if (_decimalPlace != value)
                {
                    _decimalPlace = value;
                    textbox.Text = Math.Round(this.Value, _decimalPlace).ToString();
                    Invalidate();
                }

            }
        }

        public decimal Minimum
        {
            get => _minimum;
            set
            {
                if (_minimum != value)
                {
                    _minimum = value;
                    if (this.Value < _minimum)
                        this.Value = _minimum;

                    textbox.Text = Math.Round(this.Value, _decimalPlace).ToString();
                    Invalidate();
                }

            }
        }

        public decimal Maximum
        {
            get => _maximum;
            set
            {
                if (_maximum != value)
                {
                    _maximum = value;
                    if (this.Value > _maximum)
                        this.Value = _maximum;

                    textbox.Text = Math.Round(this.Value, _decimalPlace).ToString();
                    Invalidate();
                }

            }
        }

        public Color ButtonBackColor
        {
            get => btnUp.Active1;
            set
            {
                if (btnUp.Active1 != value)
                {
                    btnUp.Active1 = value;
                    btnDown.Active1 = value;
                    Invalidate();
                }

            }
        }


        public HorizontalAlignment TextAlign
        {
            get => textbox.TextAlign;
            set
            {
                if (textbox.TextAlign != value)
                {
                    textbox.TextAlign = value;
                    Invalidate();
                }

            }
        }


    }

    #endregion
}
