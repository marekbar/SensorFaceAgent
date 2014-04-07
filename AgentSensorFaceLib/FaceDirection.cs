using System.Drawing;

namespace AgentSensorFaceLib
{
    /// <summary>
    /// Face direction enumeration - depends eye detection
    /// </summary>
    public enum FaceDirection
    {
        /// <summary>
        /// Eyes not detected or eye sensor not activated
        /// </summary>
        NoInfo = 0,

        /// <summary>
        /// Frontal face direction - looks at me
        /// </summary>
        Frontal = 1,

        /// <summary>
        /// Face in left direction
        /// </summary>
        TurnedLeft = 2,

        /// <summary>
        /// Face in right direction
        /// </summary>
        TurnedRight = 3
    }
}
