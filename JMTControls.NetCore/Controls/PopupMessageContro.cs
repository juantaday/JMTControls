using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public partial class PopupMessageContro : UserControl
    {
        private int _duration;  
        private string _message;
        private Color _textColor;
        private Color _backColor;
        private Image _icon;
        private Font _messageFont;  

        public PopupMessageContro()
        {
            InitializeComponent();

            this.DisplayDuration = disappearTimer.Interval = 2000;    
            this.MessageFont = new Font("Arial", 10, FontStyle.Bold);   
            this.Icon = SystemIcons.Information.ToBitmap();
            this.Message = "¡Operación exitosa!";
            this.BackColor = Color.LightGreen;
      
        }

        private void PopupMessageContro_Load(object sender, EventArgs e)
        {

        }



        [DefaultValue(typeof(Color), "LightGreen")]
        [Description("Color de fondo del mensaje")]
        [Category("Apariencia")]
        [Browsable(true)]
        public new  Color BackColor
        {
            get => _backColor; // Devuelve el valor de la propiedad privada _backColor
            set
            {
                if (value != _backColor) // Solo actualizar si el valor cambia
                {
                    _backColor = value; // Asigna el nuevo valor a la propiedad privada
                    this.BackColor = _backColor; // Actualiza el color de fondo del control
                    base.BackColor = _backColor;
                    Invalidate(); // Invalida el control para forzar un repintado
                }
            }
        }


        // valor predeterminado del color

        [DefaultValue(typeof(Color), "Green")]  // Establece el valor por defecto como Color.Green
        [Description("Mensaje a mostrar en el popup")]
        [Category("Apariencia")]
        [Browsable(true)]
        public Color TextColor
        {
            get => _textColor;
            set
            {
                if (value != _textColor)
                {
                    _textColor = value;
                    messageLabel.ForeColor = _textColor;
                    this.Invalidate(); // Invalidar el control para forzar un repintado
                }
            }
        }   




        [DefaultValue("¡Operación exitosa!")]
        [Description("Mensaje a mostrar en el popup")]
        [Category("Apariencia")]
        [Browsable(true)]

        public string Message {
            get => _message; 
            set {
                if (value != _message)
                {
                    _message = value;
                    messageLabel.Text = _message;
                }       
            }
        }


        [DefaultValue(2000)]
        [Description("Duración total del mensaje (en milisegundos)")]
        [Category("Comportamiento")]
        [Browsable(true)]
        public int DisplayDuration { 
            get => _duration;
            set { 
                if (value != _duration )
                {
                    _duration = value;
                    disappearTimer.Interval = _duration;
                }
            }
        }


     
        [DefaultValue(typeof(Image), "SystemIcons.Information.ToBitmap()")]
        [Description("Ícono mostrado en el mensaje")]
        [Category("Apariencia")]
        [Browsable(true)]
        public Image Icon
        {
            get => _icon; // Devuelve el valor de la propiedad privada _icon
            set
            {
                if (value != _icon) // Solo actualizar si el valor cambia
                {
                    _icon = value; // Asigna el nuevo valor a la propiedad privada
                    successIcon.Image = _icon; // Actualiza el ícono mostrado en el control
                    Invalidate(); // Invalida el control para forzar un repintado
                }
            }
        }

        [DefaultValue(typeof(Font), "Arial, 12pt, Bold")]
        [Description("Fuente del mensaje")]
        [Category("Apariencia")]
        [Browsable(true)]
        public Font MessageFont
        {
            get => _messageFont; // Devuelve el valor de la propiedad privada _messageFont
            set
            {
                if (value != _messageFont) // Solo actualizar si el valor cambia
                {
                    _messageFont = value; // Asigna el nuevo valor a la propiedad privada
                    messageLabel.Font = _messageFont; // Actualiza la fuente del texto en el control
                    Invalidate(); // Invalida el control para forzar un repintado
                }
            }
        }

        public void ShowSuccessMessage(Control control, string message,
           Color backgroundColor = default, Color textColor = default,
           Image icon = null)
        {
            // Si el control ya está en el formulario, solo actualizarlo
            var parentForm = control.FindForm();
            if (parentForm == null) return;

            if (!parentForm.Controls.Contains(this))
            {
                parentForm.Controls.Add(this); // Solo agregar si no está en el formulario
            }

            // Asignar valores personalizados
            this.Message = message;
            this.BackColor = backgroundColor == default ? Color.LightGreen : backgroundColor;
            this.TextColor = textColor == default ? Color.Black : textColor;
            this.Icon = icon ?? SystemIcons.Information.ToBitmap();

            messageLabel.ForeColor = TextColor;
            successIcon.Image = Icon;
            messageLabel.Text = Message;

            // Calcular el tamaño adecuado del mensaje usando MeasureString
            using (Graphics g = this.CreateGraphics())
            {
                string[] lines = Message.Split('\n');
                float maxWidth = 0;
                float totalHeight = 0;

                foreach (string line in lines)
                {
                    SizeF lineSize = g.MeasureString(line.Trim(), MessageFont);
                    maxWidth = Math.Max(maxWidth, lineSize.Width);
                    totalHeight += lineSize.Height;
                }

                this.Size = new Size((int)(maxWidth + successIcon.Width + 55), (int)(Math.Max(totalHeight, successIcon.Height) + 20));
            }

            // Calcular la posición del popup con respecto al formulario
            Point location = control.Parent.PointToScreen(control.Location);
            location = parentForm.PointToClient(location);
            location.Y -= this.Height;

            if (location.Y < 0)
            {
                location.Y += control.Height + this.Height;
            }

            if (location.X + this.Width > parentForm.ClientSize.Width)
            {
                location.X = parentForm.ClientSize.Width - this.Width;
            }

            this.Location = location;

            // Asegurar que el mensaje quede visible y en el frente
            this.BringToFront();
            this.Visible = true;
            disappearTimer.Start();
        }




        private void disappearTimer_Tick(object sender, EventArgs e)
        {
            if (this == null || this.IsDisposed || this.Parent == null)
            {
                disappearTimer?.Stop();
                return; // Salir para evitar errores si el control ya no existe
            }

            // Desaparecer el mensaje después de la duración configurada
            disappearTimer?.Stop();
            this.Visible = false;  // Ocultar el popup después de que termine el temporizador
        }

    }
}
