namespace WindowsFormsApp2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonSolid2 = new JMControls.Controls.ButtonSolid();
            this.SuspendLayout();
            // 
            // buttonSolid2
            // 
            this.buttonSolid2.BackColor = System.Drawing.Color.Transparent;
            this.buttonSolid2.BackColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.buttonSolid2.BackColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.buttonSolid2.BackColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.buttonSolid2.BackColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.buttonSolid2.BorderColorActive = System.Drawing.Color.Navy;
            this.buttonSolid2.BorderColorDisable = System.Drawing.Color.Empty;
            this.buttonSolid2.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(92)))));
            this.buttonSolid2.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonSolid2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buttonSolid2.BorderThickness = 1;
            this.buttonSolid2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.buttonSolid2.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            this.buttonSolid2.ForeColor = System.Drawing.Color.Black;
            this.buttonSolid2.Image = ((System.Drawing.Image)(resources.GetObject("buttonSolid2.Image")));
            this.buttonSolid2.ImageAlinement = JMControls.Enums.ImageAlinement.Center;
            this.buttonSolid2.ImageLocation = new System.Drawing.Point(5, 2);
            this.buttonSolid2.Location = new System.Drawing.Point(88, 60);
            this.buttonSolid2.Name = "buttonSolid2";
            this.buttonSolid2.Radius = 8;
            this.buttonSolid2.Size = new System.Drawing.Size(194, 39);
            this.buttonSolid2.Stroke = false;
            this.buttonSolid2.StrokeColor = System.Drawing.Color.Gray;
            this.buttonSolid2.TabIndex = 0;
            this.buttonSolid2.Text = "buttonSolid2";
            this.buttonSolid2.TextAling = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSolid2.Transparency = false;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(532, 261);
            this.Controls.Add(this.buttonSolid2);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }
        
        #endregion

        private JMControls.Controls.RJButton rjTextBox1;
        private CustomBox.RJControls.RJButton rjButton1;
        private JMControls.Controls.ButtonSolid buttonSolid1;
        private JMControls.Controls.TextBoxRounded textBoxRounded1;
        private JMControls.Controls.ButtonSolid buttonSolid2;
    }
}