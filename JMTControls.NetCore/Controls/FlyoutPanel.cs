using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    // ═══════════════════════════════════════════════════════════════════
    //  FlyoutPanel — panel flotante para modo sidebar colapsado
    // ═══════════════════════════════════════════════════════════════════
    public class FlyoutPanel : Form
    {
        private const int FLYOUT_W = 220;
        private const int HEADER_H = 36;
        private const int ITEM_H = 34;
        private const int PAD = 6;

        private double _opacity;
        private readonly System.Windows.Forms.Timer _fadeIn;
        private readonly SidebarContainer _owner;
        private bool _closingByItemClick;

        // ── Constructor: muestra ítems de un grupo acordeón ──────────────
        public FlyoutPanel(SidebarContainer owner, SmoothAccordionGroup group, Point screenPos)
        {
            _owner = owner;
            InitForm(screenPos);
            BuildGroupContent(group);
            AdjustHeight(group.ContentControls.Count());
            _fadeIn = BuildFadeIn();
        }

        // ── Constructor: muestra lista de tabs (sidebar colapsado) ────────
        public FlyoutPanel(List<SidebarTab> tabs, int activeIndex,
                           Point screenPos, Action<int> onTabSelected)
        {
            InitForm(screenPos);
            BuildTabList(tabs, activeIndex, onTabSelected);
            _fadeIn = BuildFadeIn();
        }

        // ── Inicialización común ─────────────────────────────────────────

        private void InitForm(Point pos)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            BackColor = Color.FromArgb(36, 36, 44);
            Location = pos;
            Width = FLYOUT_W;
            Opacity = 0;
        }

        private System.Windows.Forms.Timer BuildFadeIn()
        {
            var t = new System.Windows.Forms.Timer { Interval = 10 };
            t.Tick += (s, e) =>
            {
                _opacity += 0.18;
                if (_opacity >= 1) { _opacity = 1; t.Stop(); }
                Opacity = _opacity;
            };
            return t;
        }

        // ── Contenido: lista de tabs ──────────────────────────────────────

        private void BuildTabList(List<SidebarTab> tabs, int activeIndex, Action<int> onTabSelected)
        {
            var flow = CreateFlow();
            Controls.Add(flow);

            AddHeader("Secciones", null);
            Controls.SetChildIndex(flow, 0);

            for (int i = 0; i < tabs.Count; i++)
            {
                int idx = i;
                bool active = (i == activeIndex);
                var btn = CreateItemButton(tabs[i].Title, tabs[i].Icon, active);
                btn.Click += (s, e) => { Close(); onTabSelected?.Invoke(idx); };
                flow.Controls.Add(btn);
            }
            Height = HEADER_H + 1 + PAD * 2 + tabs.Count * ITEM_H + PAD;
        }

        // ── Contenido: ítems de un grupo ──────────────────────────────────

        private void BuildGroupContent(SmoothAccordionGroup group)
        {
            var flow = CreateFlow();
            Controls.Add(flow);

            AddHeader(group.Title, group.GroupIcon);
            Controls.SetChildIndex(flow, 0);

            foreach (Control original in group.ContentControls.OrderBy(c => c.Top))
            {
                Button srcBtn = FindFirstButton(original);
                string text = srcBtn?.Text ?? original.Text ?? "";
                if (string.IsNullOrWhiteSpace(text)) continue;

                var btn = CreateItemButton(text, srcBtn?.Image, false);

                // Al hacer clic en el clon disparamos el OnClick del original
                btn.Click += (s, e) =>
                {
                    _closingByItemClick = true;
                    Close();

                    var invoker = _owner?.FindForm() as Control ?? _owner;
                    if (invoker != null && !invoker.IsDisposed)
                        invoker.BeginInvoke((System.Windows.Forms.MethodInvoker)(() => InvokeOriginalClick(original)));
                    else
                        InvokeOriginalClick(original);
                };

                flow.Controls.Add(btn);
            }
        }

        // ── Helpers de construcción ───────────────────────────────────────

        private void AddHeader(string title, Image icon)
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = HEADER_H,
                BackColor = Color.FromArgb(50, 50, 62),
            };

            if (icon != null)
            {
                var pb = new PictureBox
                {
                    Image = icon,
                    Size = new Size(16, 16),
                    Location = new Point(10, (HEADER_H - 16) / 2),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                header.Controls.Add(pb);
            }

            var lbl = new Label
            {
                Text = title,
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Padding = new Padding(icon != null ? 34 : 12, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
            };
            header.Controls.Add(lbl);
            Controls.Add(header);

            // Separador
            Controls.Add(new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(60, 60, 72),
            });
        }

        private FlowLayoutPanel CreateFlow() => new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(PAD),
            BackColor = Color.Transparent,
            AutoScroll = false,
        };

        private Button CreateItemButton(string text, Image icon, bool active)
        {
            var btn = new Button
            {
                Text = text,
                Width = FLYOUT_W - PAD * 2 - 2,
                Height = ITEM_H,
                FlatStyle = FlatStyle.Flat,
                ForeColor = active ? Color.White : Color.FromArgb(200, 200, 215),
                BackColor = active ? Color.FromArgb(50, 50, 70) : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f, active ? FontStyle.Bold : FontStyle.Regular),
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 68);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 45, 58);

            if (icon != null)
            {
                btn.Image = icon;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(30, 0, 0, 0);
            }
            return btn;
        }

        /// <summary>Invoca OnClick en el control original vía reflexión.</summary>
        private static void InvokeOriginalClick(Control ctrl)
        {
            var method = typeof(Control).GetMethod("OnClick",
                BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(ctrl, new object[] { EventArgs.Empty });
        }

        /// <summary>Busca recursivamente el primer Button dentro de un control.</summary>
        private static Button FindFirstButton(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button b) return b;
                var inner = FindFirstButton(c);
                if (inner != null) return inner;
            }
            return null;
        }

        private void AdjustHeight(int itemCount) =>
            Height = HEADER_H + 1 + PAD * 2 + itemCount * ITEM_H + PAD;

        // ── Fade out ─────────────────────────────────────────────────────

        private void FadeOut()
        {
            _fadeIn?.Stop();
            var ft = new System.Windows.Forms.Timer { Interval = 10 };
            ft.Tick += (s, e) =>
            {
                Opacity -= 0.14;
                if (Opacity <= 0) { ft.Stop(); ft.Dispose(); Close(); }
            };
            ft.Start();
        }

        // ── Overrides ────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pen = new Pen(Color.FromArgb(70, 70, 85), 1f);
            e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _fadeIn?.Start();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            if (!_closingByItemClick)
                FadeOut();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_NOACTIVATE
                return cp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _fadeIn?.Dispose();
            base.Dispose(disposing);
        }
    }
}