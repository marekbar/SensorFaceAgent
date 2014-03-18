using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AgentSensorFaceLib
{
    public class SensorError : EventArgs
    {
        public String ErrorMessage;
        public Exception ErrorException;

        public SensorError(String error, Exception ex = null)
        {
            ErrorMessage = error;
            ErrorException = ex;
        }
    }
}
