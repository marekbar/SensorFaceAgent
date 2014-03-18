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
        delegate void SetTextCallback(string text);
        delegate void SetImageCallback(Bitmap bmp);

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
            sensor.WakeUp();
        }

        #region AGENT_SENSES
        private void ViewSense(object sender, Frame frame)
        {
            setImage((Bitmap)frame.Image.Clone());
        }

        private void FaceDetectSense(object sender, Face face)
        {
        }

        private void DisruptionSense(object sender, SensorError error)
        {
            SetText(error.ErrorMessage);
        }

        private void SetText(string text)
        {
            try
            {
                if (this.status.GetCurrentParent().InvokeRequired)
                {
                    this.status.GetCurrentParent().Invoke(new MethodInvoker(delegate { status.Text = text; }));
                }
                else
                {
                    status.Text = text;
                }
            }
            catch { }
        }

        private void setImage(Bitmap bmp)
        {
            try
            {
                if (this.display.InvokeRequired)
                {
                    SetImageCallback sic = new SetImageCallback(setImage);
                    this.Invoke(sic, new object[] { bmp });
                }
                else
                {
                    this.display.Image = bmp;
                }
            }
            catch { }
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sensor.GoSleep();
        }
    }
}
