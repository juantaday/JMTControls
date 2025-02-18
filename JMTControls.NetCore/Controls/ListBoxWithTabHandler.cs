namespace JMTControls.NetCore.Controls
{
    using System;
    using System.Windows.Forms;
    public class ListBoxWithTabHandler : ListBox
    {
        public event EventHandler KeyTabPressed;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                // Notificar que se presionó Tab
                KeyTabPressed?.Invoke(this, EventArgs.Empty);

                // Indica que la tecla Tab ha sido manejada
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
