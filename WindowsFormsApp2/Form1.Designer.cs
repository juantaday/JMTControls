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
            this.textBoxRounded1.ToolTipButton = null;
            this.textBoxRounded1.TypeData = JMControls.Enums.TypeDataEnum.Numeric;
            this.textBoxRounded1.VisibleButton = true;
            this.textBoxRounded1.WidthButton = 32;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxRounded1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private JMControls.Controls.TextBoxRounded textBoxRounded1;
    }
}

