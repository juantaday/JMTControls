using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class TextBoxButton : TextBox//TextBoxButton
    {
        private bool _VisibleButton;
        private readonly Button _button;
        public TextBoxButton()
        {
            _button = new Button
            {
                Cursor = Cursors.Default,
                TabStop = false,
                BackgroundImage = Properties.Resources.ellipsis1,
                BackgroundImageLayout = ImageLayout.Zoom
            };
            this.Controls.Add(_button);
            PosicionarBoton();
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
           // PosicionarBoton();
        }

        private void PosicionarBoton()
        {
            _button.Size = new Size(this.ClientSize.Height, this.ClientSize.Height);
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
                    _button.BackgroundImage = Properties.Resources.ellipsis1;
                else
                    _button.BackgroundImage = _buttonImage;
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

        public Button Button {
            get {return _button; }
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


    }
}
