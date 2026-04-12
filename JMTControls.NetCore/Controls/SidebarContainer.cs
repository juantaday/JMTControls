using JMTControls.NetCore.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public class SidebarContainer : Panel
    {
        public int ExpandedWidth { get; set; } = 280;
        public int CollapsedWidth { get; set; } = 52;

        private const int TOGGLE_H = 40;
        private const int TAB_H = 42;
        private const int ANIM_MS = 6;
        private const double EASE = 0.28;
        private const double SNAP = 2.0;

        private bool _expanded = true;
        private double _animCurrent;
        private int _animTarget;

        private readonly Panel _toggleBtn;
        private readonly Panel _groupsFlow;
        private readonly Panel _iconsStrip;
        private readonly SidebarTabStrip _tabStrip;
        private readonly System.Windows.Forms.Timer _timer;

        private int _activeTabIndex = 0;
        private FlyoutPanel _activeFlyout;

        [Category("Acción")]
        [Description("Se dispara cuando el usuario hace clic en un ítem del menú.")]
        public event EventHandler<SidebarItemClickEventArgs> ItemClicked;

        [Category("Datos")]
        [Description("Colección de pestañas del Sidebar.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SidebarTabCollection Tabs { get; }


        public SidebarContainer()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            Tabs = new SidebarTabCollection(this);

            BackColor = Color.FromArgb(30, 30, 38);
            Width = ExpandedWidth;
            _animCurrent = ExpandedWidth;

            _toggleBtn = new Panel
            {
                Dock = DockStyle.Top,
                Height = TOGGLE_H,
                BackColor = Color.FromArgb(22, 22, 30),
                Cursor = Cursors.Hand,
            };
            _toggleBtn.Paint += DrawToggleButton;
            _toggleBtn.Click += (s, e) => ToggleSidebar();
            _toggleBtn.MouseEnter += (s, e) => { _toggleBtn.BackColor = Color.FromArgb(35, 35, 45); };
            _toggleBtn.MouseLeave += (s, e) => { _toggleBtn.BackColor = Color.FromArgb(22, 22, 30); };

            _groupsFlow = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 0),
            };

            _iconsStrip = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.Transparent,
            };

            _tabStrip = new SidebarTabStrip
            {
                Height = TAB_H,
                Dock = DockStyle.Bottom,
            };
            _tabStrip.TabClicked += OnTabClicked;

            Controls.Add(_groupsFlow);
            Controls.Add(_iconsStrip);
            Controls.Add(_tabStrip);
            Controls.Add(_toggleBtn);

            _timer = new System.Windows.Forms.Timer { Interval = ANIM_MS };
            _timer.Tick += OnTick;
        }

        // ── MÉTODOS DE INTEGRACIÓN CON EL DISEÑADOR ─────────────────────

        internal void OnDesignerTabAdded(SidebarTab tab)
        {
            _tabStrip.AddTab(tab);
            // Activar el primer tab automáticamente
            if (Tabs.Count == 1)
            {
                _tabStrip.SetActive(0);
                ShowTab(0);
            }
            RefreshDesignerTabs();
        }

        internal void OnDesignerTabsChanged()
        {
            RefreshDesignerTabs();
        }

        private void RefreshDesignerTabs()
        {
            // 1. Limpiamos la LISTA INTERNA del strip y los controles del centro
            _tabStrip.ClearTabs();         // <--- ESTA ES LA CORRECCIÓN PRINCIPAL
            _groupsFlow.Controls.Clear();

            // 2. Volvemos a dibujar basándonos estrictamente en la colección real
            foreach (var tab in Tabs)
            {
                _tabStrip.AddTab(tab);
            }

            // 3. Si hay tabs, mostramos el primero automáticamente
            if (Tabs.Count > 0)
            {
                _tabStrip.SetActive(0);
                ShowTab(0);
            }

            // 4. Forzamos a que Visual Studio repinte el control sin duplicados
            PerformLayout();
            Invalidate(true);
        }

        // ── colapso / expansión ──────────────────────────────────────────

        public void ToggleSidebar()
        {
            if (_expanded) CollapseHorizontal();
            else ExpandHorizontal();
        }

        private void ExpandHorizontal()
        {
            _expanded = true;
            CloseFlyout();

            _iconsStrip.Visible = false;
            _groupsFlow.Visible = false;

            _animTarget = ExpandedWidth;
            _timer.Start();
            _toggleBtn.Invalidate();

            _tabStrip.SetCollapsed(false);
        }

        private void CollapseHorizontal()
        {
            _expanded = false;
            CloseFlyout();

            _groupsFlow.Visible = false;
            BuildIconsStrip();

            _animTarget = CollapsedWidth;
            _timer.Start();
            _toggleBtn.Invalidate();

            _tabStrip.SetCollapsed(true);
        }

        // ── tick ─────────────────────────────────────────────────────────

        private void OnTick(object s, EventArgs e)
        {
            double diff = _animTarget - _animCurrent;

            if (Math.Abs(diff) < SNAP)
            {
                _animCurrent = _animTarget;
                _timer.Stop();
                Width = _animTarget;

                if (_expanded)
                {
                    _groupsFlow.Visible = true;
                    _groupsFlow.Update();
                }
                else
                {
                    _iconsStrip.Visible = true;
                    _iconsStrip.Update();
                }

                Parent?.PerformLayout();
                return;
            }

            _animCurrent += diff * EASE;
            Width = (int)_animCurrent;
            Parent?.PerformLayout();
        }

        // ── toggle paint ─────────────────────────────────────────────────

        private void DrawToggleButton(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(180, 255, 255, 255), 1.5f)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
            };

            int cx = _toggleBtn.Width / 2;
            int cy = _toggleBtn.Height / 2;

            if (_expanded)
            {
                g.DrawLine(pen, cx - 10, cy - 6, cx + 10, cy - 6);
                g.DrawLine(pen, cx - 10, cy, cx + 10, cy);
                g.DrawLine(pen, cx - 10, cy + 6, cx + 10, cy + 6);
            }
            else
            {
                g.DrawLines(pen, new[]
                {
                    new PointF(cx - 3, cy - 7),
                    new PointF(cx + 4, cy),
                    new PointF(cx - 3, cy + 7),
                });
            }
        }

        // ── tabs ─────────────────────────────────────────────────────────

        public void ShowTab(int index)
        {
            if (index < 0 || index >= Tabs.Count) return;

            _activeTabIndex = index;

            // 1. Esto reconstruye los acordeones (necesario siempre)
            BuildActiveTabContent(Tabs[index]);

            // la reconstrucción visual de los íconos basándonos en el nuevo tab.
            if (!_expanded)
            {
                BuildIconsStrip();
            }
        }

        private void OnTabClicked(int index) => ShowTab(index);

        private void BuildActiveTabContent(SidebarTab tabModel)
        {
            _groupsFlow.SuspendLayout();
            _groupsFlow.Controls.Clear();

            if (tabModel?.Groups == null)
            {
                _groupsFlow.ResumeLayout(true);
                return;
            }

            for (int g = tabModel.Groups.Count - 1; g >= 0; g--)
            {
                var groupModel = tabModel.Groups[g];
                var accordion = new SmoothAccordionGroup
                {
                    Title = groupModel.Title,
                    Dock = DockStyle.Top
                };

                for (int i = groupModel.Items.Count - 1; i >= 0; i--)
                {
                    // LE PASAMOS EL ITEM, EL GRUPO Y EL TAB
                    AddMenuButton(accordion, groupModel.Items[i], groupModel, tabModel);
                }

                _groupsFlow.Controls.Add(accordion);
            }

            _groupsFlow.ResumeLayout(true);
        }

        void AddMenuButton(SmoothAccordionGroup group, SidebarItemModel model, SidebarGroupModel groupModel, SidebarTab tabModel)
        {
            var btn = new SidebarMenuButton
            {
                Dock = DockStyle.Top,
                Text = model.Text,
                Shortcut = model.Shortcut,
                Icon = model.Icon,
                Font = model.Font,
                NormalBackColor = model.NormalBackColor,
                NormalForeColor = model.NormalForeColor,
                NormalShortcutColor = model.NormalShortcutColor,
                HoverBackColor = model.HoverBackColor,
                HoverForeColor = model.HoverForeColor,
                HoverShortcutColor = model.HoverShortcutColor
            };

            // 🔴 AQUÍ ES LA MAGIA: Enlazamos el clic del botón visual con nuestro nuevo evento
            btn.Click += (s, e) =>
            {
                ItemClicked?.Invoke(this, new SidebarItemClickEventArgs(model, groupModel, tabModel));
            };

            group.AddContent(btn);
        }
        // ── strip de íconos ──────────────────────────────────────────────

        private void BuildIconsStrip()
        {
            _iconsStrip.Controls.Clear();

            var accordions = _groupsFlow.Controls.OfType<SmoothAccordionGroup>()
                                                 .OrderBy(c => c.Top)
                                                 .ToList();
            int y = 0;
            foreach (var groupControl in accordions)
            {
                var iconBtn = new SidebarIconButton(groupControl)
                {
                    Top = y,
                    Left = 0,
                    Width = CollapsedWidth,
                    Height = CollapsedWidth,
                };
                iconBtn.Click += (s, e) => ShowFlyout(((SidebarIconButton)s).Group);
                _iconsStrip.Controls.Add(iconBtn);
                y += CollapsedWidth;
            }
        }

        // ── flyout ───────────────────────────────────────────────────────

        private void ShowFlyout(SmoothAccordionGroup group)
        {
            CloseFlyout();
            var pos = this.PointToScreen(new Point(CollapsedWidth + 2, TOGGLE_H));
            _activeFlyout = new FlyoutPanel(group, pos);
            _activeFlyout.FormClosed += (s, e) => _activeFlyout = null;
            _activeFlyout.Show(this.FindForm());
        }

        private void CloseFlyout()
        {
            _activeFlyout?.Close();
            _activeFlyout = null;
        }

        // ── resize y dispose ─────────────────────────────────────────────

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _timer?.Dispose(); CloseFlyout(); }
            base.Dispose(disposing);
        }
    }


    // ═══════════════════════════════════════════════════════════════════
    //  SidebarTab
    // ═══════════════════════════════════════════════════════════════════
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SidebarTab
    {
        // Contador estático para auto-nombrado
        private static int _nameCounter = 0;

        [Category("Diseño")]
        [Description("Nombre único del componente (usado en código).")]
        public string Name { get; set; }

        [Category("Apariencia")]
        public string Title { get; set; } = "Nueva Pestaña";

        [Category("Apariencia")]
        public Image Icon { get; set; }

        [Category("Apariencia")]
        public Color BackColor { get; set; } = Color.Transparent;

        [Category("Datos")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<SidebarGroupModel> Groups { get; } = new Collection<SidebarGroupModel>();

        public SidebarTab()
        {
            _nameCounter++;
            Name = $"sidebarTab{_nameCounter}";
        }

        /// <summary>Reinicia el contador (útil al limpiar el diseñador).</summary>
        public static void ResetNameCounter() => _nameCounter = 0;

        public override string ToString() => string.IsNullOrWhiteSpace(Title) ? Name : Title;
    }

    // ═══════════════════════════════════════════════════════════════════
    //  SidebarTabStrip  — panel que pinta todos los tabs directamente.
    //  Sin controles hijos, sin TableLayout, sin z-order.
    //  OnPaint dibuja cada tab como una franja; OnMouseDown detecta click.
    // ═══════════════════════════════════════════════════════════════════
    public class SidebarTabStrip : Panel
    {
        public event Action<int> TabClicked;

        private readonly List<SidebarTab> _tabs = new();
        private int _active = -1;
        private bool _collapsed = false;
        private int _hovered = -1;
        private TabListFlyout _flyout;

        public SidebarTabStrip()
        {
            BackColor = Color.FromArgb(20, 20, 28);
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
        }

        public void ClearTabs()
        {
            _tabs.Clear();
            _active = -1;
            Invalidate();
        }


        public void AddTab(SidebarTab tab)
        {
            _tabs.Add(tab);
            if (_tabs.Count == 1) _active = 0;
            Invalidate();
        }

        public void SetActive(int index)
        {
            _active = index;
            Invalidate();
        }

        public void SetCollapsed(bool collapsed)
        {
            _collapsed = collapsed;
            Invalidate();
        }

        // ── geometría ────────────────────────────────────────────────────

        /// <summary>Devuelve el rectángulo de pantalla del tab [i] según modo.</summary>
        private Rectangle TabRect(int i)
        {
            if (_collapsed)
            {
                // Modo colapsado: solo el tab activo, ocupa todo el ancho
                return (i == _active) ? new Rectangle(0, 0, Width, Height) : Rectangle.Empty;
            }
            if (_tabs.Count == 0 || Width <= 0) return Rectangle.Empty;
            int w = Width / _tabs.Count;
            int extra = Width - w * _tabs.Count;
            int x = 0;
            for (int j = 0; j < i; j++)
                x += w + (j < extra ? 1 : 0);
            return new Rectangle(x, 0, w + (i < extra ? 1 : 0), Height);
        }

        private int TabAtPoint(Point p)
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                var r = TabRect(i);
                if (!r.IsEmpty && r.Contains(p)) return i;
            }
            return -1;
        }

        // ── mouse ────────────────────────────────────────────────────────

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int h = TabAtPoint(e.Location);
            if (h != _hovered) { _hovered = h; Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hovered != -1) { _hovered = -1; Invalidate(); }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;

            if (_collapsed)
            {
                OpenFlyout();
                return;
            }

            int idx = TabAtPoint(e.Location);
            if (idx < 0) return;
            _active = idx;
            Invalidate();
            TabClicked?.Invoke(idx);
        }

        // ── flyout (modo colapsado) ──────────────────────────────────────

        private void OpenFlyout()
        {
            if (_flyout != null && !_flyout.IsDisposed)
            {
                _flyout.Close(); _flyout = null; return;
            }
            var screenPos = PointToScreen(new Point(0, 0));
            _flyout = new TabListFlyout(_tabs, _active, screenPos);
            _flyout.TabSelected += (idx) =>
            {
                _active = idx;
                Invalidate();
                TabClicked?.Invoke(idx);
                _flyout = null;
            };
            _flyout.FormClosed += (s, e) => _flyout = null;
            _flyout.Show(this.FindForm());
        }

        // ── pintura ──────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Línea separadora superior
            using (var pen = new Pen(Color.FromArgb(60, 60, 75), 1f))
                g.DrawLine(pen, 0, 0, Width, 0);

            for (int i = 0; i < _tabs.Count; i++)
            {
                if (_collapsed && i != _active) continue;

                var r = TabRect(i);
                bool active = (i == _active);
                bool hov = (i == _hovered) && !active;

                DrawTab(g, r, _tabs[i], active, hov, _collapsed);

                // Separador vertical entre tabs
                if (!_collapsed && i < _tabs.Count - 1)
                {
                    using var sep = new Pen(Color.FromArgb(45, 45, 58), 1f);
                    g.DrawLine(sep, r.Right, 4, r.Right, Height - 4);
                }
            }
        }

        private static void DrawTab(Graphics g, Rectangle r, SidebarTab tab,
                                    bool active, bool hovered, bool collapsed)
        {
            // Fondo
            if (active)
            {
                using var bg = new SolidBrush(Color.FromArgb(50, 50, 65));
                g.FillRectangle(bg, r);
                // Acento superior
                using var acc = new SolidBrush(Color.FromArgb(100, 140, 255));
                g.FillRectangle(acc, r.X, r.Y, r.Width, 2);
            }
            else if (hovered)
            {
                using var bg = new SolidBrush(Color.FromArgb(38, 38, 50));
                g.FillRectangle(bg, r);
            }

            if (collapsed)
            {
                // Letra inicial centrada
                string letter = tab.Title?.Length > 0 ? tab.Title[0].ToString().ToUpper() : "?";
                using var font = new Font("Segoe UI", 13f, FontStyle.Bold);
                using var brush = new SolidBrush(active ? Color.White : Color.FromArgb(130, 130, 145));
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                };
                g.DrawString(letter, font, brush,
                    new RectangleF(r.X, r.Y - 4, r.Width, r.Height), sf);

                // Triángulo ▲ indica que hay más tabs
                using var arrow = new SolidBrush(Color.FromArgb(100, 140, 255));
                int ax = r.X + r.Width / 2;
                int ay = r.Bottom - 6;
                g.FillPolygon(arrow, new[]
                {
                    new PointF(ax - 5, ay + 2),
                    new PointF(ax + 5, ay + 2),
                    new PointF(ax,     ay - 3),
                });
            }
            else
            {
                int contentY = r.Y + 6;

                if (tab.Icon != null)
                {
                    g.DrawImage(tab.Icon, r.X + (r.Width - 16) / 2, contentY, 16, 16);
                    contentY += 18;
                }

                using var font = new Font("Segoe UI", 7.5f);
                using var brush = new SolidBrush(active ? Color.White : Color.FromArgb(130, 130, 145));
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter,
                };
                g.DrawString(tab.Title, font, brush,
                    new RectangleF(r.X + 2, contentY, r.Width - 4, r.Bottom - contentY - 4), sf);
            }
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  SidebarIconButton
    // ═══════════════════════════════════════════════════════════════════
    internal class SidebarIconButton : Panel
    {
        public SmoothAccordionGroup Group { get; }
        private bool _hovered;

        public SidebarIconButton(SmoothAccordionGroup group)
        {
            Group = group;
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            ResizeRedraw = true;
        }

        protected override void OnVisibleChanged(EventArgs e) { base.OnVisibleChanged(e); ForceRepaint(); }
        protected override void OnSizeChanged(EventArgs e) { base.OnSizeChanged(e); ForceRepaint(); }
        protected override void OnHandleCreated(EventArgs e) { base.OnHandleCreated(e); ForceRepaint(); }
        protected override void OnMouseEnter(EventArgs e) { _hovered = true; ForceRepaint(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hovered = false; ForceRepaint(); base.OnMouseLeave(e); }
        protected override void OnClick(EventArgs e) { ForceRepaint(); base.OnClick(e); }

        private void ForceRepaint()
        {
            if (!IsHandleCreated || !Visible || Width <= 0 || Height <= 0) return;
            Invalidate(true);
            Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (_hovered)
            {
                using var bg = new SolidBrush(Color.FromArgb(55, 55, 68));
                g.FillRectangle(bg, ClientRectangle);
            }

            if (Group.GroupIcon != null)
            {
                int ix = (Width - 20) / 2;
                int iy = (Height - 20) / 2 - 4;
                g.DrawImage(Group.GroupIcon, ix, iy, 20, 20);
            }
            else
            {
                string letter = Group.Title?.Length > 0
                    ? Group.Title[0].ToString().ToUpper() : "?";
                using var font = new Font("Segoe UI", 13f, FontStyle.Bold);
                using var brush = new SolidBrush(Color.FromArgb(180, 200, 220));
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                };
                g.DrawString(letter, font, brush,
                    new RectangleF(0, -4, Width, Height), sf);
            }

            string abbr = Group.Title?.Length >= 3
                ? Group.Title.Substring(0, 3) : (Group.Title ?? "");
            using var sf2 = new StringFormat { Alignment = StringAlignment.Center };
            using var sfont = new Font("Segoe UI", 7f);
            using var sb = new SolidBrush(Color.FromArgb(110, 115, 130));
            g.DrawString(abbr, sfont, sb,
                new RectangleF(0, Height - 14, Width, 12), sf2);

            using var pen = new Pen(Color.FromArgb(45, 45, 58), 0.5f);
            g.DrawLine(pen, 8, Height - 1, Width - 8, Height - 1);
        }
    }


    // ═══════════════════════════════════════════════════════════════════
    //  TabListFlyout — panel flotante con lista de tabs (modo colapsado)
    //  FIX: Se posiciona ENCIMA del tab strip (no debajo)
    // ═══════════════════════════════════════════════════════════════════
    internal class TabListFlyout : Form
    {
        public event Action<int> TabSelected;

        private readonly List<SidebarTab> _tabs;
        private readonly int _activeIndex;

        private const int ITEM_H = 40;
        private const int FLYOUT_W = 200;

        public TabListFlyout(List<SidebarTab> tabs, int activeIndex, Point screenPos)
        {
            _tabs = tabs;
            _activeIndex = activeIndex;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            BackColor = Color.FromArgb(30, 30, 40);
            Size = new Size(FLYOUT_W, tabs.Count * ITEM_H + 2);

            // FIX: posicionar ENCIMA del strip
            Location = new Point(screenPos.X, screenPos.Y - Height);

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            BuildItems();

            Deactivate += (s, e) => Close();
        }

        private void BuildItems()
        {
            int y = 1;
            for (int i = 0; i < _tabs.Count; i++)
            {
                int idx = i;
                var tab = _tabs[i];
                bool isActive = (i == _activeIndex);

                var item = new TabFlyoutItem(tab, isActive)
                {
                    Top = y,
                    Left = 0,
                    Width = FLYOUT_W,
                    Height = ITEM_H,
                };
                item.Click += (s, e) =>
                {
                    TabSelected?.Invoke(idx);
                    Close();
                };
                Controls.Add(item);
                y += ITEM_H;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pen = new Pen(Color.FromArgb(60, 60, 80), 1f);
            e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) { Close(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_NOACTIVATE — no roba foco
                return cp;
            }
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  TabFlyoutItem — fila individual dentro del TabListFlyout
    // ═══════════════════════════════════════════════════════════════════
    internal class TabFlyoutItem : Panel
    {
        private readonly SidebarTab _tab;
        private readonly bool _active;
        private bool _hovered;

        public TabFlyoutItem(SidebarTab tab, bool active)
        {
            _tab = tab;
            _active = active;
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (_active)
            {
                using var bg = new SolidBrush(Color.FromArgb(50, 50, 68));
                g.FillRectangle(bg, ClientRectangle);
                using var acc = new SolidBrush(Color.FromArgb(100, 140, 255));
                g.FillRectangle(acc, 0, 0, 3, Height);
            }
            else if (_hovered)
            {
                using var bg = new SolidBrush(Color.FromArgb(42, 42, 56));
                g.FillRectangle(bg, ClientRectangle);
            }

            int textX = 14;

            if (_tab.Icon != null)
            {
                g.DrawImage(_tab.Icon, 12, (Height - 16) / 2, 16, 16);
                textX = 36;
            }

            using var font = new Font("Segoe UI", 9f, _active ? FontStyle.Bold : FontStyle.Regular);
            using var brush = new SolidBrush(_active
                ? Color.White
                : Color.FromArgb(170, 170, 185));
            using var sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
            };
            g.DrawString(_tab.Title, font, brush,
                new RectangleF(textX, 0, Width - textX - 8, Height), sf);

            using var pen = new Pen(Color.FromArgb(45, 45, 58), 0.5f);
            g.DrawLine(pen, 8, Height - 1, Width - 8, Height - 1);
        }

        protected override void OnMouseEnter(EventArgs e) { _hovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hovered = false; Invalidate(); base.OnMouseLeave(e); }
    }


    // ═══════════════════════════════════════════════════════════════════
    //  SidebarMenuButton
    // ═══════════════════════════════════════════════════════════════════
    public class SidebarMenuButton : Control
    {
        public string Shortcut { get; set; } = "";
        public Image Icon { get; set; }

        public Color NormalBackColor { get; set; } = Color.Transparent;
        public Color NormalForeColor { get; set; } = Color.FromArgb(195, 195, 210);
        public Color NormalShortcutColor { get; set; } = Color.FromArgb(90, 95, 115);

        public Color HoverBackColor { get; set; } = Color.FromArgb(50, 50, 65);
        public Color HoverForeColor { get; set; } = Color.White;
        public Color HoverShortcutColor { get; set; } = Color.FromArgb(130, 135, 155);

        private bool _isHovered;
        private bool _isPressed;

        public SidebarMenuButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.Selectable, true);

            Height = 34;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9f);
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _isHovered = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _isHovered = false; _isPressed = false; Invalidate(); }
        protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); _isPressed = true; Invalidate(); }
        protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); _isPressed = false; Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Color currentBackColor = _isPressed ? Color.FromArgb(40, 40, 58) : (_isHovered ? HoverBackColor : NormalBackColor);
            using (var bgBrush = new SolidBrush(currentBackColor))
                g.FillRectangle(bgBrush, ClientRectangle);

            int currentX = 20;

            if (Icon != null)
            {
                int iconSize = 16;
                int iconY = (Height - iconSize) / 2;
                g.DrawImage(Icon, currentX, iconY, iconSize, iconSize);
                currentX += iconSize + 12;
            }

            Color currentForeColor = _isHovered ? HoverForeColor : NormalForeColor;
            using (var textBrush = new SolidBrush(currentForeColor))
            using (var sf = new StringFormat { LineAlignment = StringAlignment.Center })
            {
                var textRect = new Rectangle(currentX, 0, Width - currentX - 60, Height);
                g.DrawString(Text, Font, textBrush, textRect, sf);
            }

            if (!string.IsNullOrEmpty(Shortcut))
            {
                Color currentShortcutColor = _isHovered ? HoverShortcutColor : NormalShortcutColor;
                using (var shortcutBrush = new SolidBrush(currentShortcutColor))
                using (var sf = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center })
                using (var shortcutFont = new Font(Font.FontFamily, Font.Size - 1.5f))
                {
                    var shortcutRect = new Rectangle(0, 0, Width - 15, Height);
                    g.DrawString(Shortcut, shortcutFont, shortcutBrush, shortcutRect, sf);
                }
            }
        }
    }
}