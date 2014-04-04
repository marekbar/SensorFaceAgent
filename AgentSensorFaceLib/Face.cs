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
        /// Face image as Bitmap
        /// </summary>
        public Bitmap FaceImage = null;

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

        public bool Save(System.Environment.SpecialFolder where, System.String name = "")
        {
            try
            {
                var path = System.Environment.GetFolderPath(where) + "\\" + (name == "" ? System.DateTime.Now.ToString().Replace('-','_').Replace(':','_').Replace(' ','_') : name) + ".bmp";
                FaceImage.Save(path);
                return true;
            }
            catch(System.Exception)
            {
                return false;
            }
        }
    }
}
