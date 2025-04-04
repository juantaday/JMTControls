﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JMTControls.NetCore.Componets
{
    
        public class ElipseComponent : Component
        {

            [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
            private static extern IntPtr CreateRoundRectRgn
               (
                  int nLeftRect,
                  int nTopRect,
                  int nRightRect,
                  int nBottomRect,
                  int nWidthEllipse,
                  int nHeightEllipse
               );

            [DllImport("Gdi32.dll", EntryPoint = "DeleteObject")]
            public static extern bool DeleteObject(IntPtr hObject);


            private Control _control;
            private int _CornerRadius = 30;

            public Control TargetControl
            {
                get { return _control; }
                set
                {
                    _control = value;


                    _control.SizeChanged += (sender, eventArgs) =>
                    {
                        CreateRoundRect();
                    };
                }
            }

            private void CreateRoundRect()
            {
                if (_control != null)
                {
                    IntPtr handle = CreateRoundRectRgn(0, 0, _control.Width, _control.Height, _CornerRadius, _CornerRadius);
                    _control.Region = Region.FromHrgn(handle);
                    DeleteObject(handle);
                }

            }

            public int CornerRadius
        {
                get { return _CornerRadius; }
                set
                {
                    _CornerRadius = value;
                    CreateRoundRect();
                }
            }
        }
    
}
