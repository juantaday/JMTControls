using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMControls.ExpandCollapsePanel
{ /// <summary>
  /// Информация о развёртывании/свёртывании контрола
  /// </summary>
    public class ExpandCollapseEventArgs : EventArgs
    {
        /// <summary>
        /// true - контрол развёрнут. false - контрол свёрнут
        /// </summary>
        public bool IsExpanded { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="isExpanded">true - контрол развёрнут. false - контрол свёрнут</param>
        public ExpandCollapseEventArgs(bool isExpanded)
        {
            IsExpanded = isExpanded;
        }
    }
}
