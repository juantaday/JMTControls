namespace JMControls.ExpandCollapsePanel
{
    partial class AccordionCotrol
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._btnExpandCollapse = new JMControls.ExpandCollapsePanel.ExpandCollapseButton();
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.panelHeader = new System.Windows.Forms.Panel();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnExpandCollapse
            // 
            this._btnExpandCollapse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(65)))), ((int)(((byte)(73)))));
            this._btnExpandCollapse.BackColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this._btnExpandCollapse.BackColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(65)))), ((int)(((byte)(73)))));
            this._btnExpandCollapse.ButtonImgLocation = new System.Drawing.Point(5, 1);
            this._btnExpandCollapse.ButtonSize = JMControls.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonSize.Normal;
            this._btnExpandCollapse.ButtonStyle = JMControls.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this._btnExpandCollapse.HeaderTitleLocation = new System.Drawing.Point(55, 10);
            this._btnExpandCollapse.IconAlignment = JMControls.ExpandCollapsePanel.ExpandCollapseButton.ExpandIconAlignment.Right;
            this._btnExpandCollapse.ImageLocation = new System.Drawing.Point(5, 1);
            this._btnExpandCollapse.ImagePicture = null;
            this._btnExpandCollapse.IsExpanded = true;
            this._btnExpandCollapse.Location = new System.Drawing.Point(3, 50);
            this._btnExpandCollapse.MaximumSize = new System.Drawing.Size(40, 40);
            this._btnExpandCollapse.Name = "_btnExpandCollapse";
            this._btnExpandCollapse.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this._btnExpandCollapse.PaddingLeftIcon = 5;
            this._btnExpandCollapse.Size = new System.Drawing.Size(40, 40);
            this._btnExpandCollapse.TabIndex = 0;
            this._btnExpandCollapse.VisibleIcon = true;
            // 
            // animationTimer
            // 
            this.animationTimer.Interval = 50;
            this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(91)))), ((int)(((byte)(135)))));
            this.panelHeader.Controls.Add(this._btnExpandCollapse);
            this.panelHeader.Controls.Add(this._pictureBox);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(271, 100);
            this.panelHeader.TabIndex = 0;
            // 
            // _pictureBox
            // 
            this._pictureBox.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox.Image = global::JMControls.Properties.Resources.Danasha_intelligence;
            this._pictureBox.Location = new System.Drawing.Point(60, 1);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(500, 100);
            this._pictureBox.TabIndex = 1;
            this._pictureBox.TabStop = false;
            // 
            // AccordionCotrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelHeader);
            this.Name = "AccordionCotrol";
            this.Size = new System.Drawing.Size(271, 351);
            this.Load += new System.EventHandler(this.AccordionCotrol_Load);
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Timer animationTimer;
        public System.Windows.Forms.Panel panelHeader;
        private JMControls.ExpandCollapsePanel.ExpandCollapseButton _btnExpandCollapse;
        private System.Windows.Forms.PictureBox _pictureBox;
        #endregion
    }
}
