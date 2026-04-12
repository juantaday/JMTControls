using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace JMTControls.NetCore.Controls
{
    public class SidebarTabCollection : Collection<SidebarTab>
    {
        private readonly SidebarContainer _owner;

        public SidebarTabCollection(SidebarContainer owner)
        {
            _owner = owner;
        }

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
            // Al limpiar, reiniciamos los contadores para que vuelvan desde 1
            SidebarTab.ResetNameCounter();
            SidebarGroupModel.ResetNameCounter();
            SidebarItemModel.ResetNameCounter();
            base.ClearItems();
            _owner.OnDesignerTabsChanged();
        }
    }


    // ═══════════════════════════════════════════════════════════════════
    //  SidebarItemModel — modelo de ítem de menú
    // ═══════════════════════════════════════════════════════════════════
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SidebarItemModel
    {
        private static int _nameCounter = 0;

        [Category("Diseño")]
        [Description("Nombre único del componente (usado en código).")]
        public string Name { get; set; }

        [Category("Datos")]
        public string Text { get; set; } = "Nuevo Item";

        [Category("Datos")]
        public string Shortcut { get; set; } = "";

        [Category("Apariencia - Datos")]
        public Image Icon { get; set; }

        [Category("Apariencia - Fuentes")]
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

        public SidebarItemModel()
        {
            _nameCounter++;
            Name = $"sidebarItem{_nameCounter}";
        }

        public static void ResetNameCounter() => _nameCounter = 0;

        public override string ToString() => Text;
    }


    // ═══════════════════════════════════════════════════════════════════
    //  SidebarGroupModel — modelo de grupo (acordeón)
    // ═══════════════════════════════════════════════════════════════════
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SidebarGroupModel
    {
        private static int _nameCounter = 0;

        [Category("Diseño")]
        [Description("Nombre único del componente (usado en código).")]
        public string Name { get; set; }

        [Category("Datos")]
        public string Title { get; set; } = "Nuevo Grupo";

        [Category("Datos")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<SidebarItemModel> Items { get; } = new Collection<SidebarItemModel>();

        public SidebarGroupModel()
        {
            _nameCounter++;
            Name = $"sidebarGroup{_nameCounter}";
        }

        public static void ResetNameCounter() => _nameCounter = 0;

        public override string ToString() => Title;
    }
}