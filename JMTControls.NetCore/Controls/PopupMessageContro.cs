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

        public void ShowSuccessMessage(Control control, string message, Color backgroundColor, Color textColor, Image icon)
        {
            // Asignar valores personalizados
            this.Message = message;
            this.BackColor = backgroundColor;
            this.TextColor = textColor;
            this.Icon = icon;

            messageLabel.ForeColor = TextColor;
            successIcon.Image = Icon;
            messageLabel.Text = Message;

            // Calcular el tamaño adecuado del mensaje usando MeasureString
            using (Graphics g = this.CreateGraphics()) // Usamos el gráfico del control para medir el texto
            {
                string[] lines = Message.Split('\n'); // Dividir el mensaje en líneas
                float maxWidth = 0;
                float totalHeight = 0;

                foreach (string line in lines)
                {
                    SizeF lineSize = g.MeasureString(line.Trim(), MessageFont);
                    maxWidth = Math.Max(maxWidth, lineSize.Width); // Tomar el ancho máximo
                    totalHeight += lineSize.Height; // Sumar la altura de cada línea
                }

                this.Size = new Size((int)(maxWidth + successIcon.Width + 45), (int)(Math.Max(totalHeight, successIcon.Height) + 20));
            }


            // Calcular la posición del popup con respecto al formulario
            var parentForm = control.FindForm();
            if (parentForm != null)
            {
                // Convertir la ubicación del control a la ubicación en la pantalla
                Point location = control.Parent.PointToScreen(control.Location);

                // Convertir la ubicación a coordenadas dentro del formulario
                location = parentForm.PointToClient(location);

                // Mostrar arriba del control
                location.Y -= this.Height;
                if (location.Y < 0) // Si no hay espacio arriba, mostrar abajo
                {
                    location.Y += control.Height + this.Height;
                }

                // Ajustar la posición horizontal para que no se salga de la pantalla
                if (location.X + this.Width > parentForm.ClientSize.Width)
                {
                    location.X = parentForm.ClientSize.Width - this.Width;
                }

                this.Location = location;

                // Mostrar el popup en el formulario actual
                parentForm.Controls.Add(this); // Agregar el control al formulario
                this.BringToFront();  // Asegurar que el mensaje quede encima de otros controles
                this.Visible = true;  // Hacer visible el mensaje

                // Iniciar el temporizador para cerrar el popup después de un tiempo
                disappearTimer.Start();
            }
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
