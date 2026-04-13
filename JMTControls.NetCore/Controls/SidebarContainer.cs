using JMTControls.NetCore.Designers;
using JMTControls.NetCore.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    // ═══════════════════════════════════════════════════════════════════
    //  SidebarContainer — acordeón estilo DevExpress con soporte completo
    //  de diseñador: selección de ítems + generación de eventos Click.
    // ═══════════════════════════════════════════════════════════════════
    [Designer(typeof(SidebarDesigner))]
    public class SidebarContainer : Panel
    {
        // ── Propiedades ──────────────────────────────────────────────────
        [Category("Apariencia")]
        public int ExpandedWidth { get; set; } = 280;
        [Category("Apariencia")]
        public int CollapsedWidth { get; set; } = 52;

        // ── Constantes ───────────────────────────────────────────────────
        private const int TOGGLE_H = 38;
        private const int TAB_H = 42;
        private const int ANIM_MS = 6;
        private const double EASE = 0.28;
        private const double SNAP = 2.0;

        // ── Estado ───────────────────────────────────────────────────────
        private bool _expanded = true;
        private double _animCurrent;
        private int _animTarget;
        private int _activeTabIndex;
        private int _pendingSelectedTabIndex = -1;
        private string _pendingSelectedTabName;
        private SidebarItemModel _selectedItem;
        private FlyoutPanel _activeFlyout;
        private readonly HashSet<SidebarGroupModel> _trackedGroups = new();
        private readonly HashSet<SidebarItemModel> _trackedItems = new();

        // ── Controles internos ───────────────────────────────────────────
        private readonly Panel _toggleBtn;
        private readonly DoubleBufferedPanel _groupsFlow;
        private readonly DoubleBufferedPanel _iconsStrip;
        private readonly SidebarTabStrip _tabStrip;
        private readonly System.Windows.Forms.Timer _timer;

        // ── Eventos ──────────────────────────────────────────────────────
        [Category("Acción")]
        [Description("Se dispara cuando el usuario hace clic en cualquier ítem del menú.")]
        public event EventHandler<SidebarItemClickEventArgs> ItemClicked;

        // ── Colección de tabs ─────────────────────────────────────────────
        [Category("Datos")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SidebarTabCollection Tabs { get; }

        [Category("Comportamiento")]
        [DefaultValue(0)]
        public int SelectedTabIndex
        {
            get => _activeTabIndex;
            set
            {
                if (value < 0) return;

                if (Tabs.Count == 0)
                {
                    _pendingSelectedTabIndex = value;
                    return;
                }

                ApplySelectedTabIndex(value);
            }
        }

        [Category("Comportamiento")]
        [DefaultValue(null)]
        public string SelectedTabName
        {
            get => (_activeTabIndex >= 0 && _activeTabIndex < Tabs.Count) ? Tabs[_activeTabIndex].Name : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                if (Tabs.Count == 0)
                {
                    _pendingSelectedTabName = value;
                    return;
                }

                int idx = FindTabIndexByName(value);
                if (idx >= 0)
                {
                    ApplySelectedTabIndex(idx);
                    _pendingSelectedTabName = null;
                }
                else
                {
                    _pendingSelectedTabName = value;
                }
            }
        }

        [Browsable(false)]
        public SidebarItemModel SelectedItem => _selectedItem;

        [Browsable(false)]
        public int ActiveTabIndex => _activeTabIndex;

        [Browsable(false)]
        internal Control DesignerTabStripHost => _tabStrip;

        [Browsable(false)]
        internal Control DesignerGroupsHost => _groupsFlow;

        private bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime || Site?.DesignMode == true;

        // ════════════════════════════════════════════════════════════════
        //  Constructor
        // ════════════════════════════════════════════════════════════════
        public SidebarContainer()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            Tabs = new SidebarTabCollection(this);
            BackColor = Color.FromArgb(30, 30, 38);
            Width = ExpandedWidth;
            _animCurrent = ExpandedWidth;

            // ── Toggle ───────────────────────────────────────────────────
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

            // ── Panel de grupos ───────────────────────────────────────────
            _groupsFlow = new DoubleBufferedPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 0),
            };

            // ── Strip de íconos (colapsado) ───────────────────────────────
            _iconsStrip = new DoubleBufferedPanel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.Transparent,
            };

            // ── Tab strip ─────────────────────────────────────────────────
            _tabStrip = new SidebarTabStrip { Height = TAB_H, Dock = DockStyle.Bottom };
            _tabStrip.TabClicked += OnTabClicked;

            Controls.Add(_groupsFlow);
            Controls.Add(_iconsStrip);
            Controls.Add(_tabStrip);
            Controls.Add(_toggleBtn);

            _timer = new System.Windows.Forms.Timer { Interval = ANIM_MS };
            _timer.Tick += OnAnimTick;
        }

        // ════════════════════════════════════════════════════════════════
        //  Integración con el Diseñador
        // ════════════════════════════════════════════════════════════════

        internal void OnDesignerTabAdded(SidebarTab tab)
        {
            _tabStrip.AddTab(tab);
            if (Tabs.Count == 1) { _tabStrip.SetActive(0); ShowTab(0); }
            RefreshDesignerTabs();
        }

        internal void OnDesignerTabsChanged() => RefreshDesignerTabs();

        private void RefreshDesignerTabs()
        {
            _tabStrip.ClearTabs();
            _groupsFlow.Controls.Clear();

            TrackModelChanges();

            foreach (var tab in Tabs)
                _tabStrip.AddTab(tab);

            if (Tabs.Count > 0)
            {
                int index = ResolveDesiredTabIndex();
                ApplySelectedTabIndex(index);
            }

            PerformLayout();
            Invalidate(true);
        }

        private void TrackModelChanges()
        {
            foreach (var tab in Tabs)
            {
                foreach (var group in tab.Groups)
                {
                    if (_trackedGroups.Add(group))
                        group.Changed += OnModelChanged;

                    foreach (var item in group.Items)
                    {
                        if (_trackedItems.Add(item))
                            item.Changed += OnModelChanged;
                    }
                }
            }
        }

        private void OnModelChanged(object sender, EventArgs e)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => OnModelChanged(sender, e)));
                return;
            }

            if (Tabs.Count == 0) return;

            int index = (_activeTabIndex >= 0 && _activeTabIndex < Tabs.Count) ? _activeTabIndex : 0;
            BuildActiveTabContent(Tabs[index]);

            if (!_expanded)
                BuildIconsStrip();

            _tabStrip.Invalidate();
            Invalidate(true);
        }

        /// <summary>
        /// Llamado por SidebarDesigner al inicializarse.
        /// Registra todos los SidebarItemModel y SidebarGroupModel existentes
        /// en el IContainer del formulario para que VS los gestione
        /// (serialización, generación de eventos, ventana de Propiedades).
        /// </summary>
        public void RegisterModelsWithDesigner(IDesignerHost host)
        {
            RegisterModelsWithContainer(host?.Container);
        }

        public void RegisterModelsWithContainer(IContainer container)
        {
            if (container == null) return;

            foreach (var tab in Tabs)
            {
                foreach (var group in tab.Groups)
                {
                    EnsureRegistered(container, group);
                    foreach (var item in group.Items)
                        EnsureRegistered(container, item);
                }
            }
        }

        private static void EnsureRegistered(IContainer container, IComponent component)
        {
            // Solo registrar si aún no tiene sitio (no está en el IContainer)
            if (component.Site == null)
            {
                string name = (component is SidebarItemModel m) ? m.Name
                            : (component is SidebarGroupModel g) ? g.Name
                            : null;

                try
                {
                    container.Add(component, name);
                }
                catch
                {
                    try { container.Add(component); } catch { }
                }
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  HitTest para el Diseñador
        //  Devuelve SidebarItemModel o SidebarGroupModel según dónde se hizo clic.
        // ════════════════════════════════════════════════════════════════

        public object HitTestItem(Point pt)
        {
            if (!_groupsFlow.Visible)
                return null;

            var groupsHostRect = RectangleToClient(_groupsFlow.RectangleToScreen(_groupsFlow.ClientRectangle));
            if (!groupsHostRect.Contains(pt))
                return null;

            foreach (var groupCtrl in _groupsFlow.Controls.OfType<SmoothAccordionGroup>().OrderBy(c => c.Top))
            {
                var groupRect = RectangleToClient(groupCtrl.RectangleToScreen(groupCtrl.ClientRectangle));
                if (!groupRect.Contains(pt))
                    continue;

                int localY = pt.Y - groupRect.Top;

                // Clic en cabecera → selecciona el SidebarGroupModel
                if (localY >= 0 && localY < groupCtrl.HeaderHeight)
                    return groupCtrl.Tag as SidebarGroupModel;

                // Clic en ítem → selecciona el SidebarItemModel via Tag
                foreach (Control itemCtrl in groupCtrl.ContentControls.OrderBy(c => c.Top))
                {
                    var itemRect = RectangleToClient(itemCtrl.RectangleToScreen(itemCtrl.ClientRectangle));
                    if (itemRect.Contains(pt) && itemCtrl.Tag is SidebarItemModel model)
                        return model;
                }

                return null;
            }

            return null;
        }

        public bool HitTestDesignerSurface(Point pt)
        {
            if (HitTestItem(pt) != null)
                return true;

            return TryGetTabAtPoint(pt, out _);
        }

        public bool ActivateTabFromDesigner(Point pt)
        {
            if (!TryGetTabAtPoint(pt, out int index))
                return false;

            ApplySelectedTabIndex(index);
            return true;
        }

        public void ToggleGroupFromDesignerIfChevron(Point pt)
        {
            if (!_groupsFlow.Visible) return;

            foreach (var groupCtrl in _groupsFlow.Controls.OfType<SmoothAccordionGroup>().OrderBy(c => c.Top))
            {
                var groupRect = RectangleToClient(groupCtrl.RectangleToScreen(groupCtrl.ClientRectangle));
                if (!groupRect.Contains(pt))
                    continue;

                int localY = pt.Y - groupRect.Top;
                if (localY < 0 || localY >= groupCtrl.HeaderHeight)
                    return;

                groupCtrl.Toggle();
                if (groupCtrl.Tag is SidebarGroupModel gm)
                    gm.Collapsed = !groupCtrl.IsExpanded;

                return;
            }
        }

        private bool TryGetTabAtPoint(Point pt, out int index)
        {
            index = -1;
            if (Tabs.Count == 0 || !_tabStrip.Visible || _tabStrip.Width <= 0 || _tabStrip.Height <= 0)
                return false;

            var tabRect = RectangleToClient(_tabStrip.RectangleToScreen(_tabStrip.ClientRectangle));
            if (!tabRect.Contains(pt))
                return false;

            var pTab = new Point(pt.X - tabRect.X, pt.Y - tabRect.Y);

            int w = _tabStrip.Width / Tabs.Count;
            int extra = _tabStrip.Width - w * Tabs.Count;
            int x = 0;

            for (int i = 0; i < Tabs.Count; i++)
            {
                int cw = w + (i < extra ? 1 : 0);
                if (new Rectangle(x, 0, cw, _tabStrip.Height).Contains(pTab))
                {
                    index = i;
                    return true;
                }
                x += cw;
            }

            return false;
        }

        // ════════════════════════════════════════════════════════════════
        //  Colapso / Expansión
        // ════════════════════════════════════════════════════════════════

        public void ToggleSidebar()
        {
            if (_expanded) CollapseHorizontal(); else ExpandHorizontal();
        }

        private void ExpandHorizontal()
        {
            _expanded = true;
            CloseFlyout();
            _iconsStrip.Visible = false;
            if (IsInDesignMode)
            {
                _groupsFlow.Visible = true;
                Width = ExpandedWidth;
                _animCurrent = ExpandedWidth;
            }
            else
            {
                _groupsFlow.Visible = false;
                _animTarget = ExpandedWidth;
                _timer.Start();
            }
            _toggleBtn.Invalidate();
            _tabStrip.SetCollapsed(false);
        }

        private void CollapseHorizontal()
        {
            _expanded = false;
            CloseFlyout();
            BuildIconsStrip();
            if (IsInDesignMode)
            {
                _groupsFlow.Visible = false;
                _iconsStrip.Visible = true;
                Width = CollapsedWidth;
                _animCurrent = CollapsedWidth;
            }
            else
            {
                _groupsFlow.Visible = false;
                _animTarget = CollapsedWidth;
                _timer.Start();
            }
            _toggleBtn.Invalidate();
            _tabStrip.SetCollapsed(true);
        }

        private void OnAnimTick(object s, EventArgs e)
        {
            double diff = _animTarget - _animCurrent;
            if (Math.Abs(diff) < SNAP)
            {
                _animCurrent = _animTarget;
                _timer.Stop();
                Width = _animTarget;
                if (_expanded) { _groupsFlow.Visible = true; _groupsFlow.Update(); }
                else { _iconsStrip.Visible = true; _iconsStrip.Update(); }
                Parent?.PerformLayout();
                return;
            }
            _animCurrent += diff * EASE;
            Width = (int)_animCurrent;
            Parent?.PerformLayout();
        }

        // ════════════════════════════════════════════════════════════════
        //  Pintura del toggle
        // ════════════════════════════════════════════════════════════════

        private void DrawToggleButton(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var pen = new Pen(Color.FromArgb(180, 255, 255, 255), 1.6f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
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

        // ════════════════════════════════════════════════════════════════
        //  Gestión de tabs y contenido
        // ════════════════════════════════════════════════════════════════

        public void ShowTab(int index)
        {
            if (index < 0 || index >= Tabs.Count) return;
            _activeTabIndex = index;
            _selectedItem = null;
            BuildActiveTabContent(Tabs[index]);
            if (!_expanded) BuildIconsStrip();
        }

        private void OnTabClicked(int index)
        {
            _tabStrip.SetActive(index);
            ShowTab(index);
        }

        private void BuildActiveTabContent(SidebarTab tabModel)
        {
            _groupsFlow.SuspendLayout();
            _groupsFlow.Controls.Clear();

            if (tabModel?.Groups == null) { _groupsFlow.ResumeLayout(true); return; }

            // Orden inverso porque DockStyle.Top apila al revés
            for (int g = tabModel.Groups.Count - 1; g >= 0; g--)
            {
                var gm = tabModel.Groups[g];
                var accordion = new SmoothAccordionGroup
                {
                    Title = gm.Title,
                    GroupIcon = gm.GroupIcon,
                    Dock = DockStyle.Top,
                    Tag = gm,
                };

                accordion.MouseClick += (s, e) =>
                {
                    if (e.Y < accordion.HeaderHeight)
                        gm.Collapsed = !accordion.IsExpanded;
                };

                for (int i = gm.Items.Count - 1; i >= 0; i--)
                    AddMenuButton(accordion, gm.Items[i], gm, tabModel);

                if (gm.Collapsed)
                    accordion.Collapse(animate: false);
                else
                    accordion.Expand(animate: false);

                _groupsFlow.Controls.Add(accordion);
            }

            _groupsFlow.ResumeLayout(true);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (Tabs.Count == 0) return;

            int index = ResolveDesiredTabIndex();
            ApplySelectedTabIndex(index);
        }

        private bool ApplySelectedTabIndex(int index)
        {
            if (index < 0 || index >= Tabs.Count) return false;
            if (_activeTabIndex == index && _groupsFlow.Controls.Count > 0) 
                return true;

            _tabStrip.SetActive(index);
            ShowTab(index);
            _pendingSelectedTabIndex = -1;
            _pendingSelectedTabName = null;
            return true;
        }

        private int ResolveDesiredTabIndex()
        {
            if (!string.IsNullOrWhiteSpace(_pendingSelectedTabName))
            {
                int byName = FindTabIndexByName(_pendingSelectedTabName);
                if (byName >= 0) return byName;
            }

            if (_pendingSelectedTabIndex >= 0 && _pendingSelectedTabIndex < Tabs.Count)
                return _pendingSelectedTabIndex;

            if (_activeTabIndex >= 0 && _activeTabIndex < Tabs.Count)
                return _activeTabIndex;

            return 0;
        }

        private int FindTabIndexByName(string name)
        {
            for (int i = 0; i < Tabs.Count; i++)
                if (string.Equals(Tabs[i].Name, name, StringComparison.OrdinalIgnoreCase))
                    return i;

            return -1;
        }

        private void AddMenuButton(SmoothAccordionGroup group,
                                   SidebarItemModel model,
                                   SidebarGroupModel groupModel,
                                   SidebarTab tabModel)
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
                HoverShortcutColor = model.HoverShortcutColor,
                Tag = model, // ← HitTest lo usa para identificar el modelo
            };

            btn.Click += (s, e) =>
            {
                _selectedItem = model;
                // Evento global (útil para routing centralizado)
                ItemClicked?.Invoke(this, new SidebarItemClickEventArgs(model, groupModel, tabModel));
                // Evento individual del ítem (el que el usuario conecta en el diseñador)
                model.RaiseClick();
            };

            group.AddContent(btn);
        }

        // ════════════════════════════════════════════════════════════════
        //  Strip de íconos (colapsado)
        // ════════════════════════════════════════════════════════════════

        private void BuildIconsStrip()
        {
            _iconsStrip.Controls.Clear();
            var groups = _groupsFlow.Controls.OfType<SmoothAccordionGroup>()
                                             .OrderBy(c => c.Top).ToList();
            int y = 0;
            foreach (var grp in groups)
            {
                var iconBtn = new SidebarIconButton(grp)
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

        // ════════════════════════════════════════════════════════════════
        //  Flyout
        // ════════════════════════════════════════════════════════════════

        private void ShowFlyout(SmoothAccordionGroup group)
        {
            CloseFlyout();
            var pos = PointToScreen(new Point(CollapsedWidth + 2, TOGGLE_H));
            _activeFlyout = new FlyoutPanel(this, group, pos);
            _activeFlyout.FormClosed += (s, e) => _activeFlyout = null;
            _activeFlyout.Show(FindForm());
        }

        private void CloseFlyout()
        {
            _activeFlyout?.Close();
            _activeFlyout = null;
        }

        // ════════════════════════════════════════════════════════════════
        //  Dispose
        // ════════════════════════════════════════════════════════════════

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Dispose();
                CloseFlyout();

                foreach (var group in _trackedGroups)
                    group.Changed -= OnModelChanged;
                foreach (var item in _trackedItems)
                    item.Changed -= OnModelChanged;

                _trackedGroups.Clear();
                _trackedItems.Clear();
            }
            base.Dispose(disposing);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  SidebarTab
    // ═══════════════════════════════════════════════════════════════════
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SidebarTab
    {
        private static int _nameCounter;

        [Category("Diseño")]
        public string Name { get; set; }
        [Category("Apariencia")]
        public string Title { get; set; } = "Nueva Pestaña";
        [Category("Apariencia")]
        public Image Icon { get; set; }
        [Category("Apariencia")]
        public new Color BackColor { get; set; } = Color.Transparent;

        [Category("Datos")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<SidebarGroupModel> Groups { get; } = new Collection<SidebarGroupModel>();

        public SidebarTab() { _nameCounter++; Name = $"sidebarTab{_nameCounter}"; }
        public static void ResetNameCounter() => _nameCounter = 0;
        public override string ToString() => string.IsNullOrWhiteSpace(Title) ? Name : Title;
    }

    // ═══════════════════════════════════════════════════════════════════
    //  SidebarTabStrip
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

        public void ClearTabs() { _tabs.Clear(); _active = -1; Invalidate(); }
        public void AddTab(SidebarTab t) { _tabs.Add(t); if (_tabs.Count == 1) _active = 0; Invalidate(); }
        public void SetActive(int i) { _active = i; Invalidate(); }
        public void SetCollapsed(bool c) { _collapsed = c; Invalidate(); }

        private Rectangle TabRect(int i)
        {
            if (_collapsed)
                return (i == _active) ? new Rectangle(0, 0, Width, Height) : Rectangle.Empty;
            if (_tabs.Count == 0 || Width <= 0) return Rectangle.Empty;
            int w = Width / _tabs.Count, extra = Width - w * _tabs.Count, x = 0;
            for (int j = 0; j < i; j++) x += w + (j < extra ? 1 : 0);
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
            if (_collapsed) { OpenFlyout(); return; }
            int idx = TabAtPoint(e.Location);
            if (idx < 0) return;
            _active = idx;
            Invalidate();
            TabClicked?.Invoke(idx);
        }

        private void OpenFlyout()
        {
            if (_flyout != null && !_flyout.IsDisposed) { _flyout.Close(); _flyout = null; return; }
            var screenPos = PointToScreen(new Point(0, 0));
            _flyout = new TabListFlyout(_tabs, _active, screenPos);
            _flyout.TabSelected += (idx) =>
            {
                _active = idx; Invalidate();
                TabClicked?.Invoke(idx);
                _flyout = null;
            };
            _flyout.FormClosed += (s, e) => _flyout = null;
            _flyout.Show(FindForm());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var pen = new Pen(Color.FromArgb(60, 60, 75), 1f))
                g.DrawLine(pen, 0, 0, Width, 0);

            for (int i = 0; i < _tabs.Count; i++)
            {
                if (_collapsed && i != _active) continue;
                var r = TabRect(i);
                DrawTab(g, r, _tabs[i], i == _active, i == _hovered && i != _active, _collapsed);

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
            if (active)
            {
                using var bg = new SolidBrush(Color.FromArgb(50, 50, 65));
                g.FillRectangle(bg, r);
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
                if (tab.Icon != null)
                {
                    int size = 18;
                    g.DrawImage(tab.Icon, r.X + (r.Width - size) / 2, r.Y + 8, size, size);
                }
                else
                {
                    string letter = tab.Title?.Length > 0 ? tab.Title[0].ToString().ToUpper() : "?";
                    using var font = new Font("Segoe UI", 13f, FontStyle.Bold);
                    using var brush = new SolidBrush(active ? Color.White : Color.FromArgb(130, 130, 145));
                    using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(letter, font, brush, new RectangleF(r.X, r.Y - 4, r.Width, r.Height), sf);
                }

                using var arrow = new SolidBrush(Color.FromArgb(100, 140, 255));
                int ax = r.X + r.Width / 2, ay = r.Bottom - 6;
                g.FillPolygon(arrow, new[] {
                    new PointF(ax - 5, ay + 2), new PointF(ax + 5, ay + 2), new PointF(ax, ay - 3)
                });
            }
            else
            {
                int contentY = r.Y + 6;
                if (tab.Icon != null) { g.DrawImage(tab.Icon, r.X + (r.Width - 16) / 2, contentY, 16, 16); contentY += 18; }
                using var font = new Font("Segoe UI", 7.5f);
                using var brush = new SolidBrush(active ? Color.White : Color.FromArgb(130, 130, 145));
                using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
                g.DrawString(tab.Title, font, brush, new RectangleF(r.X + 2, contentY, r.Width - 4, r.Bottom - contentY - 4), sf);
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
            Group = group; Cursor = Cursors.Hand;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            ResizeRedraw = true;
        }

        protected override void OnMouseEnter(EventArgs e) { _hovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hovered = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnClick(EventArgs e) { Invalidate(); base.OnClick(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_hovered) { using var bg = new SolidBrush(Color.FromArgb(55, 55, 68)); g.FillRectangle(bg, ClientRectangle); }

            if (Group.GroupIcon != null)
                g.DrawImage(Group.GroupIcon, (Width - 20) / 2, (Height - 20) / 2 - 4, 20, 20);
            else
            {
                string letter = Group.Title?.Length > 0 ? Group.Title[0].ToString().ToUpper() : "?";
                using var font = new Font("Segoe UI", 13f, FontStyle.Bold);
                using var brush = new SolidBrush(Color.FromArgb(180, 200, 220));
                using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(letter, font, brush, new RectangleF(0, -4, Width, Height), sf);
            }

            string abbr = Group.Title?.Length >= 3 ? Group.Title.Substring(0, 3) : (Group.Title ?? "");
            using var sf2 = new StringFormat { Alignment = StringAlignment.Center };
            using var sfont = new Font("Segoe UI", 7f);
            using var sb = new SolidBrush(Color.FromArgb(110, 115, 130));
            g.DrawString(abbr, sfont, sb, new RectangleF(0, Height - 14, Width, 12), sf2);

            using var pen = new Pen(Color.FromArgb(45, 45, 58), 0.5f);
            g.DrawLine(pen, 8, Height - 1, Width - 8, Height - 1);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  TabListFlyout
    // ═══════════════════════════════════════════════════════════════════
    internal class TabListFlyout : Form
    {
        public event Action<int> TabSelected;
        private const int ITEM_H = 40, FLYOUT_W = 200;

        public TabListFlyout(List<SidebarTab> tabs, int activeIndex, Point screenPos)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            BackColor = Color.FromArgb(30, 30, 40);
            Size = new Size(FLYOUT_W, tabs.Count * ITEM_H + 2);
            Location = new Point(screenPos.X, screenPos.Y - Height);

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            int y = 1;
            for (int i = 0; i < tabs.Count; i++)
            {
                int idx = i;
                var item = new TabFlyoutItem(tabs[i], i == activeIndex) { Top = y, Left = 0, Width = FLYOUT_W, Height = ITEM_H };
                item.Click += (s, e) => { TabSelected?.Invoke(idx); Close(); };
                Controls.Add(item);
                y += ITEM_H;
            }
            Deactivate += (s, e) => Close();
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
            get { var cp = base.CreateParams; cp.ExStyle |= 0x02000000; return cp; }
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  TabFlyoutItem
    // ═══════════════════════════════════════════════════════════════════
    internal class TabFlyoutItem : Panel
    {
        private readonly SidebarTab _tab;
        private readonly bool _active;
        private bool _hovered;

        public TabFlyoutItem(SidebarTab tab, bool active)
        {
            _tab = tab; _active = active; Cursor = Cursors.Hand;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected override void OnMouseEnter(EventArgs e) { _hovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hovered = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_active)
            {
                using var bg = new SolidBrush(Color.FromArgb(50, 50, 68)); g.FillRectangle(bg, ClientRectangle);
                using var acc = new SolidBrush(Color.FromArgb(100, 140, 255)); g.FillRectangle(acc, 0, 0, 3, Height);
            }
            else if (_hovered) { using var bg = new SolidBrush(Color.FromArgb(42, 42, 56)); g.FillRectangle(bg, ClientRectangle); }

            int textX = 14;
            if (_tab.Icon != null) { g.DrawImage(_tab.Icon, 12, (Height - 16) / 2, 16, 16); textX = 36; }

            using var font = new Font("Segoe UI", 9f, _active ? FontStyle.Bold : FontStyle.Regular);
            using var brush = new SolidBrush(_active ? Color.White : Color.FromArgb(170, 170, 185));
            using var sf = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
            g.DrawString(_tab.Title, font, brush, new RectangleF(textX, 0, Width - textX - 8, Height), sf);

            using var pen = new Pen(Color.FromArgb(45, 45, 58), 0.5f);
            g.DrawLine(pen, 8, Height - 1, Width - 8, Height - 1);
        }
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

        private bool _hovered, _pressed;

        public SidebarMenuButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.Selectable, true);
            Height = 34; Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9f);
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovered = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovered = false; _pressed = false; Invalidate(); }
        protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); _pressed = true; Invalidate(); }
        protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); _pressed = false; Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Color bg = _pressed ? Color.FromArgb(40, 40, 58) : (_hovered ? HoverBackColor : NormalBackColor);
            using (var br = new SolidBrush(bg)) g.FillRectangle(br, ClientRectangle);

            int x = 20;
            if (Icon != null) { g.DrawImage(Icon, x, (Height - 16) / 2, 16, 16); x += 28; }

            using (var tb = new SolidBrush(_hovered ? HoverForeColor : NormalForeColor))
            using (var sf = new StringFormat { LineAlignment = StringAlignment.Center })
                g.DrawString(Text, Font, tb, new Rectangle(x, 0, Width - x - 60, Height), sf);

            if (!string.IsNullOrEmpty(Shortcut))
            {
                using var sb = new SolidBrush(_hovered ? HoverShortcutColor : NormalShortcutColor);
                using var sf = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
                using var sf2 = new Font(Font.FontFamily, Font.Size - 1.5f);
                g.DrawString(Shortcut, sf2, sb, new Rectangle(0, 0, Width - 15, Height), sf);
            }
        }
    }
}