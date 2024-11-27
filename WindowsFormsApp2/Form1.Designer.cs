namespace WindowsFormsApp2
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
            this.textBoxRounded1 = new JMControls.Controls.TextBoxRounded();
            this.jmTabControl1 = new JMControls.TabControlGRD.JMTabControl();
            this.tabPageEx3 = new JMControls.TabControlGRD.TabPageEx();
            this.tabPageEx1 = new JMControls.TabControlGRD.TabPageEx();
            this.tabPageEx2 = new JMControls.TabControlGRD.TabPageEx();
            this.jmTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxRounded1
            // 
            this.textBoxRounded1.Autosize = false;
            this.textBoxRounded1.BorderColorActive = System.Drawing.Color.Red;
            this.textBoxRounded1.BorderColorDisable = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.textBoxRounded1.BorderColorHover = System.Drawing.Color.Orange;
            this.textBoxRounded1.BorderColorIdle = System.Drawing.Color.DeepPink;
            this.textBoxRounded1.BorderRadius = 8;
            this.textBoxRounded1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRounded1.BorderThickness = 1;
            this.textBoxRounded1.ButtonImage = null;
            this.textBoxRounded1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textBoxRounded1.DecimalPosition = 2;
            this.textBoxRounded1.DockButton = System.Windows.Forms.DockStyle.Right;
            this.textBoxRounded1.FillColor = System.Drawing.Color.White;
            this.textBoxRounded1.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.textBoxRounded1.Location = new System.Drawing.Point(125, 85);
            this.textBoxRounded1.MaxLenght = 32767;
            this.textBoxRounded1.Multiline = false;
            this.textBoxRounded1.Name = "textBoxRounded1";
            this.textBoxRounded1.Padding = new System.Windows.Forms.Padding(15, 5, 5, 5);
            this.textBoxRounded1.PasswordChar = false;
            this.textBoxRounded1.PlaceHolderColor = System.Drawing.Color.Empty;
            this.textBoxRounded1.PlaceHolderText = null;
            this.textBoxRounded1.ReadOnly = false;
            this.textBoxRounded1.SelectionLength = 0;
            this.textBoxRounded1.Size = new System.Drawing.Size(253, 35);
            this.textBoxRounded1.TabIndex = 0;
            this.textBoxRounded1.TabStop = false;
            this.textBoxRounded1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxRounded1.ToolTipButton = "";
            this.textBoxRounded1.TypeData = JMControls.Enums.TypeDataEnum.Numeric;
            this.textBoxRounded1.VisibleButton = true;
            this.textBoxRounded1.WidthButton = 32;
            // 
            // jmTabControl1
            // 
            this.jmTabControl1.AllowDrop = true;
            this.jmTabControl1.BackgroundHatcher.HatchType = System.Drawing.Drawing2D.HatchStyle.DashedVertical;
            this.jmTabControl1.Controls.Add(this.tabPageEx3);
            this.jmTabControl1.Controls.Add(this.tabPageEx2);
            this.jmTabControl1.Controls.Add(this.tabPageEx1);
            this.jmTabControl1.Font = new System.Drawing.Font("Tahoma", 14F);
            this.jmTabControl1.ItemSize = new System.Drawing.Size(291, 35);
            this.jmTabControl1.Location = new System.Drawing.Point(158, 206);
            this.jmTabControl1.Name = "jmTabControl1";
            this.jmTabControl1.SelectedIndex = 2;
            this.jmTabControl1.Size = new System.Drawing.Size(606, 156);
            this.jmTabControl1.TabGradient.ColorEnd = System.Drawing.Color.Gainsboro;
            this.jmTabControl1.TabIndex = 1;
            this.jmTabControl1.UpDownStyle = JMControls.TabControlGRD.JMTabControl.UpDown32Style.Default;
            // 
            // tabPageEx3
            // 
            this.tabPageEx3.BackColor = System.Drawing.Color.White;
            this.tabPageEx3.Font = new System.Drawing.Font("Arial", 14F);
            this.tabPageEx3.ImageLocation = new System.Drawing.Point(15, 5);
            this.tabPageEx3.Location = new System.Drawing.Point(1, 41);
            this.tabPageEx3.Name = "tabPageEx3";
            this.tabPageEx3.Size = new System.Drawing.Size(604, 71);
            this.tabPageEx3.TabIndex = 2;
            this.tabPageEx3.Text = "tabPageEx3 setert d sfgrtr x";
            // 
            // tabPageEx1
            // 
            this.tabPageEx1.BackColor = System.Drawing.Color.White;
            this.tabPageEx1.Font = new System.Drawing.Font("Arial", 14F);
            this.tabPageEx1.ImageLocation = new System.Drawing.Point(15, 5);
            this.tabPageEx1.Location = new System.Drawing.Point(1, 41);
            this.tabPageEx1.Name = "tabPageEx1";
            this.tabPageEx1.Size = new System.Drawing.Size(604, 71);
            this.tabPageEx1.TabIndex = 3;
            this.tabPageEx1.Text = "tabPageEx1 dffg xgdth xgdty";
            // 
            // tabPageEx2
            // 
            this.tabPageEx2.BackColor = System.Drawing.Color.White;
            this.tabPageEx2.Font = new System.Drawing.Font("Arial", 12F);
            this.tabPageEx2.ImageLocation = new System.Drawing.Point(15, 5);
            this.tabPageEx2.Location = new System.Drawing.Point(1, 41);
            this.tabPageEx2.Name = "tabPageEx2";
            this.tabPageEx2.Size = new System.Drawing.Size(604, 93);
            this.tabPageEx2.TabIndex = 4;
            this.tabPageEx2.Text = "tabPageEx2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 450);
            this.Controls.Add(this.jmTabControl1);
            this.Controls.Add(this.textBoxRounded1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.jmTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private JMControls.Controls.TextBoxRounded textBoxRounded1;
        private JMControls.TabControlGRD.JMTabControl jmTabControl1;
        private JMControls.TabControlGRD.TabPageEx tabPageEx3;
        private JMControls.TabControlGRD.TabPageEx tabPageEx1;
        private JMControls.TabControlGRD.TabPageEx tabPageEx2;
    }
}

