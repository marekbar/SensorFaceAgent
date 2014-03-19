namespace AgentSensorFaceLib
{
    /// <summary>
    /// Position XY
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="x">int - X</param>
        /// <param name="y">int - Y</param>
        public Position(int x, int y)
        {
            X = x; Y = y;
        }

        /// <summary>
        /// X position
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y position
        /// </summary>
        public int Y { get; set; }
    }
}
