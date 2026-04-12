using JMTControls.NetCore.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void sidebarContainer1_ItemClicked(object sender, SidebarItemClickEventArgs e)
        {
            MessageBox.Show($"Hiciste clic en {e.Item.Text} que pertenece al grupo {e.Group.Title}");
        }
    }
}
