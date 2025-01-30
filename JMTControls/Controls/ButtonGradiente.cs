using JMControls.Enums;
using JMControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMControls.Controls
{
    [Description("Text")]
    public class ButtonGradiente : Control, IButtonControl
    {
        #region Variables
        int radius;
        bool transparency;
        MouseState state;
        RoundedRectangleF roundedRect;
        Color inactive1, inactive2, active1, active2;
        private Color strokeColor;
        private bool stroke;
        private Color borderColorHover;
        private Color borderColorActive;
        private Color borderColorIdle;
        private Color borderColorDisable;
        private int borderThickness;
        DialogResult dialogoResult;
        private BorderStyle borderStyle;

       
        #endregion
        #region AltoButton
        public ButtonGradiente()
        {
            Width = 65;
            Height = 30;
            stroke = false;
            strokeColor = Color.Gray;
            inactive1 = Color.FromArgb(44, 188, 210);
            inactive2 = Color.FromArgb(33, 167, 188);
            active1 = Color.FromArgb(64, 168, 183);
            active2 = Color.FromArgb(36, 164, 183);

            radius = 10;
            roundedRect = new RoundedRectangleF(Width, Height, radius);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            ForeColor = Color.Black;
            Font = new System.Drawing.Font("Comic Sans MS", 10, FontStyle.Bold);
            state = MouseState.Leave;

            transparency = false;
            dialogoResult = DialogResult.None;
            borderThickness = 1;

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            #region Transparency
            if (transparency)
                Transparenter.MakeTransparent(this, e.Graphics);
            #endregion

            #region Drawing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            roundedRect = new RoundedRectangleF(Width - 1, Height - 1, radius);
            e.Graphics.FillRectangle(Brushes.Transparent, this.ClientRectangle);

            int R1 = (active1.R + inactive1.R) / 2;
            int G1 = (active1.G + inactive1.G) / 2;
            int B1 = (active1.B + inactive1.B) / 2;

            int R2 = (active2.R + inactive2.R) / 2;
            int G2 = (active2.G + inactive2.G) / 2;
            int B2 = (active2.B + inactive2.B) / 2;

            Rectangle rect = new Rectangle(0, 0, Width, Height);

            if (this.Enabled)
            {
                if (state == MouseState.Leave)
                {
                    using (LinearGradientBrush inactiveGB = new LinearGradientBrush(rect, inactive1, inactive2, 90f))
                        e.Graphics.FillPath(inactiveGB, roundedRect.Path);

                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorIdle, BorderThickness))
                        using (GraphicsPath path = new RoundedRectangleF(Width - 1, Height - 1, radius).Path)
                            e.Graphics.DrawPath(pen, path);
                }
                else if (state == MouseState.Enter)
                {
                    using (LinearGradientBrush activeGB = new LinearGradientBrush(rect, active1, active2, 90f))
                        e.Graphics.FillPath(activeGB, roundedRect.Path);

                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorHover, BorderThickness))
                        using (GraphicsPath path = new RoundedRectangleF(Width - 1, Height - 1, radius).Path)
                            e.Graphics.DrawPath(pen, path);
                }
                else if (state == MouseState.Down)
                {
                    using (LinearGradientBrush downGB = new LinearGradientBrush(rect, Color.FromArgb(R1, G1, B1), Color.FromArgb(R2, G2, B2), 90f))
                        e.Graphics.FillPath(downGB, roundedRect.Path);

                    if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                        using (Pen pen = new Pen(BorderColorActive, BorderThickness))
                        using (GraphicsPath path = new RoundedRectangleF(Width - 1, Height - 1, radius).Path)
                            e.Graphics.DrawPath(pen, path);
                }
            }
            else
            {
                Color linear1 = Color.FromArgb(190, 190, 190);
                Color linear2 = Color.FromArgb(210, 210, 210);
                using (LinearGradientBrush inactiveGB = new LinearGradientBrush(rect, linear1, linear2, 90f))
                {
                    e.Graphics.FillPath(inactiveGB, roundedRect.Path);
                    e.Graphics.DrawPath(new Pen(inactiveGB), roundedRect.Path);
                }

                if (BorderStyle != BorderStyle.None && BorderThickness > 0)
                    using (Pen pen = new Pen(BorderColorDisable, BorderThickness))
                    using (GraphicsPath path = new RoundedRectangleF(Width - 1, Height - 1, radius).Path)
                        e.Graphics.DrawPath(pen, path);
            }
            #endregion

            #region Text Drawing
            using (StringFormat sf = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            })
            using (Brush brush = new SolidBrush(ForeColor))
                e.Graphics.DrawString(Text, Font, brush, this.ClientRectangle, sf);
            #endregion
            base.OnPaint(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            base.OnClick(e);
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }
        protected override void OnResize(EventArgs e)
        {
            Invalidate();
            base.OnResize(e);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            state = MouseState.Enter;
            base.OnMouseEnter(e);
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            state = MouseState.Leave;
            base.OnMouseLeave(e);
            Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Capture = false;
            state = MouseState.Down;
            base.OnMouseDown(e);
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (state != MouseState.Leave)
                state = MouseState.Enter;
            base.OnMouseUp(e);
            Invalidate();
        }
        #endregion
        #region Properties

        public bool Stroke
        {
            get { return stroke; }
            set
            {
                stroke = value;
                Invalidate();
            }
        }

        public Color BorderColorActive
        {
            get { return borderColorActive; }
            set
            {
                borderColorActive = value;
                Invalidate();
            }

        }

        public Color BorderColorDisable
        {
            get { return borderColorDisable; }
            set
            {
                borderColorDisable = value;
                Invalidate();
            }

        }
        public Color BorderColorIdle
        {
            get { return borderColorIdle; }
            set
            {
                borderColorIdle = value;
                Invalidate();
            }

        }

        public Color BorderColorHover
        {
            get { return borderColorHover; }
            set
            {
                borderColorHover = value;
                Invalidate();
            }

        }
        public Color StrokeColor
        {
            get { return strokeColor; }
            set
            {
                strokeColor = value;
                Invalidate();
            }
        }

        public BorderStyle BorderStyle { get => borderStyle; set {
                borderStyle = value;
                Invalidate();
            }
        }

        public int BorderThickness { get => borderThickness; 
            set {
                borderThickness = value;
                Invalidate();
               }
        }

        public int Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                Invalidate();
            }
        }
        public Color Inactive1
        {
            get
            {
                return inactive1;
            }
            set
            {
                inactive1 = value;
                Invalidate();
            }
        }
        public Color Inactive2
        {
            get
            {
                return inactive2;
            }
            set
            {
                inactive2 = value;
                Invalidate();
            }
        }
        public Color Active1
        {
            get
            {
                return active1;
            }
            set
            {
                active1 = value;
                Invalidate();
            }
        }
        public Color Active2
        {
            get
            {
                return active2;
            }
            set
            {
                active2 = value;
                Invalidate();
            }
        }
        public bool Transparency
        {
            get
            {
                return transparency;
            }
            set
            {
                transparency = value;
            }
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                Invalidate();
            }
        }

        public DialogResult DialogResult
        {
            get
            {
                return dialogoResult;
            }
            set
            {
                dialogoResult = value;
            }
        }

        public void NotifyDefault(bool value)
        {
        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }
        #endregion
    }
  
}
