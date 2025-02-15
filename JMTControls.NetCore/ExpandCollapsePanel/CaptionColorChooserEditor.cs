using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMTControls.NetCore.ExpandCollapsePanel
{
    public partial class JMTabAccordion
    {

        public class CaptionColorChooserEditor : UITypeEditor
        {
            #region Destructor

            ~CaptionColorChooserEditor()
            {
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Override Methods

            public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(
                System.ComponentModel.ITypeDescriptorContext context)
            {
                // We will use a window for property editing.
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(
                System.ComponentModel.ITypeDescriptorContext context,
                System.IServiceProvider provider, object value)
            {
                ICaptionRandomizer current;
                using (CaptionColorChooser frm = new CaptionColorChooser())
                {
                    // Set currently objects to the form.
                    frm.Randomizer = (ICaptionRandomizer)value;
                    frm.contextInstance = context.Instance as JMTabAccordion;

                    if (frm.ShowDialog() == DialogResult.OK)
                        current = frm.Randomizer;
                    else
                        current = (ICaptionRandomizer)value;
                }

                return current;
            }

            public override bool GetPaintValueSupported(
                System.ComponentModel.ITypeDescriptorContext context)
            {
                // No special thumbnail will be shown for the grid.
                return false;
            }

            #endregion
        }

    }
}
