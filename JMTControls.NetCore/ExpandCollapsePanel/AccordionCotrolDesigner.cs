using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMTControls.NetCore.ExpandCollapsePanel
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    internal class AccordionCotrolDesigner : System.Windows.Forms.Design.ScrollableControlDesigner
    {
        private DesignerActionListCollection actionLists;

        // Use pull model to populate smart tag menu. 
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (null == actionLists)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(
                        new AccordionCotrolActionList(this.Component));
                }
                return actionLists;
            }
        }

    }
}
