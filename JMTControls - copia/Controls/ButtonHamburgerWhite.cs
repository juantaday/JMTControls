using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;



namespace JMControls.Controls
{
    public class ButtonHamburgerWhite : Button
    {
        private PictureBox _PictureBox;
        private bool _isCollepse;
        private int _heitParent;
        internal  CancellationTokenSource _cancelTokenSource;
        public Action<CancellationToken> ActionToExecute;

        public ButtonHamburgerWhite()
        {
            _PictureBox = new PictureBox
            {
                Cursor = Cursors.Default,
                TabStop = false
            };
            this.Controls.Add(_PictureBox);
            PosicionarBoton();

        }
        private void PosicionarBoton()
        {
            _PictureBox.Size = new Size(32, 32);
            _PictureBox.Location = new Point(2, 2);
            SendMessage(this.Handle, 0xd3, (IntPtr)2, (IntPtr)(_PictureBox.Width << 16));
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);


        [Category("Action")]
        public event EventHandler ImageClick
        {
            add { _PictureBox.Click += value; }
            remove { _PictureBox.Click -= value; }
        }

        private Image _buttonImageCollapse;

        [Category("ImageCollapse"), Description("Imagen del colapse")]
        public Image ImageIconCollapse
        {
            get
            {
                return _buttonImageCollapse;
            }
            set
            {
                _buttonImageCollapse = value;
                if (_buttonImageCollapse == null)
                    _PictureBox.Image = Properties.Resources.hamburger_22_white2;
                else
                    _PictureBox.Image = _buttonImageCollapse;
            }
        }

        private Image _buttonImageUp;
        public Image ImageIconUp
        {
            get
            {
                return _buttonImageUp;
            }
            set
            {
                _buttonImageUp = value;
                if (_buttonImageUp == null)
                    _PictureBox.Image = Properties.Resources.hamburger_22_Down_white;
                else
                    _PictureBox.Image = _buttonImageUp;
            }
        }
        public bool IsCollapse
        {
            get
            {
                return _isCollepse;
            }

            set
            {
                _isCollepse = value;
                using (var extender = new ButtonHamburgerWhite(new CancellationTokenSource()))
                {
                    extender.ActionToExecute = (CancellationTokenSource) => SetWith(_isCollepse);
                    extender.Star();
                }
                             
            }

        }

        ButtonHamburgerWhite(CancellationTokenSource cancelTokenSource)
            {
            _cancelTokenSource = cancelTokenSource;
          }
        private  void SetWith(bool collapse) {

    
                if (collapse)
                {
                    this.BackColor = Color.DimGray;
                    this.ForeColor = Color.Black;
                    _PictureBox.Image = Properties.Resources.hamburger_22_white2;
                    if (this.Parent != null && this.Parent is Panel)
                    {
                        Panel panel = (Panel)this.Parent;
                        _heitParent = panel.Height;
                        for (int i = 0; 36 <= panel.Height; i++)
                        {
                            this.Invoke(new MethodInvoker(() => {
                                panel.Height -= 2;
                                panel.Refresh();
                            }));

                        }

                    }

                }
                else
                {
                    _PictureBox.Image = Properties.Resources.hamburger_22_Down_white;
                    this.BackColor = Color.Black;
                    this.ForeColor = Color.White;
                    if (this.Parent != null && this.Parent is Panel)
                    {
                        Panel panel = (Panel)this.Parent;
                        if (panel == null) return;
                        if (panel != null && panel.Height > 0 && _heitParent == 0)
                        {
                            _heitParent = panel.Height;
                        }
                        for (int i = 0; _heitParent >= panel.Height; i++)
                        {
                            this.Invoke(new MethodInvoker(() => {
                                panel.Height += 2;
                                panel.Refresh();
                            }));
                        }

                    }
                }

        }


        public  async void Star() {
            try
            {
                //Task.Factory.StartNew(() =>
                //{
                //    ActionToExecute(_cancelTokenSource.Token);
                //});

             await   Task.Factory.StartNew(() =>
                    ActionToExecute(_cancelTokenSource.Token))
                    .ContinueWith((CancellationTokenSource) => TaskCompleted());

            }
            catch (Exception)
            {

                throw;
            }
        
        }
        private void TaskCompleted() {
            releaseCancellationTokenSource();
        }

        private void releaseCancellationTokenSource()
        {
            if (_cancelTokenSource != null)
            {
                _cancelTokenSource.Dispose();
                _cancelTokenSource = null;
            }
        }

    }
}
