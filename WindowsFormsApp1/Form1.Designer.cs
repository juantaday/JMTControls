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
            this.jmTabControl1 = new JMControls.TabControlGRD.JMTabControl();
            this.tabPageEx1 = new JMControls.TabControlGRD.TabPageEx();
            this.tabPageEx2 = new JMControls.TabControlGRD.TabPageEx();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxRounded1 = new JMControls.Controls.TextBoxRounded();
            this.textBoxRounded2 = new JMControls.Controls.TextBoxRounded();
            this.jmTabControl1.SuspendLayout();
            this.tabPageEx2.SuspendLayout();
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
            // jmTabControl1
            // 
            this.jmTabControl1.AllowDrop = true;
            this.jmTabControl1.BackgroundHatcher.HatchType = System.Drawing.Drawing2D.HatchStyle.DashedVertical;
            this.jmTabControl1.Controls.Add(this.tabPageEx1);
            this.jmTabControl1.Controls.Add(this.tabPageEx2);
            this.jmTabControl1.ItemSize = new System.Drawing.Size(0, 26);
            this.jmTabControl1.Location = new System.Drawing.Point(76, 46);
            this.jmTabControl1.Name = "jmTabControl1";
            this.jmTabControl1.SelectedIndex = 1;
            this.jmTabControl1.Size = new System.Drawing.Size(345, 270);
            this.jmTabControl1.TabGradient.ColorEnd = System.Drawing.Color.Gainsboro;
            this.jmTabControl1.TabIndex = 0;
            this.jmTabControl1.UpDownStyle = JMControls.TabControlGRD.JMTabControl.UpDown32Style.Default;
            // 
            // tabPageEx1
            // 
            this.tabPageEx1.BackColor = System.Drawing.Color.White;
            this.tabPageEx1.Font = new System.Drawing.Font("Arial", 10F);
            this.tabPageEx1.ImageLocation = new System.Drawing.Point(15, 5);
            this.tabPageEx1.Location = new System.Drawing.Point(1, 32);
            this.tabPageEx1.Name = "tabPageEx1";
            this.tabPageEx1.Size = new System.Drawing.Size(343, 216);
            this.tabPageEx1.TabIndex = 0;
            this.tabPageEx1.Text = "tabPageEx1";
            // 
            // tabPageEx2
            // 
            this.tabPageEx2.BackColor = System.Drawing.Color.White;
            this.tabPageEx2.Controls.Add(this.textBoxRounded1);
            this.tabPageEx2.Controls.Add(this.textBox1);
            this.tabPageEx2.Font = new System.Drawing.Font("Arial", 10F);
            this.tabPageEx2.ImageLocation = new System.Drawing.Point(15, 5);
            this.tabPageEx2.Location = new System.Drawing.Point(1, 32);
            this.tabPageEx2.Name = "tabPageEx2";
            this.tabPageEx2.Size = new System.Drawing.Size(343, 216);
            this.tabPageEx2.TabIndex = 1;
            this.tabPageEx2.Text = "tabPageEx2";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(51, 122);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(152, 23);
            this.textBox1.TabIndex = 0;
            // 
            // textBoxRounded1
            // 
            this.textBoxRounded1.BorderColorActive = System.Drawing.Color.Empty;
            this.textBoxRounded1.BorderColorDisable = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.textBoxRounded1.BorderColorHover = System.Drawing.Color.Empty;
            this.textBoxRounded1.BorderColorIdle = System.Drawing.Color.Teal;
            this.textBoxRounded1.BorderRadius = 12;
            this.textBoxRounded1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRounded1.BorderThickness = 1;
            this.textBoxRounded1.ButtonImage = null;
            this.textBoxRounded1.DockButton = System.Windows.Forms.DockStyle.Right;
            this.textBoxRounded1.FillColor = System.Drawing.Color.White;
            this.textBoxRounded1.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.textBoxRounded1.Location = new System.Drawing.Point(38, 31);
            this.textBoxRounded1.MaxLenght = 32767;
            this.textBoxRounded1.Multiline = false;
            this.textBoxRounded1.Name = "textBoxRounded1";
            this.textBoxRounded1.Padding = new System.Windows.Forms.Padding(1);
            this.textBoxRounded1.PasswordChar = false;
            this.textBoxRounded1.PlaceHolderColor = System.Drawing.Color.DarkGray;
            this.textBoxRounded1.PlaceHolderText = null;
            this.textBoxRounded1.ReadOnly = false;
            this.textBoxRounded1.Size = new System.Drawing.Size(175, 23);
            this.textBoxRounded1.TabIndex = 1;
            this.textBoxRounded1.TabStop = false;
            this.textBoxRounded1.Text = "textBoxRounded1";
            this.textBoxRounded1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxRounded1.ToolTipButton = null;
            this.textBoxRounded1.VisibleButton = true;
            this.textBoxRounded1.WidthButton = 32;
            // 
            // textBoxRounded2
            // 
            this.textBoxRounded2.BorderColorActive = System.Drawing.Color.Empty;
            this.textBoxRounded2.BorderColorDisable = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.textBoxRounded2.BorderColorHover = System.Drawing.Color.Empty;
            this.textBoxRounded2.BorderColorIdle = System.Drawing.Color.Teal;
            this.textBoxRounded2.BorderRadius = 12;
            this.textBoxRounded2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRounded2.BorderThickness = 1;
            this.textBoxRounded2.ButtonImage = null;
            this.textBoxRounded2.DockButton = System.Windows.Forms.DockStyle.Right;
            this.textBoxRounded2.FillColor = System.Drawing.Color.White;
            this.textBoxRounded2.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.textBoxRounded2.Location = new System.Drawing.Point(478, 278);
            this.textBoxRounded2.MaxLenght = 32767;
            this.textBoxRounded2.Multiline = false;
            this.textBoxRounded2.Name = "textBoxRounded2";
            this.textBoxRounded2.Padding = new System.Windows.Forms.Padding(1);
            this.textBoxRounded2.PasswordChar = false;
            this.textBoxRounded2.PlaceHolderColor = System.Drawing.Color.DarkGray;
            this.textBoxRounded2.PlaceHolderText = null;
            this.textBoxRounded2.ReadOnly = false;
            this.textBoxRounded2.Size = new System.Drawing.Size(188, 23);
            this.textBoxRounded2.TabIndex = 1;
            this.textBoxRounded2.TabStop = false;
            this.textBoxRounded2.Text = "textBoxRounded2";
            this.textBoxRounded2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxRounded2.ToolTipButton = null;
            this.textBoxRounded2.VisibleButton = true;
            this.textBoxRounded2.WidthButton = 32;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxRounded2);
            this.Controls.Add(this.jmTabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.jmTabControl1.ResumeLayout(false);
            this.tabPageEx2.ResumeLayout(false);
            this.tabPageEx2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private JMControls.TabControlGRD.JMTabControl jmTabControl1;
        private JMControls.TabControlGRD.TabPageEx tabPageEx1;
        private JMControls.TabControlGRD.TabPageEx tabPageEx2;
        private JMControls.Controls.TextBoxRounded textBoxRounded1;
        private System.Windows.Forms.TextBox textBox1;
        private JMControls.Controls.TextBoxRounded textBoxRounded2;
    }
}

