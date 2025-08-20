using JMTControls.NetCore.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace JMTControls.NetCore.Class
{
    [ListBindable(false)]
    public class ObjectCollection : IList, ICollection, IEnumerable
    {
        private readonly JMComboBox owner;
        private readonly List<object> items;

        internal ObjectCollection(JMComboBox owner)
        {
            this.owner = owner;
            this.items = new List<object>();
        }

        public int Count => items.Count;
        public bool IsReadOnly => false;
        public bool IsFixedSize => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;

        public object this[int index]
        {
            get => items[index];
            set
            {
                items[index] = value;
                owner?.RefreshItems();
            }
        }

        public int Add(object item)
        {
            items.Add(item);
            owner?.RefreshItems();
            return items.Count - 1;
        }

        public int Add(string text)
        {
            items.Add(text);
            owner?.RefreshItems();
            return items.Count - 1;
        }

        public void Clear()
        {
            items.Clear();
            owner?.RefreshItems();
        }

        public bool Contains(object item) => items.Contains(item);
        public int IndexOf(object item) => items.IndexOf(item);

        public void Insert(int index, object item)
        {
            items.Insert(index, item);
            owner?.RefreshItems();
        }

        public void Remove(object item)
        {
            items.Remove(item);
            owner?.RefreshItems();
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            owner?.RefreshItems();
        }

        public IEnumerator GetEnumerator() => items.GetEnumerator();
        public void CopyTo(Array array, int index) => ((ICollection)items).CopyTo(array, index);

        public void AddRange(object[] items)
        {
            this.items.AddRange(items);
            owner?.RefreshItems();
        }

        public void AddRange(string[] items)
        {
            this.items.AddRange(items.Cast<object>());
            owner?.RefreshItems();
        }

        // Métodos para serialización del diseñador
        public string[] ToStringArray()
        {
            return items.Select(item => item?.ToString() ?? string.Empty).ToArray();
        }

        public void FromStringArray(string[] stringArray)
        {
            items.Clear();
            if (stringArray != null)
            {
                items.AddRange(stringArray.Cast<object>());
                owner?.RefreshItems();
            }
        }
    }

    // Convertidor de tipos para string array
    public class StringArrayConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return stringValue.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is string[] stringArray)
            {
                return string.Join(Environment.NewLine, stringArray);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    // Editor personalizado que funciona con .NET 8
    public class ComboBoxStringCollectionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value is ObjectCollection collection)
            {
                using (var form = new StringCollectionForm())
                {
                    // Cargar elementos existentes
                    form.SetItems(collection.ToStringArray());
                    
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Actualizar la colección con los nuevos valores
                        collection.FromStringArray(form.GetItems());
                    }
                }
                
                return collection;
            }
            
            return base.EditValue(context, provider, value);
        }
    }

    // Formulario simple para editar la colección de strings
    public partial class StringCollectionForm : Form
    {
        private TextBox textBox;
        private Button okButton;
        private Button cancelButton;

        public StringCollectionForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.textBox = new TextBox();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.SuspendLayout();

            // Form
            this.Text = "Editor de elementos";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // TextBox
            this.textBox.Multiline = true;
            this.textBox.ScrollBars = ScrollBars.Vertical;
            this.textBox.Location = new System.Drawing.Point(12, 12);
            this.textBox.Size = new System.Drawing.Size(360, 200);
            this.textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // OK Button
            this.okButton.Text = "Aceptar";
            this.okButton.Location = new System.Drawing.Point(217, 227);
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.DialogResult = DialogResult.OK;
            this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Cancel Button
            this.cancelButton.Text = "Cancelar";
            this.cancelButton.Location = new System.Drawing.Point(297, 227);
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Add controls
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);

            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;

            this.ResumeLayout(false);
        }

        public void SetItems(string[] items)
        {
            if (items != null)
            {
                textBox.Text = string.Join(Environment.NewLine, items);
            }
        }

        public string[] GetItems()
        {
            return textBox.Text
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
        }
    }
}
