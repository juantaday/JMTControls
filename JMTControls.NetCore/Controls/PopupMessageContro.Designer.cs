using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    partial class PopupMessageContro
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
            components = new System.ComponentModel.Container();
            elipseComponent1 = new Componets.ElipseComponent();
            messageLabel = new Label();
            successIcon = new PictureBox();
            disappearTimer = new Timer(components);
            ((System.ComponentModel.ISupportInitialize)successIcon).BeginInit();
            SuspendLayout();
            // 
            // elipseComponent1
            // 
            elipseComponent1.CornerRadius = 20;
            elipseComponent1.TargetControl = this;
            // 
            // messageLabel
            // 
            messageLabel.AutoSize = true;
            messageLabel.Location = new Point(41, 13);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(38, 15);
            messageLabel.TabIndex = 0;
            messageLabel.Text = "label1";
            // 
            // successIcon
            // 
            successIcon.Location = new Point(10, 15);
            successIcon.Name = "successIcon";
            successIcon.Size = new Size(30, 30);
            successIcon.SizeMode = PictureBoxSizeMode.Zoom;
            successIcon.TabIndex = 0;
            successIcon.TabStop = false;
            // 
            // disappearTimer
            // 
            disappearTimer.Tick += disappearTimer_Tick;
            // 
            // PopupMessageContro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGreen;
            Controls.Add(successIcon);
            Controls.Add(messageLabel);
            Name = "PopupMessageContro";
            Padding = new Padding(10);
            Size = new Size(371, 60);
            Load += PopupMessageContro_Load;
            ((System.ComponentModel.ISupportInitialize)successIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Componets.ElipseComponent elipseComponent1;
        private System.Windows.Forms.Label messageLabel;
        private PictureBox successIcon;
        private Timer disappearTimer;
    }
}
