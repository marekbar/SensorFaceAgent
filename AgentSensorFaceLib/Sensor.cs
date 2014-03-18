using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AgentSensorFaceLib
{
    public class Sensor
    {
        #region EVENTS

        #region FACE_DETECTED
        public delegate void FaceDetected(object sender, Face args);

        public event FaceDetected OnFaceDetected;

        private void updateFaceDetected(Rectangle rect)
        {
            if (OnFaceDetected != null)
            {
                OnFaceDetected(this, new Face(rect));
            }
        }
        #endregion

        #region FRAME_RECEIVED
        public delegate void FrameReceived(object sender, Frame frame);

        public event FrameReceived OnFrameReceived;

        private void updateFrameReceived(Bitmap bmp)
        {
            if (OnFrameReceived != null)
            {
                OnFrameReceived(this, new Frame(bmp));
            }
        }
        #endregion

        #region ERROR_OCCURED
        public delegate void ErrorOccured(object sender, SensorError error);

        public event ErrorOccured OnSensorError;

        private void updateError(String error, Exception ex = null)
        {
            if (OnSensorError != null)
            {
                OnSensorError(this, new SensorError(error, ex));
            }
        }
        #endregion

        #endregion

        #region CONSTRUCTORS

        public Sensor()
        {
        }

        public Sensor(int localCameraIndex)
        {
        }

        public Sensor(String url, String login = "", String password = "")
        {
        }
        #endregion
    }
}
