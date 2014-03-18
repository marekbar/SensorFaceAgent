using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AgentSensorFaceLib
{
    public class Frame : EventArgs
    {
        public Bitmap Image;
        public int Width { get { return Image.Width; } }
        public int Height { get { return Image.Height; } }
        public Frame(Bitmap bmp)
        {
            Image = bmp;
        }
    }
}
