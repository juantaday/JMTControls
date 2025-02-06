namespace JMTControls.NetCore.Controls
{
    using System.Drawing;
    using System.Windows.Forms;

    public partial class JMComboBox
    {

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        { 
            base.BackColor  = _borderColor;
            base.BorderStyle = System.Windows.Forms.BorderStyle.None;   
            base.ForeColor = _foreColor;
            base.Padding = new System.Windows.Forms.Padding(1); 

            panel = new Panel();
            btnIcon = new Button();
            btnAction = new Button();
            lstItems = new ListBox();
            //panel

            panel.Dock = DockStyle.Fill;
            panel.BackColor = _backColor;
            panel.BorderStyle = BorderStyle.None;
            panel.Padding = new Padding(0);
            panel.Margin = new Padding(0);
            panel.ForeColor = _foreColor;

            //BUTTON ICON
            btnIcon.Dock = DockStyle.Right;
            btnIcon.FlatStyle = FlatStyle.Flat;
            btnIcon.FlatAppearance.BorderSize = 0;
            btnIcon.BackColor = _backColor;
            btnIcon.ForeColor = _foreColor;
            btnIcon.Size = new Size(30, 30);
            btnIcon.Cursor = Cursors.Hand;
            btnIcon.Click += Icon_Click;
            btnIcon.Paint += Icon_Paint;

            // Botón de acción
            btnAction.Dock = DockStyle.Right;
            btnAction.FlatStyle = FlatStyle.Flat;
            btnAction.ForeColor = _foreColor;
            btnAction.FlatAppearance.BorderSize = 0;
            btnAction.BackColor = _backColor;
            btnAction.Size = new Size(30, 30);
            btnAction.Cursor = Cursors.Hand;
            btnAction.Click += BtnAction_Click;


            this.Leave += TxtSearch_LostFocus;
            this.Padding = new System.Windows.Forms.Padding(1);


            // TextBox para ingresar texto
            txtSearch = new TextBox
            {
                Multiline = false,
                Dock = DockStyle.None,
                Font = _font,
                BorderStyle = BorderStyle.None,
                BackColor = _backColor,
                ForeColor = _foreColor, 
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.KeyDown += TxtSearch_KeyDown;


            // ListBox para desplegar opciones
            lstItems.IntegralHeight = true;
            lstItems.MaximumSize = new Size(600, 350);
            lstItems.ScrollAlwaysVisible = true;    
            lstItems.Margin = new Padding (0);
            lstItems.BackColor = _backColor;
            lstItems.BorderStyle = BorderStyle.None;
            lstItems.Font = _font;
            lstItems.ForeColor = _foreColor;
            lstItems.Click += LstItems_Click;
            lstItems.KeyDown += LstItems_KeyDown;
            //lstItems.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            // Menú contextual para simular el desplegable
            dropdownMenu = new ContextMenuStrip
            {
                AutoClose = false,
                BackColor = _backColor,
                Font = _font,
                Padding = new Padding(0),
                Margin = new Padding(0),
                ForeColor = _foreColor,
                MaximumSize = new Size(600, 400)
            };
            dropdownMenu.Items.Add(new ToolStripControlHost(lstItems));
            dropdownMenu.Opening += DropdownMenu_Opening;


            // Estructura del control
            panel.Controls.Add(txtSearch);
            panel.Controls.Add(btnIcon);
            panel.Controls.Add(btnAction);
            Controls.Add(panel);

        }

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ListBox lstItems;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.ContextMenuStrip dropdownMenu;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button btnIcon;

    }

}
