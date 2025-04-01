using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static JMTControls.NetCore.Controls.StepProgressControl;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Linq;

namespace JMTControls.NetCore.Controls
{
    [Designer(typeof(StepProgressControlDesigner))]
    [DefaultProperty("Steps")]
    [DefaultEvent("SelectedIndexChanged")]
    [ToolboxItem(true)]
    public class StepProgressControl : UserControl
    {
        #region Campos
        private int _indexStep = -1;
        private Color _activeColor = Color.FromArgb(0, 120, 215);
        private Color _inactiveColor = Color.Gray;
        private Color _completedColor = Color.Green;
        private int _stepHeight = 60;
        private int _circleSize = 30;
        private int _lineThickness = 2;
        private Panel _headerPanel;
        private Panel _navigationPanel;
        private Panel _contentPanel;
        private StepCollection _steps;
        private StepPage _currentStep;
        private ToolTip _navigationToolTip;

        public event EventHandler<StepChangingEventArgs> SelectedIndexChanging;
        public event EventHandler SelectedIndexChanged; // Evento después del cambio
        #endregion

        #region Propiedades
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Color for the active step")]
        public Color ActiveColor
        {
            get => _activeColor;
            set { _activeColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Color for inactive steps")]
        public Color InactiveColor
        {
            get => _inactiveColor;
            set { _inactiveColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Color for completed steps")]
        public Color CompletedColor
        {
            get => _completedColor;
            set { _completedColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Height of each step")]
        public int StepHeight
        {
            get => _stepHeight;
            set { _stepHeight = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Size of the step circle")]
        public int CircleSize
        {
            get => _circleSize;
            set { _circleSize = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Current active step (zero-based)")]
        public int IndexStep
        {
            get => _indexStep;
            set
            {
                if (value < 0 || value >= Steps.Count || value == _indexStep)
                    return;

                // Disparar el evento de "cambiando" que permite cancelar
                var args = new StepChangingEventArgs(_indexStep, value);
                SelectedIndexChanging?.Invoke(this, args);

                if (args.Cancel)
                    return;

                _indexStep = value;

                try
                {
                    UpdateContent();
                }
                finally
                {
                    Invalidate();
                }

                // Disparar el evento de "cambiado"
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);

                if (DesignMode && Site != null)
                {
                    try
                    {
                        SelectStepInDesigner(Steps[value]);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Designer selection error: {ex.Message}");
                    }
                }
            }
        }

        [Editor(typeof(StepPageSelectorEditor), typeof(UITypeEditor))]
        public StepPage CurrentStep
        {
            get => _currentStep;
            set
            {
                int index = Steps.IndexOf(value);   
                if (_currentStep != value && index >= 0)
                {
                    _currentStep = value;
                    this.IndexStep = index;  
                    Invalidate();
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StepCollection Steps => _steps ??= new StepCollection(this);

        [Browsable(false)]
        public Panel ContentPanel => _contentPanel;
        #endregion

        #region Constructor
        public StepProgressControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
        }
        #endregion

        #region Inicialización
        private void InitializeComponent()
        {
            this.SuspendLayout();


            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = _stepHeight + 30,
                BackColor = Color.White
            };

            _navigationPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40, // Altura suficiente para los botones
                BackColor = Color.Transparent
            };


            // Crear el ToolTip
            _navigationToolTip = new ToolTip
            {
                AutomaticDelay = 500,
                AutoPopDelay = 3000,
                InitialDelay = 500,
                ReshowDelay = 100,
                ShowAlways = true,
                OwnerDraw = true,
                IsBalloon = false,
                ToolTipTitle = "Navegación" // Título opcional
            };

            _navigationToolTip.Draw += ToolTip_Draw;
            _navigationToolTip.Popup += ToolTip_Popup;

            // Agregar botones reales
            var btnPrevious = new Button
            {
                Text = "◀",
                Font = new Font(Font.FontFamily, 12, FontStyle.Bold),
                Size = new Size(30, 30),
                Location = new Point(10 ,5),
                FlatStyle = FlatStyle.Flat,
                BackColor = SystemColors.Control,
                Cursor = Cursors.Hand,
                
            };
            btnPrevious.FlatAppearance.BorderSize = 1;
            btnPrevious.Click += (s, e) => PreviousStep();
            btnPrevious.MouseEnter += (s, e) => btnPrevious.BackColor = Color.LightGray;
            btnPrevious.MouseLeave += (s, e) => btnPrevious.BackColor = SystemColors.Control;
            btnPrevious.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
            btnPrevious.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 200, 200);
            // Agregar ToolTip al botón Anterior
            _navigationToolTip.SetToolTip(btnPrevious, "Paso anterior");

            var btnNext = new Button
            {
                Text = "▶",
                Font = new Font(Font.FontFamily, 12, FontStyle.Bold),
                Size = new Size(30, 30),
                Location = new Point(_navigationPanel.Width - 40, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = SystemColors.Control,
                Cursor = Cursors.Hand,
                Name ="nextButton"
            };

            btnNext.FlatAppearance.BorderSize = 1;
            btnNext.Click += (s, e) => NextStep();
            btnNext.MouseEnter += (s, e) => btnNext.BackColor = Color.LightGray;
            btnNext.MouseLeave += (s, e) => btnNext.BackColor = SystemColors.Control;
            btnNext.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
            btnNext.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 200, 200);

            // Agregar ToolTip al botón Siguiente
            _navigationToolTip.SetToolTip(btnNext, "Paso siguiente");

            _navigationPanel.Controls.Add(btnPrevious);
            _navigationPanel.Controls.Add(btnNext);

            // Agregar el panel de navegación al header
            _headerPanel.Controls.Add(_navigationPanel);
            _navigationPanel.BringToFront();

            // Ajustar el padding del header para el contenido
            _headerPanel.Padding = new Padding(0, 0, 0, 40);

            _headerPanel.Paint += HeaderPanel_Paint;

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Control,
                Name = "ContentPanel"
            };

            this.Controls.Add(_contentPanel);
            this.Controls.Add(_headerPanel);

            if (DesignMode)
            {
                UpdateDesigner();
                System.Diagnostics.Debug.WriteLine("Design mode:");  
            }



            this.Size = new Size(600, 400);
            this.ResumeLayout(false);
        }


        private void SelectStepInDesigner(StepPage page)
        {
            if (!DesignMode || page == null || this.Site == null) return;

            try
            {
                var selectionService = this.Site.GetService(typeof(ISelectionService)) as ISelectionService;
                if (selectionService != null)
                {
                    // Crear lista de componentes a seleccionar
                    List<IComponent> components = new List<IComponent> { this };

                    // Si la página es válida, agregarla a la selección
                    if (page != null && page.Site != null)
                    {
                        components.Add(page);
                    }

                    // Establecer la selección
                    selectionService.SetSelectedComponents(components, SelectionTypes.Auto);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error selecting step in designer: {ex.Message}");
            }
        }


        #endregion

        #region Métodos de diseño

  
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (DesignMode && e.Control is StepPage page)
            {
                // Actualizar el selector en modo diseño
                UpdateDesigner();

                // Si es el primer step, seleccionarlo por defecto
                if (Steps.Count == 1)
                {
                    CurrentStep = page;
                }
            }
        }

        internal void UpdateDesigner()
        {
            if (!DesignMode) return;

            UpdateContent();
        }

        private void ToolTip_Popup(object sender, PopupEventArgs e)
        { // Calcular posición correcta para el ToolTip
            var button = e.AssociatedControl as Button;
            if (button != null)
            {
                // Tamaño dinámico basado en el texto
                Size textSize = TextRenderer.MeasureText(_navigationToolTip.GetToolTip(button), button.Font);
                e.ToolTipSize = new Size(textSize.Width , textSize.Height + 10);

                // Verificar posición para no salir de pantalla
                Point screenPos = button.PointToScreen(Point.Empty);
                Screen currentScreen = Screen.FromControl(button);

                if (screenPos.X + e.ToolTipSize.Width > currentScreen.WorkingArea.Right)
                {
                    // Ajustar posición si el ToolTip se sale por la derecha
                    e.ToolTipSize = new Size(textSize.Width - 10, textSize.Height + 10);
                }
            }
        }

        private void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            // Fondo personalizado
            using (var backBrush = new SolidBrush(Color.LightGoldenrodYellow))
            using (var borderPen = new Pen(Color.Goldenrod))
            using (var textBrush = new SolidBrush(Color.Black))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
                e.Graphics.DrawRectangle(borderPen, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));

                // Texto centrado
                TextRenderer.DrawText(
                    e.Graphics,
                    e.ToolTipText,
                    e.Font,
                    new Rectangle(e.Bounds.X , e.Bounds.Y + 2, e.Bounds.Width - 4, e.Bounds.Height - 4),
                    Color.Black,
                    (e.ToolTipText.Equals ("Paso siguiente")? TextFormatFlags.Left : TextFormatFlags.HorizontalCenter) | TextFormatFlags.VerticalCenter);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _navigationToolTip?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Métodos públicos
        #region Navegación en Tiempo de Ejecución
        private bool _isUpdating;
        public bool NextStep()
        {
            return ChangeStep(IndexStep + 1);
        }

        public bool PreviousStep()
        {
            return ChangeStep(IndexStep - 1);
        }

        private bool ChangeStep(int newIndex)
        {
            if (_isUpdating) return false;

            try
            {
                _isUpdating = true;
                if (newIndex >= 0 && newIndex < Steps.Count)
                {
                    IndexStep = newIndex;
                    return true;
                }
                return false;
            }
            finally
            {
                _isUpdating = false;
            }
        }

        #endregion

        #region Gestión de contenido
        private void UpdateContent()
        {
            SuspendLayout(); // Suspender el diseño temporalmente

            try
            {
                if (!DesignMode)
                {
                    _contentPanel.SuspendLayout();
                    _contentPanel.Controls.Clear();
                }

                if (IndexStep >= 0 && IndexStep < Steps.Count)
                {
                    var currentPage = Steps[IndexStep];

                    if (!DesignMode)
                    {
                        _contentPanel.Controls.Add(currentPage);
                    }

                    currentPage.Visible = true;
                    currentPage.Dock = DockStyle.Fill;
                    currentPage.BringToFront();
                }
            }
            finally
            {
                if (!DesignMode)
                {
                    _contentPanel.ResumeLayout(true);
                }
                ResumeLayout(true); // Reanudar el diseño
            }

            // Forzar redibujado solo una vez
            _headerPanel.Invalidate();
        }


        #endregion

        #region Dibujado
        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            if (Steps.Count == 0) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Ajustar el área de dibujo para no incluir el espacio de los botones
            var drawHeight = _headerPanel.Height - _navigationPanel.Height;
            int stepWidth = _headerPanel.Width / Steps.Count;
            int circleY = drawHeight / 2;

            // Pre-cache de recursos
            using (var activeBrush = new SolidBrush(_activeColor))
            using (var inactiveBrush = new SolidBrush(_inactiveColor))
            using (var completedBrush = new SolidBrush(_completedColor))
            using (var whiteTextBrush = new SolidBrush(Color.White))
            using (var activeTextBrush = new SolidBrush(_activeColor))
            using (var inactiveTextBrush = new SolidBrush(_inactiveColor))
            using (var boldFont = new Font("Segoe UI", 10, FontStyle.Bold))
            using (var regularFont = new Font("Segoe UI", 8))
            using (var boldSmallFont = new Font("Segoe UI", 8, FontStyle.Bold))
            using (var regularNumberFont = new Font("Segoe UI", 10)) // Nueva fuente para números no activos
            {
                for (int i = 0; i < Steps.Count; i++)
                {
                    var step = Steps[i];
                    int circleX = (i * stepWidth) + (stepWidth / 2) - (_circleSize / 2);

                    // Dibujar línea conectora
                    if (i > 0)
                    {
                        using (var pen = new Pen(i <= _indexStep ? _completedColor : _inactiveColor, _lineThickness))
                        {
                            int lineStartX = (_circleSize / 2) + (i * stepWidth) - (stepWidth / 2);
                            int lineEndX = (i * stepWidth) - (stepWidth / 2) + stepWidth;
                            g.DrawLine(pen, lineStartX, circleY + (_circleSize / 2), lineEndX, circleY + (_circleSize / 2));
                        }
                    }

                    // Dibujar círculo
                    g.FillEllipse(
                        i == _indexStep ? activeBrush :
                        i < _indexStep ? completedBrush : inactiveBrush,
                        circleX, circleY, _circleSize, _circleSize);

                    // Dibujar número - usamos boldFont solo para el paso actual
                    var numberText = (i + 1).ToString();
                    var currentNumberFont = i == _indexStep ? boldFont : regularNumberFont;
                    var numberSize = g.MeasureString(numberText, currentNumberFont);
                    g.DrawString(numberText, currentNumberFont, whiteTextBrush,
                        circleX + (_circleSize - numberSize.Width) / 2,
                        circleY + (_circleSize - numberSize.Height) / 2);

                    // Dibujar título
                    var titleFont = i == _indexStep ? boldFont :(i <= _indexStep ? boldSmallFont : regularFont);
                    var titleBrush = i <= _indexStep ? activeTextBrush : inactiveTextBrush;
                    var titleSize = g.MeasureString(step.Title, titleFont);
                    g.DrawString(step.Title, titleFont, titleBrush,
                        (i * stepWidth) + (stepWidth - titleSize.Width) / 2,
                        circleY + _circleSize + 5);
                }
            }
        }

        #endregion

        #region Clases auxiliares

        // Clase para los argumentos del evento que permite cancelar
        public class StepChangingEventArgs : EventArgs
        {
            public int CurrentIndex { get; }
            public int NewIndex { get; }
            public bool Cancel { get; set; }

            public StepChangingEventArgs(int currentIndex, int newIndex)
            {
                CurrentIndex = currentIndex;
                NewIndex = newIndex;
                Cancel = false;
            }
        }

        [Designer(typeof(StepPageDesigner))]
        [ToolboxItem(false)]
        public class StepPage : ContainerControl
        {
            private string _title = "Step";
            private Color _borderColor = Color.Gray;
            private int _borderRadius = 10;
            private int _borderThickness = 2;

            [Browsable(true)]
            [Category("Appearance")]
            [Description("Title of the step")]
            public string Title
            {
                get => _title;
                set { _title = value; Invalidate(); }
            }

            [Browsable(true)]
            [Category("Appearance")]
            [Description("Color of the border")]
            public Color BorderColor
            {
                get => _borderColor;
                set { _borderColor = value; Invalidate(); }
            }

            [Browsable(true)]
            [Category("Appearance")]
            [Description("Radius of the border corners")]
            public int BorderRadius
            {
                get => _borderRadius;
                set { _borderRadius = Math.Max(0, value); Invalidate(); }
            }

            [Browsable(true)]
            [Category("Appearance")]
            [Description("Thickness of the border")]
            public int BorderThickness
            {
                get => _borderThickness;
                set { _borderThickness = Math.Max(1, value); Invalidate(); }
            }

            public StepPage()
            {
                this.BackColor = SystemColors.Control;
                this.Dock = DockStyle.Fill;
                this.AutoScroll = true;
                this.Padding = new Padding(10);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                DrawRoundedBorder(e.Graphics);
            }

            private void DrawRoundedBorder(Graphics g)
            {
                using (Pen borderPen = new Pen(_borderColor, _borderThickness))
                using (GraphicsPath path = GetRoundedRectanglePath(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawPath(borderPen, path);
                }
            }

            private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
            {
                GraphicsPath path = new GraphicsPath();
                int diameter = radius * 2;

                if (radius > 0)
                {
                    path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                    path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                    path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                    path.CloseFigure();
                }
                else
                {
                    path.AddRectangle(rect);
                }

                return path;
            }
        }


        public class StepCollection : System.Collections.CollectionBase
        {
            private readonly StepProgressControl _owner;

            public StepCollection(StepProgressControl owner)
            {
                _owner = owner;
            }

            public StepPage this[int index] => (StepPage)List[index];

            public void Add(StepPage page)
            {
                List.Add(page);
                _owner.Controls.Add(page);
                if (_owner.Steps.Count > 0 && _owner.IndexStep == -1) {
                    _owner.IndexStep = 0;   
                }

                if (!page.Site?.DesignMode ?? true)
                    _owner.UpdateDesigner();
            }

            public void Remove(StepPage page)
            {
                List.Remove(page);
                _owner.Controls.Remove(page);
                if (!page.Site?.DesignMode ?? true)
                    _owner.UpdateDesigner();
            }
            public int IndexOf(StepPage page)
            {
                return List.IndexOf(page);  // 🔥 Agregado para solucionar el error
            }

            protected override void OnInsertComplete(int index, object value)
            {
                base.OnInsertComplete(index, value);
                _owner.Controls.Add((Control)value);
                _owner.UpdateDesigner();
            }

            protected override void OnRemoveComplete(int index, object value)
            {
                base.OnRemoveComplete(index, value);
                _owner.Controls.Remove((Control)value);
                _owner.UpdateDesigner();
            }
        }

        #endregion
   
    }

    #region Designer para el control

    internal class StepProgressControlDesigner : ParentControlDesigner
    {
        private IMenuCommandService _menuService;
        private ISelectionService _selectionService;
        private DesignerVerbCollection _verbs;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            // Obtener servicios necesarios
            _menuService = GetService(typeof(IMenuCommandService)) as IMenuCommandService;
            _selectionService = GetService(typeof(ISelectionService)) as ISelectionService;

            // Habilitar diseño para el panel de contenido
            if (component is StepProgressControl control)
            {
                EnableDesignMode(control.ContentPanel, "ContentPanel");
            }

            // Inicializar la colección de verbos
            _verbs = new DesignerVerbCollection();
            RefreshVerbs();
        }

        private void RefreshVerbs()
        {
            _verbs.Clear();

            if (!(Component is StepProgressControl control)) return;

            // Comando para agregar nuevo step
            _verbs.Add(new DesignerVerb("Agregar Step", (s, e) => AddNewStep(control)));

            // Comandos para cada step existente
            for (int i = 0; i < control.Steps.Count; i++)
            {
                var step = control.Steps[i];
                _verbs.Add(new DesignerVerb($"Seleccionar Step {i + 1}: {step.Title}",
                    (s, e) => SelectStep(control, step)));
            }
        }

        public override DesignerVerbCollection Verbs => _verbs;

        private void AddNewStep(StepProgressControl control)
        {
            var host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (host == null) return;

            using (var transaction = host.CreateTransaction("Agregar Step"))
            {
                var newStep = (StepPage)host.CreateComponent(typeof(StepPage));
                newStep.Title = $"Step {control.Steps.Count + 1}";
                newStep.Name = $"stepPage{control.Steps.Count + 1}";

                control.Steps.Add(newStep);

                SelectStep(control, newStep);

                RefreshVerbs();

                transaction.Commit();
            }
        }


        private void SelectStep(StepProgressControl control, StepPage step)
        {
            control.CurrentStep = step;
            _selectionService?.SetSelectedComponents(new IComponent[] { control, step },
                SelectionTypes.Replace);

            // Forzar actualización del diseñador
            Control.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_verbs != null)
                {
                    _verbs.Clear();
                    _verbs = null;
                }
            }
            base.Dispose(disposing);
        }
    }




    internal class StepPageDesigner : ParentControlDesigner
    {
        private ISelectionService _selectionService;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            var stepPage = (StepPage)component;
            EnableDesignMode(stepPage, "StepPage");
            _selectionService = (ISelectionService)GetService(typeof(ISelectionService));

            // Suscribir al evento MouseDown del control
            this.Control.MouseDown += StepPage_MouseDown;
            // Habilitar diseño para el StepPage
            EnableDesignMode((Control)component, "StepPage");
        }

        protected override void OnMouseDragBegin(int x, int y)
        {
            // Evitar que se mueva el StepPage independientemente
            // (debe moverse solo a través del StepProgressControl)
            return;
        }

        private void StepPage_MouseDown(object sender, MouseEventArgs e)
        {
            // Seleccionar tanto el StepProgressControl como el StepPage
            if (_selectionService != null)
            {
                var parentControl = Control.Parent as StepProgressControl;
                if (parentControl != null)
                {
                    // Crear array de IComponent
                    IComponent[] components = new IComponent[] { parentControl, Component };
                    _selectionService.SetSelectedComponents(components, SelectionTypes.Add);
                }
            }
        }

        public override bool CanBeParentedTo(IDesigner parentDesigner)
        {
            // Solo permitir que StepPage sea hijo de StepProgressControl
            return parentDesigner.Component is StepProgressControl;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Limpiar el evento
                if (this.Control != null)
                {
                    this.Control.MouseDown -= StepPage_MouseDown;
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            base.OnPaintAdornments(pe);

            bool isSelected = _selectionService != null &&
                             _selectionService.GetComponentSelected(Component);

            var borderColor = isSelected ? Color.DodgerBlue : SystemColors.ControlDark;
            using (var pen = new Pen(borderColor, isSelected ? 2 : 1))
            {
                var rect = Control.ClientRectangle;
                rect.Inflate(-1, -1);
                pe.Graphics.DrawRectangle(pen, rect);
            }
        }
    }

    public class StepPageSelectorEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context?.Instance is StepProgressControl control && provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService editorService)
            {
                var listBox = new ListBox { BorderStyle = BorderStyle.None };
                foreach (StepPage step in control.Steps)
                {
                    listBox.Items.Add(step);
                }

                listBox.SelectedItem = value;
                listBox.Click += (s, e) => editorService.CloseDropDown();

                editorService.DropDownControl(listBox);

                return listBox.SelectedItem;
            }

            return value;
        }
    }

    #endregion

    #endregion
}