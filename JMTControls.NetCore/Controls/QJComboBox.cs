using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMTControls.NetCore.Controls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    [DefaultEvent("SelectedIndexChanged")]
    public class QRJComboBox : UserControl
    {
        // Componentes internos del control
        private TextBox textBox;
        private ListBox listBox;
        private Button actionButton;
        private Panel dropdownPanel;
        private BindingList<object> dataSource;

        // Eventos
        public event EventHandler SelectedIndexChanged;
        public event EventHandler ActionButtonClicked;

        // Propiedades públicas
        [Browsable(true)]
        [Category("Data")]
        public object DataSource
        {
            get => dataSource;
            set
            {
                if (value is IEnumerable enumerable)
                {
                    dataSource = new BindingList<object>(enumerable.Cast<object>().ToList());
                    UpdateListBox();
                }
               
            }
        }

        [Browsable(true)]
        [Category("Data")]
        public string DisplayMember { get; set; }

        [Browsable(true)]
        [Category("Data")]
        public string ValueMember { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        public object SelectedItem
        {
            get => listBox.SelectedItem;
            set
            {
                listBox.SelectedItem = value;
                textBox.Text = GetDisplayValue(value);
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public int SelectedIndex
        {
            get => listBox.SelectedIndex;
            set
            {
                listBox.SelectedIndex = value;
                textBox.Text = GetDisplayValue(listBox.SelectedItem);
            }
        }

        // Constructor
        public QRJComboBox()
        {
            InitializeComponent();
        }

        // Inicialización de componentes
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // TextBox para entrada de texto
            textBox = new TextBox
            {
                Location = new Point(0, 0),
                Width = this.Width - 30,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            textBox.TextChanged += TextBox_TextChanged;
            textBox.KeyDown += TextBox_KeyDown;

            // Botón de acción
            actionButton = new Button
            {
                Text = "...",
                Location = new Point(this.Width - 28, 0),
                Size = new Size(25, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            actionButton.Click += ActionButton_Click;

            // Panel desplegable
            dropdownPanel = new Panel
            {
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, textBox.Height),
                Width = this.Width,
                Height = 100,
                AutoScroll = true
            };

            // ListBox para mostrar opciones
            listBox = new ListBox
            {
                Dock = DockStyle.Fill
            };
            listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
            dropdownPanel.Controls.Add(listBox);

            // Agregar controles al UserControl
            this.Controls.Add(textBox);
            this.Controls.Add(actionButton);
            this.Controls.Add(dropdownPanel);

            this.Size = new Size(200, 25);
            this.ResumeLayout(false);
        }

        // Manejadores de eventos
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            FilterListBox();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && listBox.Items.Count > 0)
            {
                listBox.Focus();
                listBox.SelectedIndex = 0;
            }
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                textBox.Text = GetDisplayValue(listBox.SelectedItem);
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                HideDropdown();
            }
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            ActionButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        // Métodos auxiliares
        private void FilterListBox()
        {
            if (dataSource == null) return;

            var filterText = textBox.Text.ToLower();
            var filteredItems = dataSource.Where(item =>
            {
                var displayValue = GetDisplayValue(item)?.ToLower();
                return displayValue != null && displayValue.Contains(filterText);
            }).ToList();

            listBox.DataSource = filteredItems;
            ShowDropdown();
        }

        private void UpdateListBox()
        {
            listBox.DataSource = dataSource;
            listBox.DisplayMember = DisplayMember;
            listBox.ValueMember = ValueMember;
        }

        private string GetDisplayValue(object item)
        {
            if (item == null) return string.Empty;

            if (!string.IsNullOrEmpty(DisplayMember))
            {
                var property = item.GetType().GetProperty(DisplayMember);
                return property?.GetValue(item)?.ToString() ?? string.Empty;
            }

            return item.ToString();
        }

        private void ShowDropdown()
        {
            dropdownPanel.Visible = true;
            dropdownPanel.BringToFront();
        }

        private void HideDropdown()
        {
            dropdownPanel.Visible = false;
        }

        // Sobrescritura de métodos
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            textBox.Width = this.Width - 30;
            actionButton.Location = new Point(this.Width - 28, 0);
            dropdownPanel.Width = this.Width;
        }
    }

}
