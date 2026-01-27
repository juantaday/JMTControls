using JMTControls.NetCore.Helpers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class GroupBoxLiner : System.Windows.Forms.GroupBox
    {
        private Color _borderColor = Color.Black;
        private int radius;
        private int borderThickness;

        public GroupBoxLiner()
        {
            BorderRadius = 8;
            BorderThickness = 1;
        }

        public Color BorderColor
        {
            get { return this._borderColor; }
            set { this._borderColor = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Usar una fuente que soporte emojis
            Font textFont = this.Font;
            bool useEmojiFont = ContainsEmoji(this.Text);

            if (useEmojiFont)
            {
                // Intentar usar fuentes que soporten emojis
                textFont = new Font("Segoe UI Emoji", this.Font.Size, this.Font.Style);

                // Fallback si Segoe UI Emoji no está disponible
                if (textFont.Name != "Segoe UI Emoji")
                {
                    textFont.Dispose();
                    textFont = new Font("Segoe UI Symbol", this.Font.Size, this.Font.Style);
                }
            }

            SizeF tSizeF = e.Graphics.MeasureString(this.Text, textFont);
            Size tSize = new Size((int)Math.Ceiling(tSizeF.Width), (int)Math.Ceiling(tSizeF.Height));

            int _Y = tSize.Height / 2;
            Rectangle borderRect = new Rectangle(new Point(2, _Y),
                new Size
                {
                    Height = this.Height - (3 + _Y),
                    Width = this.Width - (3 + borderThickness)
                });

            using (Pen pen = new Pen(BorderColor, BorderThickness))
            {
                if (BorderRadius == 0)
                {
                    Rectangle path = new Rectangle(
                        new Point(borderThickness, _Y + borderThickness),
                        new Size
                        {
                            Height = borderRect.Height - (BorderThickness * 2),
                            Width = borderRect.Width - (BorderThickness * 2)
                        });
                    e.Graphics.DrawRectangle(pen, path);
                }
                else
                {
                    using (GraphicsPath path = new MakeRoundedRect(borderRect, this.BorderRadius, this.BorderRadius, false, true, false, true).Path)
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }

            Rectangle textRect = new Rectangle(
                6,
                0,
                tSize.Width + 6,
                tSize.Height
            );

            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
            e.Graphics.DrawString(this.Text, textFont, new SolidBrush(this.ForeColor), textRect.Location);

            if (useEmojiFont && textFont != this.Font)
            {
                textFont.Dispose();
            }
        }

        private bool ContainsEmoji(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            foreach (char c in text)
            {
                // Detectar rangos Unicode de emojis
                int codePoint = char.ConvertToUtf32(text, text.IndexOf(c));
                if (codePoint >= 0x1F300 && codePoint <= 0x1F9FF) // Emojis comunes
                    return true;
                if (codePoint >= 0x2600 && codePoint <= 0x26FF) // Símbolos varios
                    return true;
                if (codePoint >= 0x2700 && codePoint <= 0x27BF) // Dingbats
                    return true;
            }
            return false;
        }

        public int BorderRadius
        {
            get { return radius; }
            set
            {
                radius = value;
                Invalidate();
            }
        }

        public int BorderThickness
        {
            get => borderThickness;
            set
            {
                borderThickness = value;
                Invalidate();
            }
        }
    }
}