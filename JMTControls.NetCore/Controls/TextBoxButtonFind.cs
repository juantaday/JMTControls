using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class TextBoxButtonFind : TextBox
    {
        private bool _VisibleButton;
        private readonly Button _button;
        public TextBoxButtonFind()
        {
            this.Height = 35;
            _button = new Button
            {
                Cursor = Cursors.Default,
                TabStop = false,
                Image = Properties.Resources.zoom_Grin_24,
                ImageAlign = ContentAlignment.MiddleRight,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = 35
            };
            _button.FlatAppearance.BorderSize = 1;
            _button.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            _button.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            _button.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 200, 200);
            this.Controls.Add(_button);
            // PosicionarBoton();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //  PosicionarBoton();
        }

        private void PosicionarBoton()
        {
            _button.Size = new Size(45, this.ClientSize.Height);
            _button.Location = new Point(this.ClientSize.Width - _button.Width, 0);
            SendMessage(this.Handle, 0xd3, (IntPtr)2, (IntPtr)(_button.Width << 16));
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [Category("Action")]
        public event EventHandler ButtonClick
        {
            add { _button.Click += value; }
            remove { _button.Click -= value; }
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


    }

}
