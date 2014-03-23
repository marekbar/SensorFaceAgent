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
            sensor.OnEyesDetected += new Sensor.EyesDetected(EyeDetectSense);
           // sensor.OnMotionDetected += new Sensor.MotionDetected(MotionSense);
            sensor.WakeUp();
        }

        #region AGENT_SENSES
        private void ViewSense(object sender, Frame frame)
        {
            setImage((Bitmap)frame.Image.Clone());
        }

        private void MotionSense(object sender, bool isMotion)
        {
            if (isMotion)
            {
                SetText("Ruch");
            }
            else
            {
                SetText("Brak ruchu");
            }
        }

        private void FaceDetectSense(object sender, Face[] faces)
        {
            if(faces.Length > 0) 
                SetText("Wykryto: " + faces.Length.ToString() + " twarzy", 2);
            else
                SetText("Brak twarzy",2);
        }

        private void EyeDetectSense(object sender, Eye[] eyes)
        {
            if (eyes.Length > 0)
                SetText("Wykryto: " + eyes.Length.ToString() + " oczu", 2);
            else
                SetText("Brak wykryć oczu", 2);
        }

        private void DisruptionSense(object sender, SensorError error)
        {
            SetText(error.ErrorMessage);
        }

        private void SetText(string text, int id = 1)
        {
            try
            {
                switch (id)
                {
                    case 1:
                        {
                            if (this.status1.GetCurrentParent().InvokeRequired)
                            {
                                this.status1.GetCurrentParent().Invoke(new MethodInvoker(delegate { status1.Text = text; }));
                            }
                            else
                            {
                                status1.Text = text;
                            }
                        }break;
                    case 2:
                        {
                            if (this.status2.GetCurrentParent().InvokeRequired)
                            {
                                this.status2.GetCurrentParent().Invoke(new MethodInvoker(delegate { status2.Text = text; }));
                            }
                            else
                            {
                                status2.Text = text;
                            }
                        } break;
                    case 3:
                        {
                            if (this.status3.GetCurrentParent().InvokeRequired)
                            {
                                this.status3.GetCurrentParent().Invoke(new MethodInvoker(delegate { status3.Text = text; }));
                            }
                            else
                            {
                                status3.Text = text;
                            }
                        } break;

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
