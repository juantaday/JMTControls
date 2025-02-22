using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class RJButton : Button
    {
        private int borderSize = 1;
        private int borderRadius = 12;
        private Color borderColor = Color.Red;
        private bool running = false;
        private Timer animationTimer;
        private float angle = 0f;

        [Category("RJ Code Advance")]
        public Color BackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }

        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get { return this.borderColor; }
            set { this.borderColor = value; base.Invalidate(); }
        }

        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get { return this.borderRadius; }
            set
            {
                if (value > base.Height)
                {
                    this.borderRadius = base.Height;
                }
                else
                {
                    this.borderRadius = value;
                }
                base.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public int BorderSize
        {
            get { return this.borderSize; }
            set { this.borderSize = value; base.Invalidate(); }
        }

        [Category("RJ Code Advance")]
        public Color TextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }

        [Category("RJ Code Advance")]
        public bool Running
        {
            get { return this.running; }
            set
            {
                this.running = value;
                if (this.running)
                {
                    this.Enabled = false; // Disable button while running
                    this.StartAnimation();
                }
                else
                {
                    this.Enabled = true; // Enable button when not running
                    this.angle = 0f; // Reset animation angle
                    this.Invalidate(); // Redraw to remove the circle
                }
            }
        }

        public new Padding Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; base.Invalidate(); }
        }

        public RJButton()
        {
            base.FlatStyle = FlatStyle.Flat;
            base.FlatAppearance.BorderSize = 0;
            base.Size = new Size(150, 40);
            this.BackColor = Color.MediumSlateBlue;
            this.ForeColor = Color.White;
            this.animationTimer = new Timer();
            this.animationTimer.Interval = 30; // Adjusted for faster animation speed
            this.animationTimer.Tick += (s, e) => { this.Invalidate(); this.angle += 20f; if (this.angle >= 360f) this.angle = 0f; };
        }

        private void StartAnimation()
        {
            if (!this.animationTimer.Enabled)
            {
                this.animationTimer.Start();
            }
        }

        private void StopAnimation()
        {
            if (this.animationTimer.Enabled)
            {
                this.animationTimer.Stop();
            }
        }

        private GraphicsPath GetFigurePath(Rectangle rect, float radius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            float single = radius * 2f;
            graphicsPath.StartFigure();
            graphicsPath.AddArc(rect.X, rect.Y, single, single, 180f, 90f);
            graphicsPath.AddArc(rect.Right - single, rect.Y, single, single, 270f, 90f);
            graphicsPath.AddArc(rect.Right - single, rect.Bottom - single, single, single, 0f, 90f);
            graphicsPath.AddArc(rect.X, rect.Bottom - single, single, single, 90f, 90f);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Rectangle clientRectangle = base.ClientRectangle;
            Rectangle rectangle = Rectangle.Inflate(clientRectangle, -this.borderSize, -this.borderSize);
            int num = 2;
            if (this.borderSize > 0)
            {
                num = this.borderSize;
            }

            if (this.borderRadius <= 2)
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.None;
                base.Region = new Region(clientRectangle);
                if (this.borderSize >= 1)
                {
                    using (Pen pen = new Pen(this.borderColor, this.borderSize))
                    {
                        pen.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
                    }
                }
            }
            else
            {
                using (GraphicsPath figurePath = this.GetFigurePath(clientRectangle, this.borderRadius))
                {
                    using (GraphicsPath graphicsPath = this.GetFigurePath(rectangle, this.borderRadius - this.borderSize))
                    {
                        using (Pen pen1 = new Pen(base.Parent.BackColor, num))
                        {
                            using (Pen pen2 = new Pen(this.borderColor, this.borderSize))
                            {
                                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                base.Region = new Region(figurePath);
                                pevent.Graphics.DrawPath(pen1, figurePath);
                                if (this.borderSize >= 1)
                                {
                                    pevent.Graphics.DrawPath(pen2, graphicsPath);
                                }
                            }
                        }
                    }
                }
            }

            if (this.running)
            {
                // Draw the loading circle on the left side of the button
                float centerX = clientRectangle.Left + 20; // Position on the left
                float centerY = clientRectangle.Top + (clientRectangle.Height / 2);
                float radius = 12f;
                using (Pen pen = new Pen(Color.White, 3))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    pevent.Graphics.DrawArc(pen, centerX - radius, centerY - radius, radius * 2, radius * 2, angle, 270f);
                }
            }
        }
    }
}
