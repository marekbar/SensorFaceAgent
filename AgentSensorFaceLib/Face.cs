using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AgentSensorFaceLib
{
    public class Face : EventArgs
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Face() { }
        public Face(Rectangle rect)
        {
            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }
    }
}
