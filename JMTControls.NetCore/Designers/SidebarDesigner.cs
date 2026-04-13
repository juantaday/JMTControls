using JMTControls.NetCore.Controls;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace JMTControls.NetCore.Designers
{
    // ═══════════════════════════════════════════════════════════════════
    //  SidebarDesigner
    //
    //  Responsabilidades:
    //  1. Al hacer clic sobre un ítem o grupo en diseño → lo selecciona
    //     en la ventana de Propiedades (mostrando sus propiedades Y eventos).
    //  2. Al hacer DOBLE clic sobre un ítem → genera el handler del evento
    //     Click en el code-behind (igual que DevExpress AccordionControl).
    //  3. Registra los SidebarItemModel/SidebarGroupModel como Components
    //     en el IContainer del formulario para que VS los serialice.
    // ═══════════════════════════════════════════════════════════════════
    public class SidebarDesigner : ParentControlDesigner
    {
        private ISelectionService _selectionService;
        private IDesignerHost _designerHost;
        private IEventBindingService _eventBindingService;
        private IComponentChangeService _changeService;
        private SidebarContainer _sidebar;
        private readonly HashSet<Control> _hookedControls = new();

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            _selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
            _designerHost = GetService(typeof(IDesignerHost)) as IDesignerHost;
            _eventBindingService = GetService(typeof(IEventBindingService)) as IEventBindingService;
            _changeService = GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            // Registrar los modelos existentes en el IContainer del diseñador
            if (component is SidebarContainer sidebar)
            {
                _sidebar = sidebar;
                _sidebar.RegisterModelsWithDesigner(_designerHost);
                _sidebar.RegisterModelsWithContainer(_sidebar.Site?.Container);

                try
                {
                    EnableDesignMode(_sidebar.DesignerGroupsHost, "SidebarGroupsHost");
                    EnableDesignMode(_sidebar.DesignerTabStripHost, "SidebarTabStripHost");
                }
                catch { }

                _sidebar.MouseDown += Sidebar_MouseDown;
                _sidebar.MouseDoubleClick += Sidebar_MouseDoubleClick;
                AttachDesignInputHooks();
            }
        }

        private void AttachDesignInputHooks()
        {
            if (_sidebar == null) return;

            HookControl(_sidebar.DesignerTabStripHost);
            HookControl(_sidebar.DesignerGroupsHost);

            if (_sidebar.DesignerGroupsHost != null)
            {
                _sidebar.DesignerGroupsHost.ControlAdded -= GroupsHost_ControlAdded;
                _sidebar.DesignerGroupsHost.ControlAdded += GroupsHost_ControlAdded;

                foreach (Control c in _sidebar.DesignerGroupsHost.Controls)
                    HookControlTree(c);
            }
        }

        private void GroupsHost_ControlAdded(object sender, ControlEventArgs e) => HookControlTree(e.Control);

        private void HookControlTree(Control root)
        {
            if (root == null) return;
            HookControl(root);
            foreach (Control c in root.Controls)
                HookControlTree(c);
        }

        private void HookControl(Control control)
        {
            if (control == null || _hookedControls.Contains(control)) return;
            _hookedControls.Add(control);
            control.MouseDown += Child_MouseDown;
            control.MouseDoubleClick += Child_MouseDoubleClick;
        }

        private void Child_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _sidebar == null) return;
            var ctrl = sender as Control;
            if (ctrl == null) return;
            var pt = _sidebar.PointToClient(ctrl.PointToScreen(e.Location));
            HandleDesignerClick(pt, isDoubleClick: false);
        }

        private void Child_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _sidebar == null) return;
            var ctrl = sender as Control;
            if (ctrl == null) return;
            var pt = _sidebar.PointToClient(ctrl.PointToScreen(e.Location));
            HandleDesignerClick(pt, isDoubleClick: true);
        }

        protected override bool GetHitTest(Point point)
        {
            var sidebar = Control as SidebarContainer;
            if (sidebar == null) return base.GetHitTest(point);

            var clientPt = sidebar.PointToClient(point);
            if (sidebar.ClientRectangle.Contains(clientPt))
                return true;

            return base.GetHitTest(point);
        }

        private void Sidebar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            HandleDesignerClick(e.Location, isDoubleClick: false);
        }

        private void Sidebar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            HandleDesignerClick(e.Location, isDoubleClick: true);
        }

        private bool HandleDesignerClick(Point pt, bool isDoubleClick)
        {
            var sidebar = _sidebar ?? (Control as SidebarContainer);
            if (sidebar == null) return false;

            TrySelectSidebar();

            if (sidebar.ActivateTabFromDesigner(pt))
            {
                NotifySelectedTabChanged(sidebar);
                AttachDesignInputHooks();
                Debug.WriteLine($"[SidebarDesigner] Tab click captured at {pt}");
                return true;
            }

            sidebar.ToggleGroupFromDesignerIfChevron(pt);

            object hitObject = sidebar.HitTestItem(pt);
            if (hitObject == null)
            {
                Point cursorPt = sidebar.PointToClient(Cursor.Position);
                hitObject = sidebar.HitTestItem(cursorPt);
            }

            if (hitObject == null)
            {
                Trace.WriteLine($"[SidebarDesigner] Click captured but no hit object at {pt}");
                Debug.WriteLine($"[SidebarDesigner] Click captured but no hit object at {pt}");
                return false;
            }

            if (hitObject is IComponent comp && comp.Site == null)
                sidebar.RegisterModelsWithContainer(sidebar.Site?.Container);

            Trace.WriteLine($"[SidebarDesigner] Selected {hitObject.GetType().Name}");
            Debug.WriteLine($"[SidebarDesigner] Selected {hitObject.GetType().Name}");
            TrySetSelection(hitObject);

            if (isDoubleClick && hitObject is SidebarItemModel model)
                GenerateClickHandler(model);

            return true;
        }

        private void TrySelectSidebar()
        {
            if (_sidebar == null) return;
            TrySetSelection(_sidebar);
        }

        private void TrySetSelection(object obj)
        {
            if (_selectionService == null || obj == null) return;

            try
            {
                var current = _selectionService.GetSelectedComponents();
                bool alreadySelected = current?.Cast<object>().Any(c => ReferenceEquals(c, obj)) == true;
                if (alreadySelected) return;

                _selectionService.SetSelectedComponents(new[] { obj }, SelectionTypes.Replace | SelectionTypes.Primary);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[SidebarDesigner] Selection failed: {ex.Message}");
                Debug.WriteLine($"[SidebarDesigner] Selection failed: {ex.Message}");
            }
        }

        private void NotifySelectedTabChanged(SidebarContainer sidebar)
        {
            if (sidebar == null) return;

            var pd = TypeDescriptor.GetProperties(sidebar)["SelectedTabIndex"];
            if (pd == null) return;

            int value = sidebar.ActiveTabIndex;
            try
            {
                _changeService?.OnComponentChanging(sidebar, pd);
                pd.SetValue(sidebar, value);
                _changeService?.OnComponentChanged(sidebar, pd, null, value);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[SidebarDesigner] NotifySelectedTabChanged failed: {ex.Message}");
                Debug.WriteLine($"[SidebarDesigner] NotifySelectedTabChanged failed: {ex.Message}");
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_LBUTTONDBLCLK = 0x0203;

            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONDBLCLK)
            {
                var sidebar = _sidebar ?? (Control as SidebarContainer);
                if (sidebar != null)
                {
                    Point pt = sidebar.PointToClient(Cursor.Position);
                    if (HandleDesignerClick(pt, m.Msg == WM_LBUTTONDBLCLK))
                        return;
                }
            }

            base.WndProc(ref m);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _sidebar != null)
            {
                _sidebar.MouseDown -= Sidebar_MouseDown;
                _sidebar.MouseDoubleClick -= Sidebar_MouseDoubleClick;

                if (_sidebar.DesignerGroupsHost != null)
                    _sidebar.DesignerGroupsHost.ControlAdded -= GroupsHost_ControlAdded;

                foreach (var ctrl in _hookedControls.ToList())
                {
                    ctrl.MouseDown -= Child_MouseDown;
                    ctrl.MouseDoubleClick -= Child_MouseDoubleClick;
                }
                _hookedControls.Clear();

                _sidebar = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Genera el método handler para el evento Click del modelo en el
        /// code-behind del formulario, igual que hace DevExpress.
        /// </summary>
        private void GenerateClickHandler(SidebarItemModel model)
        {
            if (_eventBindingService == null || _designerHost == null) return;

            try
            {
                // Obtener el EventDescriptor del evento "Click" del modelo
                EventDescriptorCollection events = TypeDescriptor.GetEvents(model);
                EventDescriptor clickEvent = events["Click"];

                if (clickEvent == null) return;

                // Si ya tiene handler asignado, navega a él; si no, lo crea.
                string handlerName = _eventBindingService.GetEventProperty(clickEvent).GetValue(model) as string;

                if (string.IsNullOrEmpty(handlerName))
                {
                    // Genera nombre estilo: sidebarItem1_Click
                    handlerName = $"{model.Name}_Click";
                    _eventBindingService.GetEventProperty(clickEvent).SetValue(model, handlerName);
                }

                _eventBindingService.ShowCode(model, clickEvent);
            }
            catch
            {
                // En caso de error del diseñador, simplemente no generamos el handler.
            }
        }
    }
}