namespace JMTControls.NetCore.Controls
{
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [Designer(typeof(AdvancedShadowPanelDesigner))]
    public class AdvancedShadowPanel : UserControl
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
            base.Controls.Add(_contentPanel);
            base.Controls.Add(_shadowPanel);


            base.SizeChanged += (s, e) =>
            {
                UpdateControlPositions();
            };


            UpdateControlPositions();
            this.BackColor = Color.Transparent;
        }

        // Propiedades configurables
        public Color ShadowColor
        {
            get => _shadowColor;
            set
            {
                _shadowColor = value;
                _shadowPanel.BackColor = value;
                Invalidate();
            }
        }

        // Propiedad BackColor
        public new Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor != value)
                {
                    _contentPanel.GradientStartColor = value;
                    _contentPanel.GradientEndColor = value;
                    base.Invalidate();
                }
            }
        }

        // Propiedad BorderSize
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                if (_borderSize != value)
                {
                    _borderSize = value;
                    _contentPanel.BorderSize = value;
                    base.Invalidate();
                }
            }
        }

        // Propiedad BorderColor
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    _contentPanel.BorderColor = _borderColor;
                    base.Invalidate();
                }
            }
        }

        // Propiedad ShadowSize
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

        // Propiedad BorderRadius
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
                    base.Invalidate();
                }
            }
        }

        // Actualiza la posición y tamaño de los paneles
        private void UpdateControlPositions()
        {
            // Usar Control.Width y Control.Height en lugar de this.Width y this.Height
            _contentPanel.Location = new Point(0, 0);
            _shadowPanel.Location = new Point(_shadowSize, _shadowSize);
            _contentPanel.Height = Height - _shadowSize;
            _contentPanel.Width = Width - _shadowSize;
            _shadowPanel.Height = Height - _shadowSize;
            _shadowPanel.Width = Width - _shadowSize;

            // Forzar la actualización visual
            base.Invalidate();
        }



        // Manejo de eventos de drag and drop
        protected override void OnDragEnter(DragEventArgs de)
        {
            if (de.Data.GetDataPresent(typeof(Control)))
            {
                de.Effect = DragDropEffects.Move;
            }
            base.OnDragEnter(de);
        }

        // Manejo de eventos de drop
        protected override void OnDragDrop(DragEventArgs de)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            IComponentChangeService changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            if (host != null && de.Data.GetDataPresent(typeof(Control)))
            {
                Control draggedControl = (Control)de.Data.GetData(typeof(Control));

                if (draggedControl != null)
                {
                    using (DesignerTransaction transaction = host.CreateTransaction("Add Control to AdvancedShadowPanel"))
                    {
                        changeService.OnComponentChanging(draggedControl, null); // Notifica cambio

                        draggedControl.Parent = draggedControl;
                        Point clientPoint = draggedControl.PointToClient(new Point(de.X, de.Y));
                        draggedControl.Location = clientPoint;

                        changeService.OnComponentChanged(draggedControl, null, null, null); // Notifica cambio
                        transaction.Commit(); // Guarda el cambio en el diseñador
                    }
                }
            }
            base.OnDragDrop(de);
        }
    }


    public class AdvancedShadowPanelDesigner : ParentControlDesigner
    {
        public override bool CanParent(Control control)
        {
            // Permite agregar cualquier control dentro del AdvancedShadowPanel
            return true;
        }

        protected override void OnDragEnter(DragEventArgs de)
        {
            // Asegura que el diseñador acepte elementos arrastrados
            de.Effect = DragDropEffects.Move;
            base.OnDragEnter(de);
        }

        protected override void OnDragDrop(DragEventArgs de)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (host != null && de.Data.GetDataPresent(typeof(Control)))
            {
                Control draggedControl = (Control)de.Data.GetData(typeof(Control));
                if (draggedControl != null)
                {
                    // Mueve el control al panel y ajusta su posición
                    draggedControl.Parent = Control;
                    Point clientPoint = Control.PointToClient(new Point(de.X, de.Y));
                    draggedControl.Location = clientPoint;
                }
            }
            base.OnDragDrop(de);
        }
    }


}
