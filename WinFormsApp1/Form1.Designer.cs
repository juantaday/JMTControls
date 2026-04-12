namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            JMTControls.NetCore.Controls.SidebarTab sidebarTab1 = new JMTControls.NetCore.Controls.SidebarTab();
            JMTControls.NetCore.Controls.SidebarGroupModel sidebarGroupModel1 = new JMTControls.NetCore.Controls.SidebarGroupModel();
            JMTControls.NetCore.Controls.SidebarItemModel sidebarItemModel1 = new JMTControls.NetCore.Controls.SidebarItemModel();
            JMTControls.NetCore.Controls.SidebarItemModel sidebarItemModel2 = new JMTControls.NetCore.Controls.SidebarItemModel();
            JMTControls.NetCore.Controls.SidebarGroupModel sidebarGroupModel2 = new JMTControls.NetCore.Controls.SidebarGroupModel();
            JMTControls.NetCore.Controls.SidebarTab sidebarTab2 = new JMTControls.NetCore.Controls.SidebarTab();
            JMTControls.NetCore.Controls.SidebarGroupModel sidebarGroupModel3 = new JMTControls.NetCore.Controls.SidebarGroupModel();
            JMTControls.NetCore.Controls.SidebarGroupModel sidebarGroupModel4 = new JMTControls.NetCore.Controls.SidebarGroupModel();
            sidebarContainer2 = new JMTControls.NetCore.Controls.SidebarContainer();
            SuspendLayout();
            // 
            // sidebarContainer2
            // 
            sidebarContainer2.BackColor = Color.FromArgb(30, 30, 38);
            sidebarContainer2.CollapsedWidth = 52;
            sidebarContainer2.ExpandedWidth = 280;
            sidebarContainer2.Location = new Point(12, 31);
            sidebarContainer2.Name = "sidebarContainer2";
            sidebarContainer2.Size = new Size(248, 411);
            sidebarContainer2.TabIndex = 0;
            sidebarTab1.BackColor = Color.Transparent;
            sidebarItemModel1.Font = new Font("Segoe UI", 9F);
            sidebarItemModel1.HoverBackColor = Color.FromArgb(50, 50, 65);
            sidebarItemModel1.HoverForeColor = Color.White;
            sidebarItemModel1.HoverShortcutColor = Color.FromArgb(130, 135, 155);
            sidebarItemModel1.Icon = null;
            sidebarItemModel1.Name = "sidebarItem1";
            sidebarItemModel1.NormalBackColor = Color.Transparent;
            sidebarItemModel1.NormalForeColor = Color.FromArgb(195, 195, 210);
            sidebarItemModel1.NormalShortcutColor = Color.FromArgb(90, 95, 115);
            sidebarItemModel1.Shortcut = "";
            sidebarItemModel1.Text = "Nueva Venta ";
            sidebarItemModel2.Font = new Font("Segoe UI", 9F);
            sidebarItemModel2.HoverBackColor = Color.FromArgb(50, 50, 65);
            sidebarItemModel2.HoverForeColor = Color.White;
            sidebarItemModel2.HoverShortcutColor = Color.FromArgb(130, 135, 155);
            sidebarItemModel2.Icon = null;
            sidebarItemModel2.Name = "sidebarItem2";
            sidebarItemModel2.NormalBackColor = Color.Transparent;
            sidebarItemModel2.NormalForeColor = Color.FromArgb(195, 195, 210);
            sidebarItemModel2.NormalShortcutColor = Color.FromArgb(90, 95, 115);
            sidebarItemModel2.Shortcut = "";
            sidebarItemModel2.Text = "Lista de productos";
            sidebarGroupModel1.Items.Add(sidebarItemModel1);
            sidebarGroupModel1.Items.Add(sidebarItemModel2);
            sidebarGroupModel1.Name = "sidebarGroup1";
            sidebarGroupModel1.Title = "Venta";
            sidebarGroupModel2.Name = "sidebarGroup2";
            sidebarGroupModel2.Title = "Producto";
            sidebarTab1.Groups.Add(sidebarGroupModel1);
            sidebarTab1.Groups.Add(sidebarGroupModel2);
            sidebarTab1.Icon = Properties.Resources.SalesMy_32_png;
            sidebarTab1.Name = "sidebarTab3";
            sidebarTab1.Title = "Venta";
            sidebarTab2.BackColor = Color.Transparent;
            sidebarGroupModel3.Name = "sidebarGroup4";
            sidebarGroupModel3.Title = "Nuevo Grupo";
            sidebarGroupModel4.Name = "sidebarGroup5";
            sidebarGroupModel4.Title = "Nuevo Grupo";
            sidebarTab2.Groups.Add(sidebarGroupModel3);
            sidebarTab2.Groups.Add(sidebarGroupModel4);
            sidebarTab2.Icon = null;
            sidebarTab2.Name = "sidebarTab1";
            sidebarTab2.Title = "Estadistica";
            sidebarContainer2.Tabs.Add(sidebarTab1);
            sidebarContainer2.Tabs.Add(sidebarTab2);
            sidebarContainer2.ItemClicked += sidebarContainer1_ItemClicked;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(521, 469);
            Controls.Add(sidebarContainer2);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private JMTControls.NetCore.Controls.SidebarContainer sidebarContainer1;
        private JMTControls.NetCore.Controls.SidebarContainer sidebarContainer2;
    }
}