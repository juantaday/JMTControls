using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace JMTControls.NetCore.Controls
{
    // ═══════════════════════════════════════════════════════════════════
    //  SidebarTabCollection
    // ═══════════════════════════════════════════════════════════════════
    public class SidebarTabCollection : Collection<SidebarTab>
    {
        private readonly SidebarContainer _owner;

        public SidebarTabCollection(SidebarContainer owner) => _owner = owner;

        protected override void InsertItem(int index, SidebarTab item)
        {
            base.InsertItem(index, item);
            _owner.OnDesignerTabAdded(item);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            _owner.OnDesignerTabsChanged();
        }

        protected override void ClearItems()
        {
            SidebarTab.ResetNameCounter();
            SidebarGroupModel.ResetNameCounter();
            SidebarItemModel.ResetNameCounter();
            base.ClearItems();
            _owner.OnDesignerTabsChanged();
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  SidebarItemModel — hereda de Component
    //
    //  Heredar de Component permite que el diseñador de VS:
    //   • Muestre la pestaña "Eventos" (rayo) en la ventana de Propiedades.
    //   • Genere el método handler en el code-behind al hacer doble clic.
    //   • Serialice el suscriptor en InitializeComponent().
    //   • Gestione el ciclo de vida del objeto (IContainer).
    //
    //  Flujo de uso en diseño:
    //   1. Clic en un "Nuevo Item" en el sidebar → HitTest lo selecciona.
    //   2. La ventana de Propiedades muestra sus propiedades Y eventos.
    //   3. Doble clic en "Click" → VS genera:
    //        private void sidebarItem1_Click(object sender, EventArgs e) { }
    //   4. En runtime, SidebarContainer llama a model.RaiseClick() al pulsar el botón.
    // ═══════════════════════════════════════════════════════════════════
    [ToolboxItem(false)]
    [DesignTimeVisible(false)]
    [DefaultEvent("Click")]
    public class SidebarItemModel : Component
    {
        private static int _nameCounter;
        private string _name;
        private string _text = "Nuevo Item";
        private string _shortcut = "";
        private Image _icon;

        // Usamos EventHandlerList (heredado de Component) para que el diseñador
        // de VS pueda leer y serializar los eventos correctamente.
        private static readonly object _clickKey = new object();

        internal event EventHandler Changed;

        private void NotifyChanged() => Changed?.Invoke(this, EventArgs.Empty);

        // ── Evento Click ─────────────────────────────────────────────────
        [Category("Acción")]
        [Description("Se dispara cuando el usuario hace clic en este ítem del menú.")]
        public event EventHandler Click
        {
            add => Events.AddHandler(_clickKey, value);
            remove => Events.RemoveHandler(_clickKey, value);
        }

        // ── Diseño ───────────────────────────────────────────────────────
        [Category("Diseño")]
        [Description("Nombre único del componente (usado en código).")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyChanged();
            }
        }

        // ── Datos ────────────────────────────────────────────────────────
        [Category("Datos")]
        [Description("Texto visible en el menú.")]
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                NotifyChanged();
            }
        }

        [Category("Datos")]
        [Description("Atajo de teclado mostrado a la derecha.")]
        public string Shortcut
        {
            get => _shortcut;
            set
            {
                if (_shortcut == value) return;
                _shortcut = value;
                NotifyChanged();
            }
        }

        [Category("Datos")]
        [Description("Ícono del ítem.")]
        public Image Icon
        {
            get => _icon;
            set
            {
                if (ReferenceEquals(_icon, value)) return;
                _icon = value;
                NotifyChanged();
            }
        }

        // ── Apariencia ───────────────────────────────────────────────────
        [Category("Apariencia")]
        public Font Font { get; set; } = new Font("Segoe UI", 9f);

        [Category("Apariencia - Normal")]
        public Color NormalBackColor { get; set; } = Color.Transparent;
        [Category("Apariencia - Normal")]
        public Color NormalForeColor { get; set; } = Color.FromArgb(195, 195, 210);
        [Category("Apariencia - Normal")]
        public Color NormalShortcutColor { get; set; } = Color.FromArgb(90, 95, 115);

        [Category("Apariencia - Hover")]
        public Color HoverBackColor { get; set; } = Color.FromArgb(50, 50, 65);
        [Category("Apariencia - Hover")]
        public Color HoverForeColor { get; set; } = Color.White;
        [Category("Apariencia - Hover")]
        public Color HoverShortcutColor { get; set; } = Color.FromArgb(130, 135, 155);

        // ── Constructores ────────────────────────────────────────────────
        public SidebarItemModel()
        {
            _nameCounter++;
            Name = $"sidebarItem{_nameCounter}";
        }

        /// <summary>Constructor con IContainer: el diseñador de VS lo registra
        /// automáticamente en el contenedor del formulario (components).</summary>
        public SidebarItemModel(IContainer container) : this()
        {
            container?.Add(this, Name);
        }

        // ── API ──────────────────────────────────────────────────────────
        public static void ResetNameCounter() => _nameCounter = 0;
        public override string ToString() => Text;

        /// <summary>Dispara el evento Click desde SidebarContainer.</summary>
        internal void RaiseClick()
        {
            var handler = (EventHandler)Events[_clickKey];
            handler?.Invoke(this, EventArgs.Empty);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  SidebarGroupModel — también Component para consistencia
    // ═══════════════════════════════════════════════════════════════════
    [ToolboxItem(false)]
    [DesignTimeVisible(false)]
    public class SidebarGroupModel : Component
    {
        private static int _nameCounter;
        private string _name;
        private string _title = "Nuevo Grupo";
        private Image _groupIcon;
        private bool _collapsed;

        internal event EventHandler Changed;

        private void NotifyChanged() => Changed?.Invoke(this, EventArgs.Empty);

        [Category("Diseño")]
        [Description("Nombre único del componente.")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyChanged();
            }
        }

        [Category("Datos")]
        [Description("Título del grupo en la cabecera del acordeón.")]
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                NotifyChanged();
            }
        }

        [Category("Apariencia")]
        [Description("Ícono de la cabecera del grupo.")]
        public Image GroupIcon
        {
            get => _groupIcon;
            set
            {
                if (ReferenceEquals(_groupIcon, value)) return;
                _groupIcon = value;
                NotifyChanged();
            }
        }

        [Category("Datos")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<SidebarItemModel> Items { get; } = new Collection<SidebarItemModel>();

        [Category("Comportamiento")]
        [Description("Indica si el grupo inicia colapsado.")]
        [DefaultValue(false)]
        public bool Collapsed
        {
            get => _collapsed;
            set
            {
                if (_collapsed == value) return;
                _collapsed = value;
                NotifyChanged();
            }
        }

        public SidebarGroupModel()
        {
            _nameCounter++;
            Name = $"sidebarGroup{_nameCounter}";
        }

        public SidebarGroupModel(IContainer container) : this()
        {
            container?.Add(this, Name);
        }

        public static void ResetNameCounter() => _nameCounter = 0;
        public override string ToString() => Title;
    }
}