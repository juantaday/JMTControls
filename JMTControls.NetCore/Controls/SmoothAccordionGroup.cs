using JMTControls.NetCore.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    // ═══════════════════════════════════════════════════════════════════
    //  SmoothAccordionGroup  v4  — fix colapso total
    //
    //  Se obliga al control a respetar siempre Math.Max(HeaderHeight, h)
    //  para evitar que implosione y desaparezca cuando no tiene ítems.
    // ═══════════════════════════════════════════════════════════════════
    public class SmoothAccordionGroup : Panel
    {
        public string Title { get; set; } = "Grupo";
        public Image GroupIcon { get; set; }
        public Color HeaderColor { get; set; } = Color.FromArgb(40, 40, 48);
        public Color HeaderText { get; set; } = Color.White;
        public int HeaderHeight { get; set; } = 40;

        public IEnumerable<Control> ContentControls =>
            _content.Controls.Cast<Control>();

        private bool _expanded = true;
        private int _collapsedH;
        private int _expandedH;
        private double _animCurrent;
        private int _animTarget;

        private readonly Panel _content;
        private readonly System.Windows.Forms.Timer _timer;

        private const int INTERVAL = 8;
        private const double EASE = 0.22;
        private const double SNAP = 1.5;

        public SmoothAccordionGroup()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            _content = new DoubleBufferedPanel
            {
                Left = 0,
                Top = HeaderHeight,
                Dock = DockStyle.None,
                Padding = new Padding(0, 2, 0, 4),
                BackColor = Color.FromArgb(28, 28, 34),
            };

            _timer = new System.Windows.Forms.Timer { Interval = INTERVAL };
            _timer.Tick += OnTick;

            Controls.Add(_content);
            MouseClick += (s, e) => { if (e.Y < HeaderHeight) Toggle(); };
            RecalcHeights(resync: true);
        }

        // ── API ──────────────────────────────────────────────────────────

        public void AddContent(Control ctrl)
        {
            ctrl.Dock = DockStyle.Top;
            _content.Controls.Add(ctrl);
            // FIX ORDEN: DockStyle.Top apila al revés. SendToBack hace que el
            // último agregado quede debajo, preservando el orden de inserción.
            ctrl.SendToBack();
            RecalcHeights(resync: true);
        }

        public void Toggle()
        {
            if (_expanded) Collapse();
            else Expand();
        }

        public void Expand(bool animate = true)
        {
            if (_expanded && !_timer.Enabled) return;

            _expanded = true;
            _animTarget = _expandedH;

            if (animate)
            {
                if (!_timer.Enabled) _timer.Start();
            }
            else
            {
                _animCurrent = _expandedH;
                ApplyHeight(_expandedH); // Usamos ApplyHeight para proteger la asignación
            }

            Invalidate();
        }

        public void Collapse(bool animate = true)
        {
            if (!_expanded && !_timer.Enabled) return;

            _expanded = false;
            _animTarget = _collapsedH;

            if (animate)
            {
                if (!_timer.Enabled) _timer.Start();
            }
            else
            {
                _animCurrent = _collapsedH;
                ApplyHeight(_collapsedH); // Usamos ApplyHeight para proteger la asignación
            }

            Invalidate();
        }

        // ── animación ────────────────────────────────────────────────────

        private void OnTick(object s, EventArgs e)
        {
            double diff = _animTarget - _animCurrent;

            if (Math.Abs(diff) < SNAP)
            {
                _animCurrent = _animTarget;
                _timer.Stop();
                ApplyHeight(_animTarget);
                Invalidate();
                return;
            }

            _animCurrent += diff * EASE;
            ApplyHeight((int)_animCurrent);
        }

        private void ApplyHeight(int h)
        {
            Parent?.SuspendLayout();

            // EL FIX PRINCIPAL ESTÁ AQUÍ: 
            // Jamás permitimos que la altura sea menor a la de la cabecera.
            Height = Math.Max(HeaderHeight, h);

            Parent?.ResumeLayout(false);
            Parent?.PerformLayout();
        }

        // ── pintura ──────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawHeader(g);
        }

        private void DrawHeader(Graphics g)
        {
            using var path = new System.Drawing.Drawing2D.GraphicsPath();
            const int R = 5;
            int bh = HeaderHeight;
            int w = Math.Max(1, Width);

            path.AddArc(0, 0, R * 2, R * 2, 180, 90);
            path.AddArc(w - R * 2, 0, R * 2, R * 2, 270, 90);

            bool roundBottom = !_expanded && !_timer.Enabled;
            if (roundBottom)
            {
                path.AddArc(w - R * 2, bh - R * 2, R * 2, R * 2, 0, 90);
                path.AddArc(0, bh - R * 2, R * 2, R * 2, 90, 90);
            }
            else
            {
                path.AddLine(w, bh, 0, bh);
            }
            path.CloseFigure();

            using var bg = new SolidBrush(HeaderColor);
            g.FillPath(bg, path);

            int iconX = 10;
            int iconY = (bh - 16) / 2;
            if (GroupIcon != null)
            {
                g.DrawImage(GroupIcon, new Rectangle(iconX, iconY, 16, 16));
                iconX += 22;
            }
            else
            {
                using var ib = new SolidBrush(Color.FromArgb(80, 255, 255, 255));
                g.FillRectangle(ib, iconX + 2, iconY + 2, 12, 12);
                iconX += 22;
            }

            using var font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            using var tb = new SolidBrush(HeaderText);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
            };
            g.DrawString(Title, font, tb,
                new RectangleF(iconX, 0, w - iconX - 24, bh), sf);

            DrawChevron(g, _expanded);
        }

        private void DrawChevron(Graphics g, bool down)
        {
            int cx = Width - 14;
            int cy = HeaderHeight / 2;

            using var pen = new Pen(Color.FromArgb(180, 255, 255, 255), 1.5f)
            {
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round,
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
            };

            if (down)
                g.DrawLines(pen, new[] {
                    new PointF(cx - 5, cy - 2),
                    new PointF(cx,     cy + 3),
                    new PointF(cx + 5, cy - 2),
                });
            else
                g.DrawLines(pen, new[] {
                    new PointF(cx - 3, cy - 5),
                    new PointF(cx + 3, cy),
                    new PointF(cx - 3, cy + 5),
                });
        }

        // ── recalcular alturas ────────────────────────────────────────────

        private void RecalcHeights(bool resync)
        {
            _collapsedH = HeaderHeight;
            _content.Top = HeaderHeight;
            _content.Width = Math.Max(1, Width > 0 ? Width : 280);

            _content.PerformLayout();

            int contentH = 0;
            foreach (Control c in _content.Controls)
            {
                int bottom = c.Top + c.Height + c.Margin.Bottom;
                if (bottom > contentH) contentH = bottom;
            }
            contentH += _content.Padding.Bottom + 4;

            const int MIN_CONTENT_H = 60;
            _content.Height = Math.Max(contentH, MIN_CONTENT_H);
            _expandedH = _collapsedH + _content.Height;

            if (resync && !_timer.Enabled)
            {
                _animCurrent = _expanded ? _expandedH : _collapsedH;
                ApplyHeight((int)_animCurrent); // Usamos ApplyHeight en lugar de Height = ...
            }
        }

        // ── resize ───────────────────────────────────────────────────────

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_content != null)
                _content.Width = Math.Max(1, ClientSize.Width);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            if (_content != null)
                _content.Width = Math.Max(1, ClientSize.Width);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _timer?.Dispose();
            base.Dispose(disposing);
        }
    }

    internal class DoubleBufferedPanel : Panel
    {
        [Category("Acción")]
        [Description("Se dispara cuando el usuario hace clic en un ítem específico del menú.")]
        public event EventHandler<SidebarItemClickEventArgs> ItemClicked;

        public DoubleBufferedPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is DoubleBufferedPanel itemControl)
            {
                itemControl.ItemClicked += (s, args) =>
                {
                    ItemClicked?.Invoke(this, args);
                };
            }
        }

    }

    
}