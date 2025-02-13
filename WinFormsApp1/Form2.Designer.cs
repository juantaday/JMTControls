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
            altoSlidingLabel1 = new AltoSlidingLabel();
            textBoxRounded3 = new TextBoxRounded();
            textBox1 = new TextBox();
            advancedShadowPanel2 = new AdvancedShadowPanel();
            textBox2 = new TextBox();
            groupBoxLiner1 = new GroupBoxLiner();
            advancedShadowPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // altoSlidingLabel1
            // 
            altoSlidingLabel1.ButtonImage = null;
            altoSlidingLabel1.ButtonVisible = false;
            altoSlidingLabel1.ButtonWidth = 20;
            altoSlidingLabel1.Location = new Point(36, 90);
            altoSlidingLabel1.Name = "altoSlidingLabel1";
            altoSlidingLabel1.Size = new Size(246, 25);
            altoSlidingLabel1.Slide = false;
            altoSlidingLabel1.TabIndex = 2;
            altoSlidingLabel1.Text = "altoSlidingLabel1";
            // 
            // textBoxRounded3
            // 
            textBoxRounded3.AutoCompleteMode = AutoCompleteMode.None;
            textBoxRounded3.AutoCompleteSource = AutoCompleteSource.None;
            textBoxRounded3.BackColor = Color.White;
            textBoxRounded3.BorderColorActive = Color.Red;
            textBoxRounded3.BorderColorDisable = Color.LightGray;
            textBoxRounded3.BorderColorHover = Color.Orange;
            textBoxRounded3.BorderColorIdle = Color.Gray;
            textBoxRounded3.BorderRadius = 14;
            textBoxRounded3.BorderStyle = BorderStyle.FixedSingle;
            textBoxRounded3.BorderThickness = 2;
            textBoxRounded3.ButtonImage = (Image)resources.GetObject("textBoxRounded3.ButtonImage");
            textBoxRounded3.CharacterCasing = CharacterCasing.Normal;
            textBoxRounded3.DecimalPosition = 2;
            textBoxRounded3.Font = new Font("Arial", 12F);
            textBoxRounded3.IconLeft = (Image)resources.GetObject("textBoxRounded3.IconLeft");
            textBoxRounded3.IconLeftBackColor = Color.White;
            textBoxRounded3.IconLeftVisible = false;
            textBoxRounded3.Location = new Point(16, 19);
            textBoxRounded3.MaxLength = 32767;
            textBoxRounded3.Multiline = false;
            textBoxRounded3.Name = "textBoxRounded3";
            textBoxRounded3.PasswordChar = '\0';
            textBoxRounded3.PlaceHolderColor = Color.FromArgb(180, 180, 180);
            textBoxRounded3.PlaceHolderText = "JMTControls TextBoxRounded..";
            textBoxRounded3.ReadOnly = false;
            textBoxRounded3.SelectedText = "";
            textBoxRounded3.SelectionLength = 0;
            textBoxRounded3.Size = new Size(251, 36);
            textBoxRounded3.TabIndex = 2;
            textBoxRounded3.TabStop = false;
            textBoxRounded3.TextAlign = HorizontalAlignment.Left;
            textBoxRounded3.ToolTipButton = "";
            textBoxRounded3.TypeData = JMTControls.NetCore.Enums.TypeDataEnum.VarChar;
            textBoxRounded3.UseSystemPasswordChar = false;
            textBoxRounded3.VisibleButton = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(378, 40);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(196, 23);
            textBox1.TabIndex = 2;
            // 
            // advancedShadowPanel2
            // 
            advancedShadowPanel2.BorderColor = Color.Gray;
            advancedShadowPanel2.BorderRadius = 20;
            advancedShadowPanel2.BorderSize = 1;
            advancedShadowPanel2.Controls.Add(textBoxRounded3);
            advancedShadowPanel2.Controls.Add(textBox2);
            advancedShadowPanel2.Controls.Add(altoSlidingLabel1);
            advancedShadowPanel2.Location = new Point(66, 67);
            advancedShadowPanel2.Name = "advancedShadowPanel2";
            advancedShadowPanel2.ShadowColor = Color.FromArgb(50, 0, 0, 0);
            advancedShadowPanel2.ShadowSize = 5;
            advancedShadowPanel2.Size = new Size(309, 137);
            advancedShadowPanel2.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(36, 61);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(103, 23);
            textBox2.TabIndex = 2;
            // 
            // groupBoxLiner1
            // 
            groupBoxLiner1.BorderColor = Color.Black;
            groupBoxLiner1.BorderRadius = 8;
            groupBoxLiner1.BorderThickness = 1;
            groupBoxLiner1.Location = new Point(185, 259);
            groupBoxLiner1.Name = "groupBoxLiner1";
            groupBoxLiner1.Size = new Size(270, 129);
            groupBoxLiner1.TabIndex = 4;
            groupBoxLiner1.TabStop = false;
            groupBoxLiner1.Text = "groupBoxLiner1";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBoxLiner1);
            Controls.Add(advancedShadowPanel2);
            Controls.Add(textBox1);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            advancedShadowPanel2.ResumeLayout(false);
            advancedShadowPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private JMTControls.NetCore.Controls.TextBoxRounded  textBoxRounded1;
        private AdvancedShadowPanel advancedShadowPanel1;
        private TextBoxRounded textBoxRounded3;
        private TextBox textBox1;
        private AltoSlidingLabel altoSlidingLabel1;
        private AdvancedShadowPanel advancedShadowPanel2;
        private TextBox textBox2;
        private GroupBoxLiner groupBoxLiner1;
    }
}