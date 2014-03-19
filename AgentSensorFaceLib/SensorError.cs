using System;

namespace AgentSensorFaceLib
{
    /// <summary>
    /// Sensor error
    /// </summary>
    public class SensorError : EventArgs
    {
        /// <summary>
        /// Error description
        /// </summary>
        public String ErrorMessage;

        /// <summary>
        /// Error original exception
        /// </summary>
        public Exception ErrorException;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="error">String - error description</param>
        /// <param name="ex">Exception - original exception</param>
        public SensorError(String error, Exception ex = null)
        {
            ErrorMessage = error;
            ErrorException = ex;
        }
    }
}
