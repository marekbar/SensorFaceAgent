using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgentSensorFaceLib;

namespace AgentSensorFace
{
    public partial class Form1 : Form
    {
        private Sensor sensor = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sensor = new Sensor();
            sensor.OnFrameReceived += new Sensor.FrameReceived(ViewSense);
            sensor.OnFaceDetected += new Sensor.FaceDetected(FaceDetectSense);
            sensor.OnSensorError += new Sensor.ErrorOccured(DisruptionSense);
        }

        #region AGENT_SENSES
        private void ViewSense(object sender, Frame frame)
        {
        }

        private void FaceDetectSense(object sender, Face face)
        {
        }

        private void DisruptionSense(object sender, SensorError error)
        {
        }
        #endregion
    }
}
