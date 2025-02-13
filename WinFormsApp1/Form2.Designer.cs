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
            advancedShadowPanel2 = new AdvancedShadowPanel();
            textBox1 = new TextBox();
            label1 = new Label();
            advancedShadowPanel2.SuspendLayout();
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
            textBoxRounded2.TabStop = false;
            textBoxRounded2.TextAlign = HorizontalAlignment.Left;
            textBoxRounded2.ToolTipButton = "";
            textBoxRounded2.TypeData = JMTControls.NetCore.Enums.TypeDataEnum.VarChar;
            textBoxRounded2.UseSystemPasswordChar = true;
            textBoxRounded2.VisibleButton = true;
            // 
            // advancedShadowPanel2
            // 
            advancedShadowPanel2.BorderColor = Color.Gray;
            advancedShadowPanel2.BorderRadius = 20;
            advancedShadowPanel2.BorderSize = 1;
            advancedShadowPanel2.Controls.Add(label1);
            advancedShadowPanel2.Controls.Add(textBox1);
            advancedShadowPanel2.Location = new Point(192, 115);
            advancedShadowPanel2.Name = "advancedShadowPanel2";
            advancedShadowPanel2.ShadowColor = Color.FromArgb(50, 0, 0, 0);
            advancedShadowPanel2.ShadowSize = 5;
            advancedShadowPanel2.Size = new Size(350, 193);
            advancedShadowPanel2.TabIndex = 1;
            advancedShadowPanel2.Text = "advancedShadowPanel2";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(17, 14);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(194, 23);
            textBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(53, 79);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 3;
            label1.Text = "label1";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(advancedShadowPanel2);
            Controls.Add(textBoxRounded2);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            advancedShadowPanel2.ResumeLayout(false);
            advancedShadowPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private JMTControls.NetCore.Controls.TextBoxRounded  textBoxRounded1;
        private TextBoxRounded textBoxRounded2;
        private AdvancedShadowPanel advancedShadowPanel1;
        private AdvancedShadowPanel advancedShadowPanel2;
        private Label label1;
        private TextBox textBox1;
    }
}