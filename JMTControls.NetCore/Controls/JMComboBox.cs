

namespace JMTControls.NetCore.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Windows.Forms;

    [DefaultEvent("SelectedIndexChanged")]
    public partial class JMComboBox : UserControl
    {

        private object dataSource;
        private string displayMember;
        private string valueMember;
        private List<object> boundItems = new List<object>();
        private Font _font = new Font("Tahoma", 12);
        private Color _backColor = Color.White;
        private Color _foreColor = Color.DarkViolet;
        private Color _borderColor = Color.DarkViolet;
        private BorderStyle _borderStyle = BorderStyle.FixedSingle;
        private Color iconColor = Color.MediumSlateBlue;
        private int _selectedIndex = -1;
        private bool _notExecuteTextChange;
        private bool _dropdownMenu;
        private bool _alreadyZizing;
        private int _maxItemsVisible = 12;
        private int _borderSize = 1;

        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;

        public JMComboBox()
        {
            // Inicializa los componentes
            InitializeComponent();

        }


        public object DataSource
        {
            get => dataSource;
            set
            {
                dataSource = value;
                BindData(); // Actualiza los datos al cambiar la fuente
            }
        }

        public string DisplayMember
        {
            get => displayMember;
            set
            {
                displayMember = value;
                lstItems.DisplayMember = displayMember; // Actualiza DisplayMember en lstItems
                BindData(); // Llama a BindData para refrescar la lista
            }
        }

        public string ValueMember
        {
            get => valueMember;
            set
            {
                valueMember = value;
                lstItems.ValueMember = valueMember; // Actualiza ValueMember en lstItems
                BindData(); // Llama a BindData para refrescar la lista
            }
        }

        private void BindData()
        {
            if (dataSource is IEnumerable<object> enumerable)
            {
                boundItems = enumerable.ToList();
                lstItems.Items.Clear(); // Limpia los items anteriores antes de agregar los nuevos

                // Agrega los elementos filtrados o los elementos completos
                lstItems.Items.AddRange(boundItems.ToArray());
            }
        }



        // border color 
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    base.BackColor = _borderColor;
                    Invalidate();
                }
            }
        }


        // genera la propiedad font
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        public new Font Font
        {
            get => _font;
            set
            {
                if (_font != value)
                {
                    _font = value;
                    txtSearch.Font = _font;
                    lstItems.Font = _font;
                    btnAction.Font = _font;
                    dropdownMenu.Font = _font;
                    Invalidate();
                }
            }
        }

        // genera la propiedad backColor
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        public new Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    panel.BackColor = _backColor;
                    txtSearch.BackColor = _backColor;
                    lstItems.BackColor = _backColor;
                    btnAction.BackColor = _backColor;
                    btnIcon.BackColor = _backColor;
                    dropdownMenu.BackColor = _backColor;
                    Invalidate();
                }
            }
        }




        // gebera para estilo de borde 
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        public new Color ForeColor
        {
            get => _foreColor;
            set
            {
                if (_foreColor != value)
                {
                    _foreColor = value;
                    txtSearch.ForeColor = _foreColor;
                    lstItems.ForeColor = _foreColor;
                    btnAction.ForeColor = _foreColor;
                    dropdownMenu.ForeColor = _foreColor;
                    btnIcon.ForeColor = _foreColor;
                    Invalidate();
                }

            }
        }
        // Boder style
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        public new BorderStyle BorderStyle
        {
            get => _borderStyle;
            set
            {
                if (_borderStyle != value)
                {
                    _borderStyle = value;

                    if (_borderStyle == BorderStyle.None)
                        base.Padding = new Padding(0);
                    else if (_borderStyle == BorderStyle.FixedSingle)
                        base.Padding = new Padding(1);
                    else if (_borderStyle == BorderStyle.Fixed3D)
                        base.Padding = new Padding(2);
                }

            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        public Image ButtonImage
        {
            get => btnAction.Image;

            set
            {
                btnAction.Image = value;
                Invalidate();
            }
        }


        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        public Color IconColor
        {
            get
            {
                return iconColor;
            }
            set
            {
                iconColor = value;
                btnIcon.Invalidate();
            }
        }


        public bool DroppedDown
        {
            get => _dropdownMenu;
            set
            {
                if (dropdownMenu.Visible != value)
                {
                    if (value)
                    {
                        _dropdownMenu = true;
                        dropdownMenu.Show(this, new Point(0, Height));
                    }
                    else
                    {
                        _dropdownMenu = false;
                        dropdownMenu.Hide();
                        dropdownMenu.Close();
                    }
                }

                if (dropdownMenu.Visible)
                {
                    DropdownMenu_Opening(dropdownMenu, new CancelEventArgs());
                }

            }
        }

        [Browsable(false)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;

                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                    SelectedValueChanged?.Invoke(this, EventArgs.Empty);
                }

            }

        }


        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Category("Action")]
        [Description("Occurs when the button  is clicked.")]
        public event EventHandler ButtonOptionClick
        {
            add
            {
                btnAction.Click += value;
            }
            remove
            {
                btnAction.Click -= value;
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool VisibleButtonOption
        {
            get => btnAction.Visible;

            set
            {
                if (btnAction.Visible != value)
                {
                    btnAction.Visible = value;
                    Invalidate();
                }
            }
        }

        public object SelectedValue
        {
            get
            {
                if (_selectedIndex >= 0 && _selectedIndex < boundItems.Count)
                {
                    var selectedItem = boundItems[_selectedIndex];
                    // Obtener el valor de la propiedad especificada en ValueMember
                    return selectedItem?.GetType().GetProperty(valueMember)?.GetValue(selectedItem, null);
                }
                return null;
            }
            set
            {
                // Buscar el valor en boundItems utilizando ValueMember
                var selectedItem = boundItems
                    .FirstOrDefault(item =>
                        item.GetType().GetProperty(valueMember)?.GetValue(item, null)?.Equals(value) == true);

                if (selectedItem != null)
                {
                    // Asignar el objeto encontrado al SelectedItem
                    _selectedIndex = boundItems.IndexOf(selectedItem);

                    _notExecuteTextChange = true;
                    this.Text = selectedItem.GetType().GetProperty(displayMember)?.GetValue(selectedItem, null)?.ToString();

                }
            }
        }



        public object SelectedItem
        {
            get
            {
                if (_selectedIndex >= 0)
                {
                    return boundItems[_selectedIndex];
                }
                return null;
            }
            set
            {
                if (value != null && boundItems.Contains(value))
                {
                    _selectedIndex = boundItems.IndexOf(value);
                    _notExecuteTextChange = true;
                    this.Text = value.GetType().GetProperty(displayMember)?.GetValue(value, null)?.ToString();
                }
                else
                {
                    _selectedIndex = -1;
                    this.Text = string.Empty;
                }
            }
        }



        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [DefaultValue("")]
        public new string Text
        {
            get => txtSearch.Text;
            set => txtSearch.Text = value;
        }


        // _maxItemsVisible 
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Behavior")]
        [DefaultValue(12)]
        public int MaxItemsVisible
        {
            get => _maxItemsVisible;
            set
            {
                if (_maxItemsVisible != value)
                {
                    _maxItemsVisible = value;
                    AdjustDropdownSize();
                }
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        public int BorderThickness
        {
            get => _borderSize; 
            set
            {
                if (_borderSize != value) {
                    _borderSize = value;
                    base.Padding = new Padding(_borderSize);
                    Invalidate();
                }
            }
        }



        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_notExecuteTextChange)
            {
                _notExecuteTextChange = false;
                return;
            }

            string searchText = txtSearch.Text.ToLower();
            var filteredItems = boundItems
              .Where(item => item.GetType().GetProperty(displayMember)?
                  .GetValue(item, null)?.ToString()?.ToLower().Contains(searchText) == true)
              .ToList();

            lstItems.Items.Clear();
            if (filteredItems.Any())
            {
                lstItems.Items.AddRange(filteredItems.ToArray());
                DroppedDown = true;
            }
            else
            {
                SelectedIndex = -1;
                DroppedDown = false;
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lstItems.Items.Count > 0)
            {
                lstItems.Focus();
                lstItems.SelectedIndex = 0;
            } 
        }
        private void TxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                DroppedDown = false;    
            }
            else if (e.KeyChar == (char)Keys.F4)
            {
                btnIcon.PerformClick();  
            }
        }

        private void TxtSearch_LostFocus(object sender, EventArgs e)
        {
            DroppedDown = false;
            dropdownMenu.Close();

        }


        private void LstItems_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedIndex >= 0)
            {
                SetObject();
            }
        }

        private void LstItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstItems.SelectedIndex >= 0)
            {
                SetObject();
            } 
            
        }

        private void SetObject()
        {
            var selectedFilteredItem = lstItems.SelectedItem; // Objeto seleccionado en la lista filtrada

            // Obtener el valor real de ValueMember del elemento seleccionado
            var selectedValue = selectedFilteredItem.GetType().GetProperty(valueMember)?.GetValue(selectedFilteredItem, null);

            // Buscar el índice real en boundItems usando ValueMember
            _selectedIndex = boundItems.FindIndex(item =>
                item.GetType().GetProperty(valueMember)?.GetValue(item, null)?.Equals(selectedValue) == true
            );


            _notExecuteTextChange = true;
            this.Text = lstItems.Text;

            DroppedDown = false;
            dropdownMenu.Close();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            SelectedValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(sender, e);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            // veticalmente centra el txtSearch 
            // y horizontalmente que ocupe  todo el ancho del control - el ancho del botón
            // limita el alto minimo de este control sumando el alto de txtSearch + 3 pixeles arriba y abajo
            var size = TextRenderer.MeasureText("A", _font);
            this.MinimumSize = new Size { Height = size.Height + 6, Width = 60 };

            txtSearch.Width = this.Width - (btnAction.Width + btnIcon.Width + 10);
            txtSearch.Location = new Point(5, (int)((Height - txtSearch.Height) / 2));

            dropdownMenu.Width = txtSearch.Width;

            base.OnSizeChanged(e);
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DroppedDown = false;
                return true; // Indicar que la tecla fue manejada
            }
            else if (keyData == Keys.F4)
            {
                btnIcon.PerformClick();
                return true; // Indicar que la tecla fue manejada
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Icon_Click(object sender, EventArgs e)
        {
            if (!DroppedDown)
            {

                lstItems.Items.Clear();
                lstItems.Items.AddRange(boundItems.ToArray());
                lstItems.SelectedIndex = _selectedIndex;
                // Asegurarse de que el ítem seleccionado sea visible
                if (_selectedIndex >= 0 && _selectedIndex < lstItems.Items.Count)
                {
                    lstItems.TopIndex = _selectedIndex;
                }
                DroppedDown = true;
            }

        }

        private void Icon_Paint(object sender, PaintEventArgs e)
        {
            //Fields
            int iconWidht = 14;
            int iconHeight = 6;
            var rectIcon = new Rectangle((btnIcon.Width - iconWidht) / 2, (btnIcon.Height - iconHeight) / 2, iconWidht, iconHeight);
            Graphics graph = e.Graphics;

            //Draw arrow down icon
            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(iconColor, 2))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                path.AddLine(rectIcon.X, rectIcon.Y, rectIcon.X + (iconWidht / 2), rectIcon.Bottom);
                path.AddLine(rectIcon.X + (iconWidht / 2), rectIcon.Bottom, rectIcon.Right, rectIcon.Y);
                graph.DrawPath(pen, path);
            }
        }

        private void DropdownMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                AdjustDropdownSize();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Manejo de la excepción: puedes registrar el error o manejarlo adecuadamente
                Console.WriteLine($"Error al ajustar el tamaño del menú: {ex.Message}");
            }
        }


        private void AdjustDropdownSize()
        {
            if (_alreadyZizing)
                return;

            _alreadyZizing = true;

            // Número de elementos en la lista
            int visibleItemsCount = lstItems.Items.Count;
            int itemHeight = lstItems.ItemHeight;

            // Calcular la nueva altura sin superar el límite
            int newHeight = Math.Min(visibleItemsCount, _maxItemsVisible) * itemHeight;
            newHeight = Math.Min(newHeight, 190); // Limitar a 190px máximo


            // Obtener el texto más largo basado en DisplayMember
            string longestText = lstItems.Items
                .Cast<object>()
                .Select(item => item.GetType().GetProperty(displayMember)?.GetValue(item, null)?.ToString() ?? "")
                .OrderByDescending(text => text.Length)
                .FirstOrDefault() ?? "Placeholder";

            // Calcular el ancho
            int scrollbarWidth = SystemInformation.VerticalScrollBarWidth; // Ancho del scroll
            int textPadding = 20; // Espacio extra para evitar cortes
            int newWidth = TextRenderer.MeasureText(longestText, lstItems.Font).Width + textPadding;

            // Asegurar un ancho mínimo
            newWidth = Math.Min(newWidth, (txtSearch.Width - scrollbarWidth));


            // Asegurar que el ListBox tenga el tamaño correcto
            lstItems.Height = newHeight;
            lstItems.IntegralHeight = false; // Para que respete exactamente el alto
            lstItems.ScrollAlwaysVisible = visibleItemsCount > _maxItemsVisible;

            lstItems.Width = newWidth;

            // Desactivar el ajuste automático del tamaño del menú
            dropdownMenu.AutoSize = false;
            // Si el ContextMenuStrip ya tiene un host, ajustar su tamaño
            if (dropdownMenu.Items[0] is ToolStripControlHost host)
            {
                host.AutoSize = false;
                host.Size = new Size(newWidth + 10, newHeight);
            }

            dropdownMenu.Size = new Size(newWidth + scrollbarWidth + 30, newHeight + 4); // +4 para márgenes

            // Mostrar menú si hay elementos
            if (visibleItemsCount > 0)
            {
                if (!dropdownMenu.Visible)
                {
                    dropdownMenu.Show(txtSearch, new Point(0, txtSearch.Height));
                }
            }
            else
            {
                dropdownMenu.Hide();
            }

            _alreadyZizing = false;
        }


    }

}
