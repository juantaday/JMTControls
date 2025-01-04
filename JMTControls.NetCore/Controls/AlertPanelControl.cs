using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMTControls.NetCore.Controls
{
    public partial class AlertPanelControl : UserControl
    {
        private bool VisibleFrame;
        private int _FramCount  ;
        private int _Interval ;
        private int currentInterval;
        public AlertPanelControl()
        {
            InitializeComponent();
            VisibleFrame = true;
            _FramCount = 8;
            _Interval = 25;
        }

        //
        // Resumen:
        //     Obtiene o establece un valor que indica si se muestran el control y todos sus
        //     controles secundarios.
        //
        // Devuelve:
        //     true si se muestran el control y todos sus controles secundarios; en caso contrario,
        //     false. De manera predeterminada, es true.
        [Localizable(true)]
        [Category("CatBehavior")]
        [Description("ControlVisibleDescr")]
        public  bool VisiblePanel
        { 
            get => VisibleFrame;
            set  {
                VisibleFrame = value;
                if (value)
                {
                    timer1.Start();
                }
                else{
                    TitleLabel.Visible = false;
                    MessageAlertLabel.Visible = false;
                }
            }
        }

        //
        // Resumen:
        //     Intervalo de tiempo 
        //     controles secundarios.
        //
        // Devuelve:
        //     true si se muestran el control y todos sus controles secundarios; en caso contrario,
        //     false. De manera predeterminada, es true.
        [DefaultValue(10)]
        [Localizable(true)]
        public int Interval {

            get { return _Interval; }
            set { _Interval = value; }
        }

        //
        // Resumen:
        //     Cantidad de Tick
        //     controles secundarios.
        //
        // Devuelve:
        //     true si se muestran el control y todos sus controles secundarios; en caso contrario,
        //     false. De manera predeterminada, es true.
        [DefaultValue(50)]
        [Localizable(true)]
        public int FrameCount {

            get { return _FramCount; }
            set { _FramCount = value; }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            TitleLabel.Visible = !TitleLabel.Visible;
            MessageAlertLabel.Visible = !MessageAlertLabel.Visible;
          
            currentInterval += 1;
            if (currentInterval == FrameCount)
            {
                TitleLabel.Visible =true;
                MessageAlertLabel.Visible =true;
                timer1.Interval = _Interval;
                currentInterval = 0;
                timer1.Stop();
            }
            else {
                timer1.Interval = _Interval * currentInterval;
            }
          
        }
    }
}
