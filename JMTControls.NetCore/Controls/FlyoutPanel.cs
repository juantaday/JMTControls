using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    // ═══════════════════════════════════════════════════════════════════
    //  FlyoutPanel  — fix: extrae texto del Button hijo del Panel-row
    // ═══════════════════════════════════════════════════════════════════
    public class FlyoutPanel : Form
    {
        private const int FLYOUT_W = 220;
        private const int HEADER_H = 38;
        private const int ITEM_H = 36;
        private const int PADDING = 6;

        private double _opacity = 0;
        private readonly System.Windows.Forms.Timer _fadeTimer;

        public FlyoutPanel(SmoothAccordionGroup group, Point screenPosition)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            BackColor = Color.FromArgb(38, 38, 46);
            Location = screenPosition;
            Width = FLYOUT_W;
            Opacity = 0;

            BuildContent(group);
            AdjustHeight(group);

            _fadeTimer = new System.Windows.Forms.Timer { Interval = 10 };
            _fadeTimer.Tick += (s, e) =>
            {
                _opacity += 0.15;
                if (_opacity >= 1) { _opacity = 1; _fadeTimer.Stop(); }
                Opacity = _opacity;
            };
        }

        // Constructor para lista de tabs (modo sidebar colapsado)
        public FlyoutPanel(List<SidebarTab> tabs, int activeIndex,
                           Point screenPosition, Action<int> onTabSelected)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            BackColor = Color.FromArgb(38, 38, 46);
            Location = screenPosition;
            Width = FLYOUT_W;
            Opacity = 0;

            BuildTabList(tabs, activeIndex, onTabSelected);

            _fadeTimer = new System.Windows.Forms.Timer { Interval = 10 };
            _fadeTimer.Tick += (s, e) =>
            {
                _opacity += 0.15;
                if (_opacity >= 1) { _opacity = 1; _fadeTimer.Stop(); }
                Opacity = _opacity;
            };
        }

        private void BuildTabList(List<SidebarTab> tabs, int activeIndex, Action<int> onTabSelected)
        {
            // Header
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = HEADER_H,
                BackColor = Color.FromArgb(52, 52, 62),
            };
            var lbl = new Label
            {
                Text = "Secciones",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Padding = new Padding(12, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
            };
            header.Controls.Add(lbl);
            Controls.Add(header);

            Controls.Add(new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(60, 60, 72),
            });

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(PADDING, PADDING, PADDING, PADDING),
                BackColor = Color.Transparent,
                AutoScroll = false,
            };

            for (int i = 0; i < tabs.Count; i++)
            {
                int idx = i;
                bool isAct = (i == activeIndex);
                var tab = tabs[i];

                var btn = new Button
                {
                    Text = tab.Title,
                    Width = FLYOUT_W - PADDING * 2 - 2,
                    Height = ITEM_H,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = isAct ? Color.White : Color.FromArgb(200, 200, 215),
                    BackColor = isAct ? Color.FromArgb(50, 50, 70) : Color.Transparent,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 0, 0),
                    Cursor = Cursors.Hand,
                    Font = new Font("Segoe UI", 9f, isAct ? FontStyle.Bold : FontStyle.Regular),
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 68);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 45, 58);

                if (tab.Icon != null)
                {
                    btn.Image = tab.Icon;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(28, 0, 0, 0);
                }

                btn.Click += (s, e) => { Close(); onTabSelected?.Invoke(idx); };
                flow.Controls.Add(btn);
            }

            Controls.Add(flow);

            Height = HEADER_H + 1 + PADDING * 2 + tabs.Count * ITEM_H + PADDING;
        }

        private void BuildContent(SmoothAccordionGroup group)
        {
            SetStyle(ControlStyles.UserPaint, true);

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = HEADER_H,
                BackColor = Color.FromArgb(52, 52, 62),
            };
            var lblTitle = new Label
            {
                Text = group.Title,
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Padding = new Padding(12, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
            };
            header.Controls.Add(lblTitle);

            if (group.GroupIcon != null)
            {
                var icon = new PictureBox
                {
                    Image = group.GroupIcon,
                    Size = new Size(16, 16),
                    Location = new Point(10, (HEADER_H - 16) / 2),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                header.Controls.Add(icon);
                lblTitle.Padding = new Padding(34, 0, 0, 0);
            }

            Controls.Add(header);
            Controls.Add(new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(60, 60, 72),
            });

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(PADDING, PADDING, PADDING, PADDING),
                BackColor = Color.Transparent,
                AutoScroll = false,
            };

            foreach (Control original in group.ContentControls)
            {
                var item = BuildFlyoutItem(original);
                if (item != null) flow.Controls.Add(item);
            }

            Controls.Add(flow);
        }

        private Control BuildFlyoutItem(Control original)
        {
            // Los items son Panel-row que contienen un Button hijo.
            // Buscamos el Button para obtener el texto real.
            Button sourceBtn = FindFirstButton(original);
            string text = sourceBtn?.Text ?? original.Text ?? "";
            if (string.IsNullOrWhiteSpace(text)) return null;

            var btn = new Button
            {
                Text = text,
                Width = FLYOUT_W - PADDING * 2 - 2,
                Height = ITEM_H,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(200, 200, 215),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f),
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 68);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 45, 58);

            var captured = (Control)(sourceBtn ?? (Control)original);
            btn.Click += (s, e) =>
            {
                Close();
                if (captured is Button b) b.PerformClick();
                else captured.Focus();
            };

            return btn;
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

        private void AdjustHeight(SmoothAccordionGroup group)
        {
            int count = 0;
            foreach (Control _ in group.ContentControls) count++;
            Height = HEADER_H + 1 + PADDING * 2 + count * ITEM_H + PADDING;
        }

    
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pen = new Pen(Color.FromArgb(70, 70, 85), 1f);
            e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _fadeTimer.Start();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            FadeOut();
        }

        private void FadeOut()
        {
            _fadeTimer.Stop();
            var ft = new System.Windows.Forms.Timer { Interval = 10 };
            ft.Tick += (s, e) =>
            {
                Opacity -= 0.12;
                if (Opacity <= 0) { ft.Stop(); ft.Dispose(); Close(); }
            };
            ft.Start();
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
            if (disposing) _fadeTimer?.Dispose();
            base.Dispose(disposing);
        }
    }
}