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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBoxRounded1 = new JMControls.Controls.TextBoxRounded();
            this.rjTextBox1 = new JMControls.Controls.RJTextBox();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "MiniApp.png");
            this.imageList1.Images.SetKeyName(1, "MiniSetting.png");
            this.imageList1.Images.SetKeyName(2, "MiniToolsBox.png");
            // 
            // textBoxRounded1
            // 
            this.textBoxRounded1.Autosize = false;
            this.textBoxRounded1.BorderColorActive = System.Drawing.Color.Empty;
            this.textBoxRounded1.BorderColorDisable = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.textBoxRounded1.BorderColorHover = System.Drawing.Color.Empty;
            this.textBoxRounded1.BorderColorIdle = System.Drawing.Color.Teal;
            this.textBoxRounded1.BorderRadius = 8;
            this.textBoxRounded1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRounded1.BorderThickness = 1;
            this.textBoxRounded1.ButtonImage = null;
            this.textBoxRounded1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textBoxRounded1.DecimalPosition = 3;
            this.textBoxRounded1.DockButton = System.Windows.Forms.DockStyle.Right;
            this.textBoxRounded1.FillColor = System.Drawing.Color.White;
            this.textBoxRounded1.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.textBoxRounded1.Location = new System.Drawing.Point(271, 105);
            this.textBoxRounded1.MaxLenght = 32767;
            this.textBoxRounded1.Multiline = false;
            this.textBoxRounded1.Name = "textBoxRounded1";
            this.textBoxRounded1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBoxRounded1.PasswordChar = false;
            this.textBoxRounded1.PlaceHolderColor = System.Drawing.Color.DarkGray;
            this.textBoxRounded1.PlaceHolderText = null;
            this.textBoxRounded1.ReadOnly = false;
            this.textBoxRounded1.SelectionLength = 0;
            this.textBoxRounded1.Size = new System.Drawing.Size(283, 27);
            this.textBoxRounded1.TabIndex = 0;
            this.textBoxRounded1.TabStop = false;
            this.textBoxRounded1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxRounded1.Texts = "";
            this.textBoxRounded1.ToolTipButton = null;
            this.textBoxRounded1.TypeData = JMControls.Enums.TypeDataEnum.Decimal;
            this.textBoxRounded1.VisibleButton = true;
            this.textBoxRounded1.WidthButton = 32;
            // 
            // rjTextBox1
            // 
            this.rjTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.rjTextBox1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.rjTextBox1.BorderFocusColor = System.Drawing.Color.HotPink;
            this.rjTextBox1.BorderRadius = 8;
            this.rjTextBox1.BorderThickness = 2;
            this.rjTextBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.rjTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.rjTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rjTextBox1.Location = new System.Drawing.Point(271, 189);
            this.rjTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.rjTextBox1.MaxLength = 32767;
            this.rjTextBox1.Multiline = false;
            this.rjTextBox1.Name = "rjTextBox1";
            this.rjTextBox1.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.rjTextBox1.PasswordChar = false;
            this.rjTextBox1.PlaceHolderColor = System.Drawing.Color.DarkGray;
            this.rjTextBox1.PlaceHolderText = "";
            this.rjTextBox1.ReadOnly = false;
            this.rjTextBox1.Size = new System.Drawing.Size(258, 31);
            this.rjTextBox1.TabIndex = 1;
            this.rjTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.rjTextBox1.Texts = "";
            this.rjTextBox1.UnderlinedStyle = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rjTextBox1);
            this.Controls.Add(this.textBoxRounded1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private JMControls.Controls.TextBoxRounded textBoxRounded1;
        private JMControls.Controls.RJTextBox rjTextBox1;
    }
}

