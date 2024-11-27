using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing.Design;
using System;

namespace JMControls.Controls
{
    [DefaultEvent("SelectedIndexChanged")]

    public class RJComboBox : UserControl
    {
        private Color backColor = Color.WhiteSmoke;

        private Color iconColor = Color.MediumSlateBlue;

        private Color listBackColor = Color.FromArgb(230, 228, 245);

        private Color listTextColor = Color.DimGray;

        private Color borderColor = Color.MediumSlateBlue;

        private int borderSize = 1;

        private ComboBox cmbList;

        private Label lblText;

        private Button btnIcon;

        private Button btnSRC;

        public event EventHandler SelectedIndexChanged;
        public event DrawItemEventHandler DrawItem;


        [Category("RJ Code - Appearance")]
        public new Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
                lblText.BackColor = backColor;
                btnIcon.BackColor = backColor;
            }
        }

        [Category("RJ Code - Appearance")]
        public Color IconColor
        {
            get
            {
                return iconColor;
            }
            set
            {
                iconColor = value;
                btnIcon.Invalidate();
            }
        }

        [Category("RJ Code - Appearance")]
        public Color ListBackColor
        {
            get
            {
                return listBackColor;
            }
            set
            {
                listBackColor = value;
                cmbList.BackColor = listBackColor;
            }
        }

        [Category("RJ Code - Appearance")]
        public Color ListTextColor
        {
            get
            {
                return listTextColor;
            }
            set
            {
                listTextColor = value;
                cmbList.ForeColor = listTextColor;
            }
        }

        [Category("RJ Code - Appearance")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                base.BackColor = borderColor;
            }
        }

        [Category("RJ Code - Appearance")]
        public int BorderThickness
        {
            get
            {
                return borderSize;
            }
            set
            {
                borderSize = value;
                base.Padding = new Padding(borderSize);
                AdjustComboBoxDimensions();
            }
        }

        [Category("RJ Code - Appearance")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                lblText.ForeColor = value;
            }
        }

        [Category("RJ Code - Appearance")]
        public new Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                lblText.Font = value;
                cmbList.Font = value;
            }
        }

        [Category("RJ Code - Appearance")]
        public new  string Text
        {
            get
            {
                return lblText.Text;
            }
            set
            {
                lblText.Text = value;
            }
        }

        [Category("RJ Code - Appearance")]
        public ComboBoxStyle DropDownStyle
        {
            get
            {
                return cmbList.DropDownStyle;
            }
            set
            {
                if (cmbList.DropDownStyle != 0)
                {
                    cmbList.DropDownStyle = value;
                }
            }
        }

        [Category("RJ Code - Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [MergableProperty(false)]
        public ComboBox.ObjectCollection Items => cmbList.Items;

        [Category("RJ Code - Data")]
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue(null)]
        public object DataSource
        {
            get
            {
                return cmbList.DataSource;
            }
            set
            {
                cmbList.DataSource = value;
            }
        }

        [Category("RJ Code - Data")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Localizable(true)]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get
            {
                return cmbList.AutoCompleteCustomSource;
            }
            set
            {
                cmbList.AutoCompleteCustomSource = value;
            }
        }

        [Category("RJ Code - Data")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteSource.None)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public AutoCompleteSource AutoCompleteSource
        {
            get
            {
                return cmbList.AutoCompleteSource;
            }
            set
            {
                cmbList.AutoCompleteSource = value;
            }
        }

        [Category("RJ Code - Data")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteMode.None)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public AutoCompleteMode AutoCompleteMode
        {
            get
            {
                return cmbList.AutoCompleteMode;
            }
            set
            {
                cmbList.AutoCompleteMode = value;
            }
        }

        [Category("RJ Code - Data")]
        [Bindable(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedItem
        {
            get
            {
                return cmbList.SelectedItem;
            }
            set
            {
                cmbList.SelectedItem = value;
            }
        }

        [Category("RJ Code - Data")]
        [Bindable(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue
        {
            get
            {
                return cmbList.SelectedValue;
            }
            set
            {
                cmbList.SelectedValue = value;
            }
        }

        [Category("RJ Code - Data")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                return cmbList.SelectedIndex;
            }
            set
            {
                cmbList.SelectedIndex = value;
            }
        }

        [Category("RJ Code - Data")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string DisplayMember
        {
            get
            {
                return cmbList.DisplayMember;
            }
            set
            {
                cmbList.DisplayMember = value;
            }
        }

        [Category("RJ Code - Data")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string ValueMember
        {
            get
            {
                return cmbList.ValueMember;
            }
            set
            {
                cmbList.ValueMember = value;
            }
        }

        public bool VisibleButtonOption
        {
            get
            {
                return btnSRC.Visible;
            }
            set
            {
                btnSRC.Visible = value;
                Invalidate();
            }
        }

        public int WidthButton
        {
            get
            {
                return btnSRC.Width;
            }
            set
            {
                btnSRC.Width = value;
                Invalidate();
            }
        }

        public Image ButtonImage
        {
            get
            {
                return btnSRC.Image;
            }
            set
            {
                btnSRC.Image = value;
                Invalidate();
            }
        }

        public bool DroppedDown
        {
            get
            {
                return cmbList.DroppedDown;
            }
            set
            {
                cmbList.DroppedDown = value;
            }
        }

        public ComboBox GetComboBox => cmbList;

        public DrawMode DrawMode { get=>this.cmbList.DrawMode; set => cmbList.DrawMode = value;}

        [Category("Action")]
        public event EventHandler ButtonOptionClick
        {
            add
            {
                btnSRC.Click += value;
            }
            remove
            {
                btnSRC.Click -= value;
            }
        }

        public RJComboBox()
        {
            cmbList = new ComboBox();
            lblText = new Label();
            btnIcon = new Button();
            btnSRC = new Button();
            SuspendLayout();


            cmbList.BackColor = listBackColor;
            cmbList.Font = new Font(Font.Name, 10f);
            cmbList.ForeColor = listTextColor;
            cmbList.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            cmbList.TextChanged += ComboBox_TextChanged;
            cmbList.DrawItem += new DrawItemEventHandler(OnDrawItem);
            cmbList.DrawMode = DrawMode.OwnerDrawFixed;


            btnIcon.Dock = DockStyle.Right;
            btnIcon.FlatStyle = FlatStyle.Flat;
            btnIcon.FlatAppearance.BorderSize = 0;
            btnIcon.BackColor = backColor;
            btnIcon.Size = new Size(30, 30);
            btnIcon.Cursor = Cursors.Hand;
            btnIcon.Click += Icon_Click;
            btnIcon.Paint += Icon_Paint;
            btnSRC.Dock = DockStyle.Right;
            btnSRC.FlatStyle = FlatStyle.Flat;
            btnSRC.FlatAppearance.BorderSize = 0;
            btnSRC.BackColor = backColor;
            btnSRC.Size = new Size(30, 30);
            btnSRC.Cursor = Cursors.Hand;
            lblText.Dock = DockStyle.Fill;
            lblText.AutoSize = false;
            lblText.BackColor = backColor;
            lblText.TextAlign = ContentAlignment.MiddleLeft;
            lblText.Padding = new Padding(8, 0, 0, 0);
            lblText.Font = new Font(Font.Name, 10f);
            lblText.Click += Surface_Click;
            lblText.MouseEnter += Surface_MouseEnter;
            lblText.MouseLeave += Surface_MouseLeave;
            base.Controls.Add(lblText);
            base.Controls.Add(btnIcon);
            base.Controls.Add(btnSRC);
            base.Controls.Add(cmbList);
            MinimumSize = new Size(200, 30);
            base.Size = new Size(200, 30);
            ForeColor = Color.DimGray;
            base.Padding = new Padding(borderSize);
            Font = new Font(Font.Name, 10f);
            base.BackColor = borderColor;
            ResumeLayout();
            AdjustComboBoxDimensions();
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (DrawItem != null)
            {
                DrawItem(sender, e);
            }
            else if (e.Index >= 0)
            {
                // La lógica de dibujo por defecto
                string itemText = cmbList.GetItemText(cmbList.Items[e.Index]);
                e.DrawBackground();
                e.Graphics.DrawString(itemText, e.Font, new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }



        private void AdjustComboBoxDimensions()
        {
            cmbList.Width = lblText.Width;
            cmbList.Location = new Point
            {
                X = 3,
                Y = lblText.Bottom - cmbList.Height
            };
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(sender, e);
            }

            lblText.Text = cmbList.Text;
        }

        private void Icon_Paint(object sender, PaintEventArgs e)
        {
            //Fields
            int iconWidht = 14;
            int iconHeight = 6;
            var rectIcon = new Rectangle((btnIcon.Width - iconWidht) / 2, (btnIcon.Height - iconHeight) / 2, iconWidht, iconHeight);
            Graphics graph = e.Graphics;

            //Draw arrow down icon
            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(iconColor, 2))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                path.AddLine(rectIcon.X, rectIcon.Y, rectIcon.X + (iconWidht / 2), rectIcon.Bottom);
                path.AddLine(rectIcon.X + (iconWidht / 2), rectIcon.Bottom, rectIcon.Right, rectIcon.Y);
                graph.DrawPath(pen, path);
            }
        }

        private void Icon_Click(object sender, EventArgs e)
        {
            cmbList.Select();
            cmbList.DroppedDown = true;
        }

        private void Surface_Click(object sender, EventArgs e)
        {
            OnClick(e);
            cmbList.Select();
            cmbList.DroppedDown = true;
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            lblText.Text = cmbList.Text;
        }

        private void Surface_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }

        private void Surface_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustComboBoxDimensions();
        }
    }
}
