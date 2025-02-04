using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{

    [DefaultEvent("ButtonClick")]
    public class AltoSlidingLabel : Control
    {
        private Timer timer = new Timer();
        private bool slide;
        private int a;
        private bool art = false;

        // Propiedades para el botón
        private bool _buttonVisible = false;
        private Image _buttonImage = null;
        private int _buttonWidth = 20;
        private bool _isHovered = false;

        // Propiedad para el hover de todo el control
        private bool _isControlHovered = false;

        public bool Slide
        {
            get { return slide; }
            set
            {
                slide = value;
                timer.Enabled = slide;
                if (!slide)
                {
                    a = 0;
                    Invalidate();
                }
            }
        }

        // Propiedades del botón
        public bool ButtonVisible
        {
            get { return _buttonVisible; }
            set
            {
                _buttonVisible = value;
                Invalidate();
            }
        }

        public Image ButtonImage
        {
            get { return _buttonImage; }
            set
            {
                _buttonImage = value;
                Invalidate();
            }
        }

        public int ButtonWidth
        {
            get { return _buttonWidth; }
            set
            {
                _buttonWidth = value;
                Invalidate();
            }
        }

        public AltoSlidingLabel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
                    ControlStyles.UserPaint, true);
            AutoSize = false;
            Width = 30;
            Height = 15;
            a = 0;
            timer.Interval = 120;
            timer.Tick += timer_Tick;
            slide = false;
            timer.Enabled = false;

            // Habilitar el manejo de eventos de mouse
            this.MouseMove += AltoSlidingLabel_MouseMove;
            this.MouseLeave += AltoSlidingLabel_MouseLeave;
            this.MouseEnter += AltoSlidingLabel_MouseEnter;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Size tSize = TextRenderer.MeasureText(Text, Font);
            if (tSize.Width <= Width - (_buttonVisible ? _buttonWidth : 0))
            {
                timer.Stop();
                a = 1;
                Invalidate();
                return;
            }
            int maxFar = tSize.Width >= Width ? tSize.Width - Width : 0;
            if (a >= 1)
                art = false;
            if (-a >= maxFar + Font.Height)
                art = true;
            a = art ? a + 1 : a - 1;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            timer.Enabled = true;
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Obtener el color de fondo ajustado para el hover
            Color backgroundColor = _isControlHovered ? AdjustBrightness(BackColor, 0.2f) : BackColor;

            // Dibujar el fondo con el color ajustado
            using (Brush brush = new SolidBrush(backgroundColor))
                e.Graphics.FillRectangle(brush, this.ClientRectangle);

            // Calcular el área disponible para el texto (excluyendo el botón)
            int textWidth = Width - (_buttonVisible ? _buttonWidth + 2 : 0); // Añadir margen derecho
            Size tSize = TextRenderer.MeasureText(Text, Font);
            int y = Height / 2 - tSize.Height / 2;

            // Dibujar el texto
            using (Brush brush = new SolidBrush(ForeColor))
                e.Graphics.DrawString(Text, Font, brush, a, y);

            // Dibujar el botón si está visible
            if (_buttonVisible && _buttonImage != null)
            {
                int buttonHeight = _buttonImage.Height + 10; // Tamaño de la imagen más 10px de margen
                int buttonX = Width - _buttonWidth - 2; // Posición X del botón con margen de 2px
                int buttonY = (Height - buttonHeight) / 2; // Centrar el botón verticalmente

                // Dibujar el fondo del botón con efecto hover
                if (_isHovered)
                {
                    using (Brush hoverBrush = new SolidBrush(Color.Gray))
                        e.Graphics.FillRectangle(hoverBrush, buttonX, buttonY, _buttonWidth, buttonHeight);
                }

                // Calcular la posición para centrar la imagen
                int imageX = buttonX + (_buttonWidth - _buttonImage.Width) / 2;
                int imageY = buttonY + (buttonHeight - _buttonImage.Height) / 2;

                // Dibujar la imagen en su tamaño original
                e.Graphics.DrawImage(_buttonImage, imageX, imageY, _buttonImage.Width, _buttonImage.Height);
            }

            base.OnPaint(e);
        }

        private void AltoSlidingLabel_MouseEnter(object sender, EventArgs e)
        {
            // Activar el hover de todo el control
            _isControlHovered = true;
            Invalidate();
        }

        private void AltoSlidingLabel_MouseLeave(object sender, EventArgs e)
        {
            // Desactivar el hover del botón cuando el mouse sale del control principal
            if (_isHovered)
            {
                _isHovered = false;
                Invalidate(); // Redibujar el control para ocultar el hover
            }
        }

        private void AltoSlidingLabel_MouseMove(object sender, MouseEventArgs e)
        {
            // Verificar si el mouse está sobre el botón
            if (_buttonVisible && IsPointInButton(e.Location))
            {
                if (!_isHovered)
                {
                    _isHovered = true;
                    Invalidate(); // Redibujar el control para mostrar el hover
                }
            }
            else if (_isHovered)
            {
                _isHovered = false;
                Invalidate(); // Redibujar el control para ocultar el hover
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {

            // Verificar si el botón está visible y si el clic ocurrió dentro del área del botón
            if (_buttonVisible && IsPointInButton(e.Location))
            {
                OnButtonClick(EventArgs.Empty);
            }

            base.OnMouseClick(e);
        }

        private bool IsPointInButton(Point point)
        {
            // Calcular el área del botón
            int buttonHeight = _buttonImage.Height + 10; // Tamaño de la imagen más 10px de margen
            int buttonX = Width - _buttonWidth - 2; // Posición X del botón con margen de 2px
            int buttonY = (Height - buttonHeight) / 2; // Centrar el botón verticalmente

            // Crear un rectángulo que representa el área del botón
            Rectangle buttonRect = new Rectangle(buttonX, buttonY, _buttonWidth, buttonHeight);

            // Verificar si el punto está dentro del rectángulo
            return buttonRect.Contains(point);
        }

        // Evento para el clic en el botón
        public event EventHandler ButtonClick;
        protected virtual void OnButtonClick(EventArgs e)
        {
            ButtonClick?.Invoke(this, e);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            Invalidate();
            base.OnBackColorChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            Invalidate();
            base.OnForeColorChanged(e);
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                timer.Start();
            }
        }

        protected override void Dispose(bool disposing)
        {
            timer.Stop();
            base.Dispose(disposing);
        }

        // Función para ajustar el brillo de un color
        private Color AdjustBrightness(Color color, float factor)
        {
            float r = color.R / 255.0f;
            float g = color.G / 255.0f;
            float b = color.B / 255.0f;

            // Ajustar el brillo
            r = Math.Min(1.0f, r * (1 + factor));
            g = Math.Min(1.0f, g * (1 + factor));
            b = Math.Min(1.0f, b * (1 + factor));

            // Convertir de nuevo a Color
            return Color.FromArgb(color.A, (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
    }

}
