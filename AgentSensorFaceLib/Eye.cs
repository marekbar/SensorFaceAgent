using System.Drawing;

namespace AgentSensorFaceLib
{
    public class Eye
    {
        /// <summary>
        /// Eye X position
        /// </summary>
        public int PositionX
        {
            get
            {
                return Left + Width / 2;
            }
        }

        /// <summary>
        /// Eye Y position
        /// </summary>
        public int PositionY
        {
            get
            {
                return Top + Height / 2;
            }
        }

        /// <summary>
        /// Eye Rectangle start X position
        /// </summary>
        public int Left;

        /// <summary>
        /// Eye Rectangle start Y position
        /// </summary>
        public int Top;

        /// <summary>
        /// Eye Rectangle width
        /// </summary>
        public int Width;

        /// <summary>
        /// Eye Rectangle height
        /// </summary>
        public int Height;

        /// <summary>
        /// Eye default constructor
        /// </summary>
        public Eye() { }

        /// <summary>
        /// Secondary eye constructor
        /// </summary>
        /// <param name="rect">Rectangle - eye bounds</param>
        public Eye(Rectangle rect)
        {
            Left = rect.X;
            Top = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }
    }
}
