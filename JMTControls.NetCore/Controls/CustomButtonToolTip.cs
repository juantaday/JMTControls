
namespace JMTControls.NetCore.Controls
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;


    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CustomButtonToolTip
    {
        private ToolTip _toolTip;
        private Control _associatedControl;

        public CustomButtonToolTip()
        {
            _toolTip = new ToolTip();

            // Valores por defecto
            Text = string.Empty;
            Title = "Información";
            Icon = ToolTipIcon.Info;
            BackColor = SystemColors.Info;
            ForeColor = SystemColors.InfoText;
            BorderColor = SystemColors.ActiveBorder;
            IsBalloon = false;
            ShowAlways = false;
            InitialDelay = 500;
            AutoPopDelay = 5000;
            ReshowDelay = 100;
        }

        [Browsable(true)]
        [Description("Texto del ToolTip")]
        public string Text { get; set; }

        [Browsable(true)]
        [Description("Título del ToolTip")]
        public string Title { get; set; }

        [Browsable(true)]
        [Description("Ícono del ToolTip")]
        public ToolTipIcon Icon { get; set; }

        [Browsable(true)]
        [Description("Color de fondo del ToolTip")]
        public Color BackColor { get; set; }

        [Browsable(true)]
        [Description("Color del texto del ToolTip")]
        public Color ForeColor { get; set; }

        [Browsable(true)]
        [Description("Color del borde del ToolTip")]
        public Color BorderColor { get; set; }

        [Browsable(true)]
        [Description("Usar estilo globo")]
        public bool IsBalloon { get; set; }

        [Browsable(true)]
        [Description("Mostrar siempre")]
        public bool ShowAlways { get; set; }

        [Browsable(true)]
        [Description("Retardo inicial en milisegundos")]
        public int InitialDelay { get; set; }

        [Browsable(true)]
        [Description("Tiempo visible en milisegundos")]
        public int AutoPopDelay { get; set; }

        [Browsable(true)]
        [Description("Retardo para re-muestra en milisegundos")]
        public int ReshowDelay { get; set; }

        internal void SetControl(Control control)
        {
            _associatedControl = control;
            ApplySettings();
        }

        private void ApplySettings()
        {
            if (_associatedControl == null) return;

            _toolTip.SetToolTip(_associatedControl, Text);
            _toolTip.ToolTipTitle = Title;
            _toolTip.ToolTipIcon = Icon;
            _toolTip.IsBalloon = IsBalloon;
            _toolTip.ShowAlways = ShowAlways;
            _toolTip.InitialDelay = InitialDelay;
            _toolTip.AutoPopDelay = AutoPopDelay;
            _toolTip.ReshowDelay = ReshowDelay;

            // Configurar colores personalizados
            _toolTip.BackColor = BackColor;
            _toolTip.ForeColor = ForeColor;
        }

        public void Show()
        {
            if (_associatedControl != null && !string.IsNullOrEmpty(Text))
            {
                _toolTip.Show(Text, _associatedControl,
                    _associatedControl.Width / 2,
                    _associatedControl.Height / 2,
                    AutoPopDelay);
            }
        }

        public void Hide()
        {
            if (_associatedControl != null)
            {
                _toolTip.Hide(_associatedControl);
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Text) ? "Sin texto" : $"ToolTip: {Text}";
        }
    }
}
