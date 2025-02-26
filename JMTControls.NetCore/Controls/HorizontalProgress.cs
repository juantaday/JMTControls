namespace JMTControls.NetCore.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    [DefaultEvent("ProgressCompleted")]
    public class HorizontalProgress : Control
    {
        private int progressValue = 0;
        private int progressMax = 100;
        private Color progressColor = Color.Blue;
        private Color backgroundColor = Color.LightGray;
        private int borderWidth = 2;
        private bool _hideVisibilityOnCompleted = false;

        public event EventHandler ProgressCompleted;

        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                if (value < 0) value = 0;
                if (value > progressMax) value = progressMax;
                progressValue = value;
                this.Invalidate();

                if (progressValue == progressMax)
                {
                    OnProgressCompleted();
                    if (_hideVisibilityOnCompleted)
                        this.Visible = false;
                }
            }
        }

        public int ProgressMax
        {
            get { return progressMax; }
            set
            {
                if (value <= 0) value = 1;
                progressMax = value;
                if (progressValue > progressMax) progressValue = progressMax;
                this.Invalidate();
            }
        }

        public Color ProgressColor
        {
            get { return progressColor; }
            set
            {
                progressColor = value;
                this.Invalidate();
            }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                this.Invalidate();
            }
        }

        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value > 0 ? value : 1;
                this.Invalidate();
            }
        }

        public HorizontalProgress()
        {
            this.Size = new Size(200, 30);
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (Brush bgBrush = new SolidBrush(backgroundColor))
            {
                g.FillRectangle(bgBrush, rect);
            }

            int fillWidth = (int)((float)progressValue / progressMax * this.Width);
            Rectangle fillRect = new Rectangle(0, 0, fillWidth, this.Height);
            using (Brush progressBrush = new SolidBrush(progressColor))
            {
                g.FillRectangle(progressBrush, fillRect);
            }
        }

        protected virtual void OnProgressCompleted()
        {
            ProgressCompleted?.Invoke(this, EventArgs.Empty);
        }

        public async void UpdateProgressAsync(int targetValue, int duration, bool hideVisibilityOnCompleted = false)
        {
            _hideVisibilityOnCompleted = hideVisibilityOnCompleted;
            if (hideVisibilityOnCompleted)
                this.Visible = true;

            if (targetValue < 0) targetValue = 0;
            if (targetValue > progressMax) targetValue = progressMax;

            int startValue = progressValue;
            int totalSteps = Math.Abs(targetValue - startValue);
            if (totalSteps == 0) return;

            int stepIncrement = Math.Sign(targetValue - startValue);
            int idealStepDuration = duration / totalSteps;

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i <= totalSteps; i++)
            {
                ProgressValue = startValue + i * stepIncrement;
                int elapsed = (int)stopwatch.ElapsedMilliseconds;
                int remaining = idealStepDuration - elapsed;
                if (remaining > 0)
                    await Task.Delay(remaining);
                stopwatch.Restart();
            }
            stopwatch.Stop();
        }
    }
}
