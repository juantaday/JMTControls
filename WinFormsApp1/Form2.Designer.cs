
namespace WinFormsApp1
{
    partial class Form2
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
            components = new System.ComponentModel.Container();
            button2 = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            holaSfgdsfgfdghfToolStripMenuItem = new ToolStripMenuItem();
            sadfwertreytrutydfffffToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button2
            // 
            button2.ContextMenuStrip = contextMenuStrip1;
            button2.Location = new Point(376, 168);
            button2.Name = "button2";
            button2.Size = new Size(178, 58);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { holaSfgdsfgfdghfToolStripMenuItem, sadfwertreytrutydfffffToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(188, 48);
            // 
            // holaSfgdsfgfdghfToolStripMenuItem
            // 
            holaSfgdsfgfdghfToolStripMenuItem.Name = "holaSfgdsfgfdghfToolStripMenuItem";
            holaSfgdsfgfdghfToolStripMenuItem.Size = new Size(187, 22);
            holaSfgdsfgfdghfToolStripMenuItem.Text = "hola sfgdsfgfdghf";
            // 
            // sadfwertreytrutydfffffToolStripMenuItem
            // 
            sadfwertreytrutydfffffToolStripMenuItem.Name = "sadfwertreytrutydfffffToolStripMenuItem";
            sadfwertreytrutydfffffToolStripMenuItem.Size = new Size(187, 22);
            sadfwertreytrutydfffffToolStripMenuItem.Text = "sadfwertreytrutydfffff";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(button2);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button button2;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem holaSfgdsfgfdghfToolStripMenuItem;
        private ToolStripMenuItem sadfwertreytrutydfffffToolStripMenuItem;
    }
}