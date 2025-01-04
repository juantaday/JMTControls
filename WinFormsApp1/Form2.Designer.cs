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
            jmTabControl1 = new JMTControls.NetCore.TabControlGRD.JMTabControl();
            tabPage1 = new JMTControls.NetCore.TabControlGRD.TabPageEx();
            tabPage2 = new JMTControls.NetCore.TabControlGRD.TabPageEx();
            jmTabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // jmTabControl1
            // 
            jmTabControl1.AllowDrop = true;
            jmTabControl1.BackgroundHatcher.HatchType = System.Drawing.Drawing2D.HatchStyle.DashedVertical;
            jmTabControl1.TabPages.Add(tabPage1);
            jmTabControl1.TabPages.Add(tabPage2);
            jmTabControl1.ItemSize = new Size(300, 35);
            jmTabControl1.Location = new Point(89, 44);
            jmTabControl1.Name = "jmTabControl1";
            jmTabControl1.SelectedIndex = 0;
            jmTabControl1.Size = new Size(650, 240);
            jmTabControl1.TabGradient.ColorEnd = Color.Gainsboro;
            jmTabControl1.TabIndex = 0;
            jmTabControl1.UpDownStyle = JMTControls.NetCore.TabControlGRD.JMTabControl.UpDown32Style.Default;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.White;
            tabPage1.Font = new Font("Arial", 10F);
            tabPage1.ImageLocation = new Point(15, 5);
            tabPage1.Location = new Point(1, 41);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(648, 177);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.White;
            tabPage2.Font = new Font("Arial", 10F);
            tabPage2.ImageLocation = new Point(15, 5);
            tabPage2.Location = new Point(1, 41);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(541, 32);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(jmTabControl1);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            jmTabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private JMTControls.NetCore.Controls.TextBoxRounded  textBoxRounded1;
        private JMTControls.NetCore.TabControlGRD.JMTabControl jmTabControl1;
        private JMTControls.NetCore.TabControlGRD.TabPageEx tabPage1;
        private JMTControls.NetCore.TabControlGRD.TabPageEx tabPage2;
    }
}