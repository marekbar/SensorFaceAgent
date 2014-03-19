using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AgentSensorFaceLib
{
    public class Face : EventArgs
    {
        public int PositionX
        {
            get
            {
                return Left + Width / 2;
            }
        }

        public int PositionY
        {
            get
            {
                return Top + Height / 2;
            }
        }

        public int Left;
        public int Top;
        public int Width;
        public int Height;
        public int ScaleFactor = 1;
        public Face() { }
        public Face(Rectangle rect)
        {
            Left = rect.X;
            Top = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }
    }
}
