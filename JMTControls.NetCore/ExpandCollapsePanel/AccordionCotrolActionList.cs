using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMTControls.NetCore.ExpandCollapsePanel
{
    internal class AccordionCotrolActionList :    System.ComponentModel.Design.DesignerActionList
    {
        private AccordionCotrol accordionCtrol;
        private bool colorLuked;
        private DesignerActionUIService designerActionUISvc = null;

        public AccordionCotrolActionList(IComponent component) : base(component)
        {
            this.accordionCtrol = component as AccordionCotrol;

            // Cache a reference to DesignerActionUIService, so the 
            // DesigneractionList can be refreshed. 
            this.designerActionUISvc =
                GetService(typeof(DesignerActionUIService))
                as DesignerActionUIService;
        }

        // Helper method to retrieve control properties. Use of  
        // GetProperties enables undo and menu updates to work properly. 
        private PropertyDescriptor GetPropertyByName(String propName)
        {
            PropertyDescriptor prop;
            prop = TypeDescriptor.GetProperties(accordionCtrol)[propName];
            if (null == prop)
                throw new ArgumentException(
                     "Matching ColorLabel property not found!",
                      propName);
            else
                return prop;
        }

        public bool LockColors
        {
            get
            {
                return colorLuked;
            }
            set
            {
                // GetPropertyByName("ColorLocked").SetValue(colLabel, value);
                colorLuked= value;  
                // Refresh the list. 
                this.designerActionUISvc.Refresh(this.Component);
            }
        }


        // Properties that are targets of DesignerActionPropertyItem entries. 
        public Color BackColor
        {
            get
            {
                return accordionCtrol.BackColor;
            }
            set
            {
                GetPropertyByName("BackColor").SetValue(accordionCtrol, value);
            }
        }

        public Color ForeColor
        {
            get
            {
                return accordionCtrol.ForeColor;
            }
            set
            {
                GetPropertyByName("ForeColor").SetValue(accordionCtrol, value);
            }
        }

        public DockStyle Dock
        {
            get
            {
                return accordionCtrol.Dock;
            }
            set
            {
                GetPropertyByName("Dock").SetValue(accordionCtrol, value);

                this.designerActionUISvc.Refresh(this.Component);
            }
        }

        public Color ButtonBackColor { 
            get=>this.accordionCtrol.ButtonBackColor;
            set
            {
                GetPropertyByName("ButtonBackColor").SetValue(accordionCtrol, value);

                this.designerActionUISvc.Refresh(this.Component);
            }
        }

        public Color ButtonBackColorHover
        {
            get => this.accordionCtrol.ButtonBackColorHover;
            set
            {
                GetPropertyByName("ButtonBackColorHover").SetValue(accordionCtrol, value);

                this.designerActionUISvc.Refresh(this.Component);
            }
        }

        public int HeaderHeight
        {
            get => this.accordionCtrol.HeaderHeight;
            set
            {
                GetPropertyByName("HeaderHeight").SetValue(accordionCtrol, value);

                this.designerActionUISvc.Refresh(this.Component);
            }
        }

       
        // Implementation of this abstract method creates smart tag   
        // items, associates their targets, and collects into list. 
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            //Define static section header entries.
            items.Add(new DesignerActionHeaderItem("Appearance"));
            items.Add(new DesignerActionHeaderItem("Controls"));
            items.Add(new DesignerActionHeaderItem("Information"));

            //Boolean property for locking color selections.
            items.Add(new DesignerActionPropertyItem("Dock",
                             "Dock style", "Appearance",
                             "Position and form of coupling to the primary control"));

            items.Add(new DesignerActionPropertyItem("LockColors",
                           "Lock Colors", "Appearance",
                           "Locks the color properties."));

         

            if (LockColors)
            {
                items.Add(new DesignerActionPropertyItem("BackColor",
                                 "Back Color", "Appearance",
                                 "Selects the background color."));
                items.Add(new DesignerActionPropertyItem("ForeColor",
                                 "Fore Color", "Appearance",
                                 "Selects the foreground color."));

                items.Add(new DesignerActionPropertyItem("ButtonBackColor",
                               "Button back bolor", "Appearance",
                               "Back colo of the button that allows collapsing"));

                items.Add(new DesignerActionPropertyItem("ButtonBackColorHover",
                              "Button hover bolor", "Appearance",
                              "button background color when you move the mouse"));



                //This next method item is also added to the context menu  
                // (as a designer verb).


                //items.Add(new DesignerActionMethodItem(this,
                //                 "InvertColors", "Invert Colors",
                //                 "Appearance",
                //                 "Inverts the fore and background colors.",
                //                  true));

            }


            items.Add(new DesignerActionPropertyItem("HeaderHeight",
                       "Header height", "Appearance",
                       "Height of the space where is the button and the logo"));


            //Create entries for static Information section.
            StringBuilder location = new StringBuilder("Location:");
            location.Append(accordionCtrol.Location);

            StringBuilder size = new StringBuilder("Size: ");
            size.Append(accordionCtrol.Size);

            items.Add(new DesignerActionTextItem(location.ToString(),
                             "Information"));
            items.Add(new DesignerActionTextItem(size.ToString(),
                             "Information"));

            return items;
        }



    }
}
