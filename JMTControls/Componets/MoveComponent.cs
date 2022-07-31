﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JMControls.Componets
{
    public class MoveComponent : Component
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
       
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        private Control _Control;


        public Control TargetControl
        {
            get { return _Control; }
            set
            {
                _Control = value;

                _Control.MouseDown += (object sender, MouseEventArgs e) => {

                    MoveControl();

                };

            }
        }

        private void MoveControl()
        {
            if (_Control != null)
            {

                ReleaseCapture();
                SendMessage(_Control.Parent.Handle, 0x112, 0xf012, 0);

            //    ReleaseCapture();
            //    SendMessage(_Control.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

    }
}
