using System.Windows.Forms;

namespace JMControls.ExpandCollapsePanel
{
    partial class ExpandCollapseButton
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblLine = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.logoPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLine
            // 
            this.lblLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLine.BackColor = System.Drawing.Color.Black;
            this.lblLine.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblLine.Location = new System.Drawing.Point(41, 30);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(382, 1);
            this.lblLine.TabIndex = 0;
            this.lblLine.Text = string.Empty;
            this.lblLine.MouseDown += new MouseEventHandler(this.OnMouseDown);
            // this.lblLine.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.lblLine.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblHeader.Location = new System.Drawing.Point(55, 10);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(68, 15);
            this.lblHeader.TabIndex = 2;
            this.lblHeader.Text = "Заголовок";
            this.lblHeader.MouseDown += new MouseEventHandler(this.OnMouseDown);
            this.lblHeader.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.lblHeader.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            // 
            // logoPicture
            // 
            //this.logoPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))));
            this.logoPicture.Image = global::JMControls.Properties.Resources.Arrow3;
            this.logoPicture.Name = "logoPicture";
            this.logoPicture.Size = new System.Drawing.Size(35, 35);
            this.logoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPicture.TabIndex = 1;
            this.logoPicture.TabStop = false;
            this.logoPicture.Location = new System.Drawing.Point(100, 6);
            this.logoPicture.MouseDown += new MouseEventHandler(this.OnMouseDown);
            this.logoPicture.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.logoPicture.MouseLeave += new System.EventHandler(this.OnMouseLeave);

            // 
            // ExpandCollapseButton
            // 
            this.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //
            // logo image
            //
            this._imagePicture = new System.Windows.Forms.PictureBox();
            this._imagePicture.Location = new System.Drawing.Point(5, 1);
            this._imagePicture.Size = new System.Drawing.Size(32, 32);
            this._imagePicture.MouseDown += new MouseEventHandler(this.OnMouseDown);
            this._imagePicture.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this._imagePicture.MouseLeave += new System.EventHandler(this.OnMouseLeave);


            this.Controls.Add(_imagePicture);
            this.Controls.Add(this.logoPicture);
            this.Controls.Add(this.lblHeader);

            // this.Controls.Add(this.lblLine);
            this.MaximumSize = new System.Drawing.Size(0, 40);
            this.Name = "ExpandCollapseButton";
            this.Size = new System.Drawing.Size(150, 40);
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).EndInit();
            this.MouseDown += new MouseEventHandler(this.OnMouseDown);
            this.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.PictureBox logoPicture;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox _imagePicture;

    }
}
