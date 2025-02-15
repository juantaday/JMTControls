namespace JMTControls.NetCore.ExpandCollapsePanel
{
    partial class ExpandCollapsePanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this._btnExpandCollapse = new JMTControls.NetCore.ExpandCollapsePanel.ExpandCollapseButton();
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _btnExpandCollapse
            // 
            //this._btnExpandCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            //| System.Windows.Forms.AnchorStyles.Right)));
            this._btnExpandCollapse.BackColor = System.Drawing.Color.FromArgb(58, 65, 73);
            this._btnExpandCollapse.BackColorHover = System.Drawing.Color.FromArgb(60, 60, 60);
            this._btnExpandCollapse.BackColorNormal = System.Drawing.Color.FromArgb(58, 65, 73);
            this._btnExpandCollapse.ButtonSize = JMTControls.NetCore.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonSize.Normal;
            this._btnExpandCollapse.ButtonStyle = JMTControls.NetCore.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this._btnExpandCollapse.IsExpanded = false;
            this._btnExpandCollapse.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnExpandCollapse.Location = new System.Drawing.Point(0, 0);
            this._btnExpandCollapse.MaximumSize = new System.Drawing.Size(0, 40);
            this._btnExpandCollapse.Name = "_btnExpandCollapse";
            this._btnExpandCollapse.Size = new System.Drawing.Size(0, 29);
            this._btnExpandCollapse.TabIndex = 0;
            // 
            // animationTimer
            // 
            this.animationTimer.Interval = 50;
            this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
            // 
            // panel controls
            // 


            this.Controls.Add(this._btnExpandCollapse);

            this.Size = new System.Drawing.Size(365, 319);
            this.ResumeLayout(false);

        }

        #endregion

        public JMTControls.NetCore.ExpandCollapsePanel.ExpandCollapseButton _btnExpandCollapse;
        private System.Windows.Forms.Timer animationTimer;

    }
}
