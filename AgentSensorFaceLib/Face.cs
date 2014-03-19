using System.Drawing;

namespace AgentSensorFaceLib
{
    /// <summary>
    /// Face description object
    /// </summary>
    public class Face
    {
        /// <summary>
        /// Face X position
        /// </summary>
        public int PositionX
        {
            get
            {
                return Left + Width / 2;
            }
        }

        /// <summary>
        /// Face Y position
        /// </summary>
        public int PositionY
        {
            get
            {
                return Top + Height / 2;
            }
        }

        /// <summary>
        /// Face Rectangle start X position
        /// </summary>
        public int Left;

        /// <summary>
        /// Face Rectangle start Y position
        /// </summary>
        public int Top;

        /// <summary>
        /// Face Rectangle width
        /// </summary>
        public int Width;

        /// <summary>
        /// Face Rectangle height
        /// </summary>
        public int Height;
        
        /// <summary>
        /// Face default constructor
        /// </summary>
        public Face() { }

        /// <summary>
        /// Face second constructor
        /// </summary>
        /// <param name="rect">Rectangle - initial rect</param>
        public Face(Rectangle rect)
        {
            Left = rect.X;
            Top = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }
    }
}
