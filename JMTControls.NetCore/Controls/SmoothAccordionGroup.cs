using JMTControls.NetCore.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    // ═══════════════════════════════════════════════════════════════════
    //  SmoothAccordionGroup  — DevExpress-style accordion panel
    // ═══════════════════════════════════════════════════════════════════
    public class SmoothAccordionGroup : Panel
    {
        // ── Propiedades públicas ─────────────────────────────────────────
        public string Title { get; set; } = "Grupo";
        public Image GroupIcon { get; set; }
        public Color HeaderColor { get; set; } = Color.FromArgb(40, 40, 50);
        public Color HeaderText { get; set; } = Color.White;
        public int HeaderHeight { get; set; } = 38;
        public bool IsExpanded => _expanded;

        public IEnumerable<Control> ContentControls => _content.Controls.Cast<Control>();

        // ── Estado de animación ──────────────────────────────────────────
        private bool _expanded = true;
        private double _animCurrent;
        private int _animTarget;
        private int _collapsedH;
        private int _expandedH;

        private readonly Panel _content;
        private readonly System.Windows.Forms.Timer _timer;

        private bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime || Site?.DesignMode == true;

        private const int INTERVAL = 8;
        private const double EASE = 0.25;
        private const double SNAP = 1.5;

        // ── Constructor ──────────────────────────────────────────────────
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
            _timer.Tick += OnAnimTick;

            Controls.Add(_content);
            MouseClick += (s, e) => { if (e.Y < HeaderHeight) Toggle(); };

            RecalcHeights(resync: true);
        }

        // ── API pública ──────────────────────────────────────────────────

        public void AddContent(Control ctrl)
        {
            ctrl.Dock = DockStyle.Top;
            _content.Controls.Add(ctrl);
            ctrl.SendToBack(); // preserva orden de inserción con DockStyle.Top
            RecalcHeights(resync: true);
        }

        public void Toggle()
        {
            bool animate = !IsInDesignMode;
            if (_expanded) Collapse(animate); else Expand(animate);
        }

        public void Expand(bool animate = true)
        {
            _expanded = true;
            _animTarget = _expandedH;

            if (animate) { if (!_timer.Enabled) _timer.Start(); }
            else { _animCurrent = _expandedH; ApplyHeight(_expandedH); }

            Invalidate();
        }

        public void Collapse(bool animate = true)
        {
            _expanded = false;
            _animTarget = _collapsedH;

            if (animate) { if (!_timer.Enabled) _timer.Start(); }
            else { _animCurrent = _collapsedH; ApplyHeight(_collapsedH); }

            Invalidate();
        }

        // ── Animación ────────────────────────────────────────────────────

        private void OnAnimTick(object s, EventArgs e)
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
            int newHeight = Math.Max(HeaderHeight, h);
            if (Height == newHeight) return;

            Height = newHeight;
            Invalidate();
        }

        // ── Pintura ──────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawHeader(g);
        }

        private void DrawHeader(Graphics g)
        {
            const int R = 4;
            int w = Math.Max(1, Width);
            int bh = HeaderHeight;

            using var path = new GraphicsPath();
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

            // Fondo del header con gradiente sutil
            using var grad = new LinearGradientBrush(
                new Rectangle(0, 0, w, bh),
                Color.FromArgb(52, 52, 64),
                HeaderColor,
                LinearGradientMode.Vertical);
            g.FillPath(grad, path);

            // Borde inferior del header
            using var sep = new Pen(Color.FromArgb(60, 60, 75), 1f);
            if (!roundBottom)
                g.DrawLine(sep, 0, bh - 1, w, bh - 1);

            // Ícono o placeholder
            int iconX = 10;
            int iconY = (bh - 16) / 2;

            if (GroupIcon != null)
            {
                g.DrawImage(GroupIcon, new Rectangle(iconX, iconY, 16, 16));
                iconX += 22;
            }
            else
            {
                // Pequeño cuadrado de color como placeholder
                using var ib = new SolidBrush(Color.FromArgb(70, 100, 140, 255));
                using var ip = new Pen(Color.FromArgb(100, 140, 255), 1.2f);
                g.FillRectangle(ib, iconX + 2, iconY + 2, 12, 12);
                g.DrawRectangle(ip, iconX + 2, iconY + 2, 12, 12);
                iconX += 22;
            }

            // Título
            using var font = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var tb = new SolidBrush(HeaderText);
            using var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
            };
            g.DrawString(Title, font, tb, new RectangleF(iconX, 0, w - iconX - 28, bh), sf);

            // Chevron
            DrawChevron(g);
        }

        private void DrawChevron(Graphics g)
        {
            int cx = Width - 14;
            int cy = HeaderHeight / 2;

            using var pen = new Pen(Color.FromArgb(160, 255, 255, 255), 1.5f)
            {
                LineJoin = LineJoin.Round,
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
            };

            if (_expanded)
                g.DrawLines(pen, new[] {
                    new PointF(cx - 5, cy - 2),
                    new PointF(cx,     cy + 3),
                    new PointF(cx + 5, cy - 2),
                });
            else
                g.DrawLines(pen, new[] {
                    new PointF(cx - 3, cy - 4),
                    new PointF(cx + 3, cy),
                    new PointF(cx - 3, cy + 4),
                });
        }

        // ── Recalcular alturas ────────────────────────────────────────────

        private void RecalcHeights(bool resync)
        {
            _collapsedH = HeaderHeight;
            _content.Top = HeaderHeight;
            _content.Width = Math.Max(1, Width > 0 ? Width : 280);

            int contentH = _content.Padding.Top;
            foreach (Control c in _content.Controls)
                contentH += c.Height + c.Margin.Top + c.Margin.Bottom;
            contentH += _content.Padding.Bottom + 4;
            _content.Height = Math.Max(contentH, 20);
            _expandedH = _collapsedH + _content.Height;

            if (resync && !_timer.Enabled)
            {
                _animCurrent = _expanded ? _expandedH : _collapsedH;
                ApplyHeight((int)_animCurrent);
            }
        }

        // ── Resize / Layout ──────────────────────────────────────────────

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

    // ═══════════════════════════════════════════════════════════════════
    //  DoubleBufferedPanel
    // ═══════════════════════════════════════════════════════════════════
    internal class DoubleBufferedPanel : Panel
    {
        public event EventHandler<SidebarItemClickEventArgs> ItemClicked;

        public DoubleBufferedPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is DoubleBufferedPanel inner)
                inner.ItemClicked += (s, args) => ItemClicked?.Invoke(this, args);
        }
    }
}