using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Accord.Vision.Detection;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;

namespace AgentSensorFaceLib
{
    public class Sensor
    {
        #region EVENTS

        #region FACE_DETECTED
        public delegate void FaceDetected(object sender, Face[] args);

        public event FaceDetected OnFaceDetected;

        private void updateFaceDetected(Rectangle[] rects)
        {
            if (OnFaceDetected != null)
            {               
                OnFaceDetected(this, rects.ToFaces());
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

        #region MOTION
        public delegate void MotionDetected(object sender, bool IsMotion);
        public event MotionDetected OnMotionDetected;
        private void updateMotion(bool isMotion)
        {
            if(OnMotionDetected != null)
            {
                OnMotionDetected(this, isMotion);
            }
        }
        #endregion
        #endregion

        #region FIELDS_AND_PROPS
        private bool isLocalCamera = true;
        private int localCameraIndex = 0;
        private IVideoSource video = null;
        private Bitmap bmp = null;
        private String cameraUrl;
        private String cameraLogin;
        private String cameraPassword;
        private MotionDetector motion;
        private MotionAreaHighlighting motionMarker;
        private Accord.Vision.Detection.HaarObjectDetector detector = null;
        private Accord.Vision.Tracking.Camshift tracker = null;
        private Accord.Imaging.Filters.RectanglesMarker marker = new Accord.Imaging.Filters.RectanglesMarker(Color.Fuchsia);
        private bool detecting = false;
        private bool tracking = false;
        private float scaleX;
        private float scaleY;
        private static int processWidth = 160;
        private static int processHeight = 120;
        private Rectangle previous;
        private Rectangle current;
        #endregion

        #region CONSTRUCTORS

        public Sensor()
        {
            this.localCameraIndex = 0;
            this.isLocalCamera = true;
            this.init();
        }

        public Sensor(int localCameraIndex)
        {
            this.localCameraIndex = localCameraIndex;
            this.isLocalCamera = true;
            this.init();
        }

        public Sensor(String url, String login = "", String password = "")
        {
            this.isLocalCamera = false;
            this.cameraUrl = url;
            this.cameraLogin = "";
            this.cameraPassword = "";
            this.init();
        }

        public void WakeUp()
        {
            try
            {
                video.Start();
            }
            catch (Exception ex)
            {
                updateError("Brak połączenia z kamerą");
            }
        }

        public void GoSleep()
        {
            try
            {
                 video.SignalToStop();
            }
            catch (Exception ex)
            {
                updateError("Brak połączenia z kamerą");
            }
        }

        private void init()
        {
            try
            {
                if (isLocalCamera)
                {
                    var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    video = new VideoCaptureDevice(videoDevices[localCameraIndex].MonikerString);
                }
                else
                {
                    video = new MJPEGStream(this.cameraUrl);
                    ((MJPEGStream)video).Login = this.cameraLogin;
                    ((MJPEGStream)video).Password = this.cameraPassword;
                }
                video.NewFrame += new NewFrameEventHandler(processFrame);
                video.VideoSourceError += new VideoSourceErrorEventHandler(processFrameError);

                motionMarker = new MotionAreaHighlighting();
                
                motion = new MotionDetector(
                    new SimpleBackgroundModelingDetector( ),
                    motionMarker
                );

               detector = new HaarObjectDetector(
               HaarCascade.FromXml(new StringReader(Properties.Resources.haarcascade_frontalface_default)));
               detector.MinSize = new Size(10, 10);
               detector.ScalingFactor = 1.2f;
               detector.ScalingMode = ObjectDetectorScalingMode.SmallerToGreater;
               detector.SearchMode = ObjectDetectorSearchMode.Single;  

            }
            catch (Exception ex)
            {
                updateError(ex.Message);
            }
        }

        private void processFrameError(object sender, VideoSourceErrorEventArgs eventArgs)
        {
            updateError(eventArgs.Description);
        }

        private void processFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Rectangle rect = new Rectangle(0, 0, eventArgs.Frame.Width, eventArgs.Frame.Height);
            PixelFormat pf = eventArgs.Frame.PixelFormat;
            System.Drawing.Imaging.ImageLockMode access = ImageLockMode.ReadWrite;

            Bitmap frame = eventArgs.Frame.Clone(rect, pf);
            scaleX = frame.Width / processWidth;
            scaleY = frame.Height / processHeight;
            Bitmap frameFace = eventArgs.Frame.Clone(rect, pf);

            

            if (OnMotionDetected != null)
            {
                BitmapData dataMotion = frame.LockBits(rect, access, pf);
                UnmanagedImage frameUI = new UnmanagedImage(dataMotion);
                if (motion.ProcessFrame(frameUI) > 0.15)
                {
                    updateMotion(true);
                }
                else
                {
                    updateMotion(false);
                }
                frame.UnlockBits(dataMotion);
            }

            if (OnFaceDetected != null)
            {
                BitmapData dataFace = frameFace.LockBits(rect, access, pf);
                var faceUI = new UnmanagedImage(dataFace);
                var downsample = faceUI.ResizeTo(processWidth, processHeight);
                var detections = detector.ProcessFrame(downsample);
                frameFace.UnlockBits(dataFace);

                if (detections.Length > 0)
                {
                    updateFaceDetected(detector.DetectedObjects);

                    BitmapData dataMain = frame.LockBits(rect, access, pf);
                    var ui = new UnmanagedImage(dataMain);
                    marker = new Accord.Imaging.Filters.RectanglesMarker(detector.DetectedObjects.Scale(scaleX, scaleY));
                    marker.MarkerColor = Color.Yellow;                    
                    frame.UnlockBits(dataMain);

                    frame = marker.Apply(frame);
                    
                }                
            }

            
            updateFrameReceived(frame);
            

        }
        #endregion
    }
}
