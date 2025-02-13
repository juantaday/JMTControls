using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace JMTControls.NetCore.Controls
{
    [Designer(typeof(ParentControlDesigner))]
    public class AdvancedShadowPanel : ContainerControl
    {
        private RoundedGradientPanel _shadowPanel;
        private RoundedGradientPanel _contentPanel;
        private int _shadowSize = 5;
        private Color _shadowColor = Color.FromArgb(50, 0, 0, 0);
        private Color _backColor = Color.Transparent;
        private Color _borderColor = Color.Gray;
        private int _borderSize = 1;
        private int _borderRadius = 20;
        public AdvancedShadowPanel()
        {
            // Crear el panel de sombra
            _shadowPanel = new RoundedGradientPanel
            {
                Dock = DockStyle.None,
                GradientEndColor = Color.Transparent,
                GradientStartColor = _shadowColor,
                Margin = new Padding(0),
                Location = new Point(_shadowSize, _shadowSize),
                BorderSize = 0,
                BorderStyle = BorderStyle.None,
                BorderRadius = BorderRadius,
            };
            // Crear el panel de contenido
            _contentPanel = new RoundedGradientPanel
            {
                Dock = DockStyle.None,
                GradientEndColor = BackColor,
                GradientStartColor = BackColor,
                BorderColor = BorderColor,
                BorderSize = BorderSize,
                BorderRadius = BorderRadius,
            };
            this.Controls.Add(_contentPanel);
            this.Controls.Add(_shadowPanel);
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            UpdateControlPositions();
            this.BackColor = Color.Transparent;
        }
        // Propiedades configurables
        public Color ShadowColor
        {
            get => _shadowColor;
            set { _shadowColor = value; _shadowPanel.BackColor = value; }
        }
        // backColor
        public new Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor != value)
                {
                    _contentPanel.GradientStartColor = value;
                    _contentPanel.GradientEndColor = value;
                    Invalidate();
                }
            }

        }
        // borderSize
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                if (_borderSize != value)
                {
                    _borderSize = value;
                    _contentPanel.BorderSize = value;
                    Invalidate();
                }
            }
        }
        // border color 
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    _contentPanel.BorderColor = _borderColor;
                    Invalidate();
                }
            }
        }
        public int ShadowSize
        {
            get => _shadowSize;
            set
            {
                if (_shadowSize != value)
                {
                    _shadowSize = value;
                    _contentPanel.Margin = new Padding(_shadowSize);
                    UpdateControlPositions();
                }
            }
        }
        // borderRadius
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                if (_borderRadius != value)
                {
                    _borderRadius = value;
                    _contentPanel.BorderRadius = value;
                    _shadowPanel.BorderRadius = value;
                    Invalidate();
                }
            }
        }
        private void UpdateControlPositions()
        {
            _contentPanel.Location = new Point(0, 0);
            _shadowPanel.Location = new Point(_shadowSize, _shadowSize);
            _contentPanel.Height = this.Height - _shadowSize;
            _contentPanel.Width = this.Width - _shadowSize;
            _shadowPanel.Height = this.Height - (_shadowSize);
            _shadowPanel.Width = this.Width - (_shadowSize);
            Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateControlPositions();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

    

    }
}
