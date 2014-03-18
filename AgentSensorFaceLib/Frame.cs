using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AgentSensorFaceLib
{
    public class Frame : EventArgs
    {
        public Bitmap Frame;
        public int Width { get { return Frame.Width; } }
        public int Height { get { return Frame.Height; } }
        public Frame(Bitmap bmp)
        {
            Frame = bmp;
        }
    }
}
