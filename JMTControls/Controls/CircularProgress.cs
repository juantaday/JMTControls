using System.Threading.Tasks;

namespace JMControls.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Markup;

    [DefaultEvent("ProgressCompleted")]
    public class CircularProgress : Control
    {
        // Fields
        private int progressValue = 0;
        private int progressMax = 100;
        private Color progressColor = Color.Blue;
        private Color outerColor = Color.Gray;
        private Color centerColor = Color.LightGray;
        private int borderWidth = 10;
        private int outerWidth = 15;
        private bool _hideVisibilityOnCompleted = false;
        // Event for progress completion
        public event EventHandler ProgressCompleted;

        // Properties
        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                if (value < 0) value = 0;
                if (value > progressMax) value = progressMax;
                progressValue = value;
                this.Invalidate(); // Redraw control

                if (progressValue == progressMax)
                {
                    OnProgressCompleted(); // Trigger event when 100% is reached
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

        public Color OuterColor
        {
            get { return outerColor; }
            set
            {
                outerColor = value;
                this.Invalidate();
            }
        }

        public Color CenterColor
        {
            get { return centerColor; }
            set
            {
                centerColor = value;
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

        public int OuterWidth
        {
            get { return outerWidth; }
            set
            {
                outerWidth = value > 0 ? value : 1;
                this.Invalidate();
            }
        }

        // Constructor
        public CircularProgress()
        {
            this.Size = new Size(150, 150);
            this.DoubleBuffered = true;
        }

        // Overridden OnPaint method
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Calculate rectangles for drawing
            var rectOuter = new Rectangle(outerWidth / 2, outerWidth / 2, this.Width - outerWidth, this.Height - outerWidth);
            var rectInner = new Rectangle(outerWidth, outerWidth, this.Width - 2 * outerWidth, this.Height - 2 * outerWidth);

            // Draw outer circle
            using (var outerPen = new Pen(outerColor, outerWidth))
            {
                g.DrawEllipse(outerPen, rectOuter);
            }

            // Draw progress arc
            using (var pen = new Pen(progressColor, borderWidth))
            {
                float sweepAngle = 360f * progressValue / progressMax;
                g.DrawArc(pen, rectInner, -90, sweepAngle);
            }

            // Draw center circle (empty space)
            var rectCenter = new Rectangle(
                outerWidth + borderWidth,
                outerWidth + borderWidth,
                this.Width - 2 * (outerWidth + borderWidth),
                this.Height - 2 * (outerWidth + borderWidth)
            );

            using (var brush = new SolidBrush(centerColor))
            {
                g.FillEllipse(brush, rectCenter);
            }
        }

        // Raise ProgressCompleted event
        protected virtual void OnProgressCompleted()
        {
            ProgressCompleted?.Invoke(this, EventArgs.Empty);
        }


        // Asynchronous progress update
        public async void UpdateProgressAsync(int targetValue, int duration, bool hideVisibilityOnCompleted = false)
        {
            if (hideVisibilityOnCompleted ) 
                this.Visible = true;

            ProgressValue = 0;
            _hideVisibilityOnCompleted = hideVisibilityOnCompleted;


            if (targetValue < 0) targetValue = 0;
            if (targetValue > progressMax) targetValue = progressMax;

            int startValue = progressValue;
            int totalSteps = Math.Abs(targetValue - startValue);
            if (totalSteps == 0) return;

            int stepIncrement = Math.Sign(targetValue - startValue); // Dirección del progreso (+1 o -1)
            int idealStepDuration = duration / totalSteps; // Tiempo ideal por paso

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i <= totalSteps; i++)
            {
                // Actualiza el progreso
                var elapsedValue = startValue + i * 3;
                if (elapsedValue >= 100)
                {
                    ProgressValue = 100;
                    return;
                }
                
                ProgressValue = elapsedValue;

                // Calcula cuánto tiempo esperar para mantener el tiempo objetivo
                int elapsed = (int)stopwatch.ElapsedMilliseconds;
                int remaining = idealStepDuration - elapsed;
                if (remaining > 0)
                    await Task.Delay(remaining);

                // Reinicia el cronómetro para el siguiente paso
                stopwatch.Restart();
            }

            stopwatch.Stop();
        }


    }

}
