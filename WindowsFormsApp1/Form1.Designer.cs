namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.expandCollapsePanel1 = new JMControls.ExpandCollapsePanel.ExpandCollapsePanel();
            this.SuspendLayout();
            // 
            // expandCollapsePanel1
            // 
            this.expandCollapsePanel1.AlignmentIcon = JMControls.ExpandCollapsePanel.ExpandCollapseButton.ExpandIconAlignment.Right;
            this.expandCollapsePanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.expandCollapsePanel1.BottonTitleLocation = new System.Drawing.Point(55, 10);
            this.expandCollapsePanel1.ButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.expandCollapsePanel1.ButtonBackColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.expandCollapsePanel1.ButtonImage = null;
            this.expandCollapsePanel1.ButtonImageLocation = new System.Drawing.Point(5, 3);
            this.expandCollapsePanel1.ButtonImageSize = new System.Drawing.Size(48, 32);
            this.expandCollapsePanel1.ButtonLogoSize = JMControls.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonSize.Normal;
            this.expandCollapsePanel1.ButtonLogoStyle = JMControls.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Arrow;
            this.expandCollapsePanel1.ExpandedHeight = 0;
            this.expandCollapsePanel1.IsExpanded = true;
            this.expandCollapsePanel1.Location = new System.Drawing.Point(91, 78);
            this.expandCollapsePanel1.Name = "expandCollapsePanel1";
            this.expandCollapsePanel1.Size = new System.Drawing.Size(441, 267);
            this.expandCollapsePanel1.TabIndex = 0;
            this.expandCollapsePanel1.Text = "expandCollapsePanel1";
            this.expandCollapsePanel1.UseAnimation = true;
            this.expandCollapsePanel1.VisibleDefaultButton = true;
            this.expandCollapsePanel1.VisibleIconButton = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.expandCollapsePanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private JMControls.ExpandCollapsePanel.ExpandCollapsePanel expandCollapsePanel1;
    }
}

