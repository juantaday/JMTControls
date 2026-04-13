using JMTControls.NetCore.Controls;
using System;

namespace JMTControls.NetCore.Events
{
    public class SidebarItemClickEventArgs : EventArgs
    {
        public SidebarItemModel Item { get; }
        public SidebarGroupModel Group { get; }
        public SidebarTab Tab { get; }

        public SidebarItemClickEventArgs(SidebarItemModel item, SidebarGroupModel group, SidebarTab tab)
        {
            Item = item;
            Group = group;
            Tab = tab;
        }
    }
}