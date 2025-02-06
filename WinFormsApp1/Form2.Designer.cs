using JMTControls.NetCore.Controls;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            textBoxRounded2 = new TextBoxRounded();
            textBox1 = new TextBox();
            jmComboBox1 = new JMComboBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // textBoxRounded2
            // 
            textBoxRounded2.AutoCompleteMode = AutoCompleteMode.None;
            textBoxRounded2.AutoCompleteSource = AutoCompleteSource.None;
            textBoxRounded2.BackColor = Color.White;
            textBoxRounded2.BorderColorActive = Color.Red;
            textBoxRounded2.BorderColorDisable = Color.LightGray;
            textBoxRounded2.BorderColorHover = Color.Orange;
            textBoxRounded2.BorderColorIdle = Color.Gray;
            textBoxRounded2.BorderRadius = 14;
            textBoxRounded2.BorderStyle = BorderStyle.FixedSingle;
            textBoxRounded2.BorderThickness = 2;
            textBoxRounded2.ButtonImage = (Image)resources.GetObject("textBoxRounded2.ButtonImage");
            textBoxRounded2.CharacterCasing = CharacterCasing.Normal;
            textBoxRounded2.DecimalPosition = 2;
            textBoxRounded2.Font = new Font("Arial", 12F);
            textBoxRounded2.IconLeft = (Image)resources.GetObject("textBoxRounded2.IconLeft");
            textBoxRounded2.IconLeftBackColor = Color.White;
            textBoxRounded2.IconLeftVisible = false;
            textBoxRounded2.Location = new Point(138, 6);
            textBoxRounded2.MaxLength = 32767;
            textBoxRounded2.Multiline = false;
            textBoxRounded2.Name = "textBoxRounded2";
            textBoxRounded2.PasswordChar = '\0';
            textBoxRounded2.PlaceHolderColor = Color.FromArgb(180, 180, 180);
            textBoxRounded2.PlaceHolderText = "JMTControls TextBoxRounded..";
            textBoxRounded2.ReadOnly = false;
            textBoxRounded2.SelectedText = "";
            textBoxRounded2.SelectionLength = 0;
            textBoxRounded2.Size = new Size(457, 36);
            textBoxRounded2.TabIndex = 0;
            textBoxRounded2.TextAlign = HorizontalAlignment.Left;
            textBoxRounded2.ToolTipButton = "";
            textBoxRounded2.TypeData = JMTControls.NetCore.Enums.TypeDataEnum.VarChar;
            textBoxRounded2.UseSystemPasswordChar = true;
            textBoxRounded2.VisibleButton = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(196, 70);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(349, 23);
            textBox1.TabIndex = 1;
            // 
            // jmComboBox1
            // 
            jmComboBox1.BackColor = Color.White;
            jmComboBox1.BorderColor = Color.DarkViolet;
            jmComboBox1.BorderStyle = BorderStyle.FixedSingle;
            jmComboBox1.ButtonImage = null;
            jmComboBox1.DataSource = null;
            jmComboBox1.DisplayMember = null;
            jmComboBox1.DroppedDown = false;
            jmComboBox1.ForeColor = Color.DarkViolet;
            jmComboBox1.IconColor = Color.MediumSlateBlue;
            jmComboBox1.Location = new Point(190, 168);
            jmComboBox1.MinimumSize = new Size(60, 25);
            jmComboBox1.Name = "jmComboBox1";
            jmComboBox1.Padding = new Padding(1);
            jmComboBox1.SelectedIndex = -1;
            jmComboBox1.Size = new Size(430, 42);
            jmComboBox1.TabIndex = 2;
            jmComboBox1.ValueMember = null;
            jmComboBox1.SelectedIndexChanged += jmComboBox1_SelectedIndexChanged_1;
            // 
            // button1
            // 
            button1.Location = new Point(175, 246);
            button1.Name = "button1";
            button1.Size = new Size(438, 65);
            button1.TabIndex = 3;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(jmComboBox1);
            Controls.Add(textBox1);
            Controls.Add(textBoxRounded2);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private JMTControls.NetCore.Controls.TextBoxRounded  textBoxRounded1;
        private TextBoxRounded textBoxRounded2;
        private TextBox textBox1;
        private JMComboBox jmComboBox1;
        private Button button1;
    }
}