using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.CheckedListBox;

namespace JMControls.Implementation
{
    /// <summary>
    /// OLVListItems are specialized ListViewItems that know which row object they came from,
    /// and the row index at which they are displayed, even when in group view mode. They
    /// also know the image they should draw against themselves
    /// </summary>
    public class OLVCheckedListBoxItem : ItemListBox
    {
        #region Constructors

        /// <summary>
        /// Create a OLVListItem for the given row object
        /// </summary>
        public OLVCheckedListBoxItem(CheckedListBox checkedListBox) :base(checkedListBox)
        {
        }

        public new void Add(object rowObject)
        {
            base.Add(rowObject);
            this.rowObject = rowObject;
        }

        public  void Clear() {

            this.Items.Clear();
        }

        /// <summary>
        /// Create a OLVListItem for the given row object, represented by the given string and image
        /// </summary>

        #endregion.

        #region Properties


        /// <summary>
        /// Gets or sets how many pixels will be left blank around each cell of this item
        /// </summary>
        /// <remarks>This setting only takes effect when the control is owner drawn.</remarks>
        public Rectangle? CellPadding
        {
            get { return this.cellPadding; }
            set { this.cellPadding = value; }
        }
        private Rectangle? cellPadding;

        /// <summary>
        /// Gets or sets how the cells of this item will be vertically aligned
        /// </summary>
        /// <remarks>This setting only takes effect when the control is owner drawn.</remarks>
        public StringAlignment? CellVerticalAlignment
        {
            get { return this.cellVerticalAlignment; }
            set { this.cellVerticalAlignment = value; }
        }
        private StringAlignment? cellVerticalAlignment;

       

        /// <summary>
        /// Enable tri-state checkbox.
        /// </summary>
        /// <remarks>.NET's Checked property was not built to handle tri-state checkboxes,
        /// and will return True for both Checked and Indeterminate states.</remarks>
     
    
        /// <summary>
        /// Gets whether or not this row can be selected and activated
        /// </summary>
        public bool Enabled
        {
            get { return this.enabled; }
            internal set { this.enabled = value; }
        }
        private bool enabled;

    
        /// <summary>
        /// Gets or sets the the model object that is source of the data for this list item.
        /// </summary>
        public object RowObject
        {
            get { return rowObject; }
            set { rowObject = value; }
        }

        private object rowObject;

        /// <summary>
        /// Gets or sets the color that will be used for this row's background when it is selected and 
        /// the control is focused.
        /// </summary>
        /// <remarks>
        /// <para>To work reliably, this property must be set during a FormatRow event.</para>
        /// <para>
        /// If this is not set, the normal selection BackColor will be used.
        /// </para>
        /// </remarks>
        public Color? SelectedBackColor
        {
            get { return this.selectedBackColor; }
            set { this.selectedBackColor = value; }
        }
        private Color? selectedBackColor;

        /// <summary>
        /// Gets or sets the color that will be used for this row's foreground when it is selected and 
        /// the control is focused.
        /// </summary>
        /// <remarks>
        /// <para>To work reliably, this property must be set during a FormatRow event.</para>
        /// <para>
        /// If this is not set, the normal selection ForeColor will be used.
        /// </para>
        /// </remarks>
        public Color? SelectedForeColor
        {
            get { return this.selectedForeColor; }
            set { this.selectedForeColor = value; }
        }
        private Color? selectedForeColor;

        #endregion

        #region Accessing



        #endregion
    }

    public class ItemListBox : ListBox
    {

        public ItemListBox(ListBox listBox)
        {

        }
        public void Add(object rowObject) {
        
        }
    }
}
