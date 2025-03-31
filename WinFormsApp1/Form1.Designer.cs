namespace WinFormsApp1
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
            stepProgressControl1 = new JMTControls.NetCore.Controls.StepProgressControl();
            stepPage2 = new JMTControls.NetCore.Controls.StepProgressControl.StepPage();
            stepPage1 = new JMTControls.NetCore.Controls.StepProgressControl.StepPage();
            stepPage3 = new JMTControls.NetCore.Controls.StepProgressControl.StepPage();
            stepPage4 = new JMTControls.NetCore.Controls.StepProgressControl.StepPage();
            button1 = new Button();
            button2 = new Button();
            stepProgressControl1.SuspendLayout();
            SuspendLayout();
            // 
            // stepProgressControl1
            // 
            stepProgressControl1.ActiveColor = Color.FromArgb(0, 120, 215);
            stepProgressControl1.CircleSize = 30;
            stepProgressControl1.CompletedColor = Color.Green;
            stepProgressControl1.Controls.Add(stepPage1);
            stepProgressControl1.Controls.Add(stepPage2);
            stepProgressControl1.Controls.Add(stepPage3);
            stepProgressControl1.Controls.Add(stepPage4);
            stepProgressControl1.CurrentStep = null;
            stepProgressControl1.InactiveColor = Color.Gray;
            stepProgressControl1.IndexStep = 0;
            stepProgressControl1.Location = new Point(120, 51);
            stepProgressControl1.Name = "stepProgressControl1";
            stepProgressControl1.Size = new Size(549, 365);
            stepProgressControl1.StepHeight = 60;
            stepProgressControl1.Steps.Add(stepPage1);
            stepProgressControl1.Steps.Add(stepPage2);
            stepProgressControl1.Steps.Add(stepPage3);
            stepProgressControl1.Steps.Add(stepPage4);
            stepProgressControl1.TabIndex = 0;
            // 
            // stepPage2
            // 
            stepPage2.AutoScroll = true;
            stepPage2.BackColor = SystemColors.Control;
            stepPage2.BorderColor = Color.Gray;
            stepPage2.BorderRadius = 10;
            stepPage2.BorderThickness = 2;
            stepPage2.Dock = DockStyle.Fill;
            stepPage2.Location = new Point(0, 0);
            stepPage2.Name = "stepPage2";
            stepPage2.Padding = new Padding(10);
            stepPage2.Size = new Size(549, 365);
            stepPage2.TabIndex = 3;
            stepPage2.Text = "stepPage2";
            stepPage2.Title = "Step";
            // 
            // stepPage1
            // 
            stepPage1.AutoScroll = true;
            stepPage1.BackColor = SystemColors.Control;
            stepPage1.BorderColor = Color.Gray;
            stepPage1.BorderRadius = 10;
            stepPage1.BorderThickness = 2;
            stepPage1.Dock = DockStyle.Fill;
            stepPage1.Location = new Point(0, 90);
            stepPage1.Name = "stepPage1";
            stepPage1.Padding = new Padding(10);
            stepPage1.Size = new Size(549, 275);
            stepPage1.TabIndex = 2;
            stepPage1.Text = "stepPage1";
            stepPage1.Title = "Step";
            // 
            // stepPage3
            // 
            stepPage3.AutoScroll = true;
            stepPage3.BackColor = SystemColors.Control;
            stepPage3.BorderColor = Color.Gray;
            stepPage3.BorderRadius = 10;
            stepPage3.BorderThickness = 2;
            stepPage3.Dock = DockStyle.Fill;
            stepPage3.Location = new Point(0, 0);
            stepPage3.Name = "stepPage3";
            stepPage3.Padding = new Padding(10);
            stepPage3.Size = new Size(549, 365);
            stepPage3.TabIndex = 4;
            stepPage3.Text = "stepPage3";
            stepPage3.Title = "Step";
            // 
            // stepPage4
            // 
            stepPage4.AutoScroll = true;
            stepPage4.BackColor = SystemColors.Control;
            stepPage4.BorderColor = Color.Gray;
            stepPage4.BorderRadius = 10;
            stepPage4.BorderThickness = 2;
            stepPage4.Dock = DockStyle.Fill;
            stepPage4.Location = new Point(0, 0);
            stepPage4.Name = "stepPage4";
            stepPage4.Padding = new Padding(10);
            stepPage4.Size = new Size(549, 365);
            stepPage4.TabIndex = 5;
            stepPage4.Text = "stepPage4";
            stepPage4.Title = "Step";
            // 
            // button1
            // 
            button1.Location = new Point(701, 93);
            button1.Name = "button1";
            button1.Size = new Size(70, 50);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(693, 177);
            button2.Name = "button2";
            button2.Size = new Size(85, 35);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(stepProgressControl1);
            Name = "Form1";
            Text = "Form1";
            stepProgressControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private JMTControls.NetCore.Controls.StepProgressControl stepProgressControl1;
        private JMTControls.NetCore.Controls.StepProgressControl.StepPage stepPage2;
        private JMTControls.NetCore.Controls.StepProgressControl.StepPage stepPage1;
        private JMTControls.NetCore.Controls.StepProgressControl.StepPage stepPage3;
        private JMTControls.NetCore.Controls.StepProgressControl.StepPage stepPage4;
        private Button button1;
        private Button button2;
    }
}