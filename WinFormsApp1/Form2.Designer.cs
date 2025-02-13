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
            label1 = new Label();
            advancedShadowPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // altoSlidingLabel1
            // 
            altoSlidingLabel1.ButtonImage = null;
            altoSlidingLabel1.ButtonVisible = false;
            altoSlidingLabel1.ButtonWidth = 20;
            altoSlidingLabel1.Location = new Point(349, 121);
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
            textBoxRounded3.Location = new Point(417, 248);
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
            advancedShadowPanel2.Controls.Add(label1);
            advancedShadowPanel2.Location = new Point(92, 67);
            advancedShadowPanel2.Name = "advancedShadowPanel2";
            advancedShadowPanel2.ShadowColor = Color.FromArgb(50, 0, 0, 0);
            advancedShadowPanel2.ShadowSize = 5;
            advancedShadowPanel2.Size = new Size(251, 170);
            advancedShadowPanel2.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(35, 39);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "label1";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(advancedShadowPanel2);
            Controls.Add(altoSlidingLabel1);
            Controls.Add(textBox1);
            Controls.Add(textBoxRounded3);
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
        private Label label1;
    }
}