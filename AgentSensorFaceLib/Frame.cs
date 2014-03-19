using System;
using System.Drawing;

namespace AgentSensorFaceLib
{
    /// <summary>
    /// Frame args
    /// </summary>
    public class Frame : EventArgs
    {
        /// <summary>
        /// Image data
        /// </summary>
        public Bitmap Image;

        /// <summary>
        /// Image width
        /// </summary>
        public int Width { get { return Image.Width; } }

        /// <summary>
        /// Image height
        /// </summary>
        public int Height { get { return Image.Height; } }

        /// <summary>
        /// Image constructor
        /// </summary>
        /// <param name="bmp">Bitmap</param>
        public Frame(Bitmap bmp)
        {
            Image = bmp;
        }
    }
}
