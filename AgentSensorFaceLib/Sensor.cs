using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;

namespace AgentSensorFaceLib
{
    /// <summary>
    /// Agent sensor
    /// </summary>
    public class Sensor
    {
        #region EVENTS

        #region FACE_DETECTED
        /// <summary>
        /// Face detection event pattern
        /// </summary>
        /// <param name="sender">object - owner</param>
        /// <param name="args">Face[] - detected faces</param>
        public delegate void FaceDetected(object sender, Face[] args);

        /// <summary>
        /// Face detected event - use FaceDetected delagate
        /// </summary>
        public event FaceDetected OnFaceDetected;

        /// <summary>
        /// Raises event for face detected
        /// </summary>
        /// <param name="rects">Face[] - detected faces</param>
        private void updateFaceDetected(Face[] faces)
        {
            if (OnFaceDetected != null)
            {               
                OnFaceDetected(this, faces);
            }
        }
        #endregion

        #region FRAME_RECEIVED
        /// <summary>
        /// Frame from camera received - event pattern
        /// </summary>
        /// <param name="sender">object - owner</param>
        /// <param name="frame">Frame - contains image from camera</param>
        public delegate void FrameReceived(object sender, Frame frame);

        /// <summary>
        /// Frame received from camera event
        /// </summary>
        public event FrameReceived OnFrameReceived;

        /// <summary>
        /// Pass frame from camera to event
        /// </summary>
        /// <param name="bmp">Bitmap - frame</param>
        private void updateFrameReceived(Bitmap bmp)
        {
            if (OnFrameReceived != null)
            {
                OnFrameReceived(this, new Frame(bmp));
            }
        }
        #endregion

        #region ERROR_OCCURED
        /// <summary>
        /// Error delegate pattern
        /// </summary>
        /// <param name="sender">object -owner</param>
        /// <param name="error">SensorError - error details</param>
        public delegate void ErrorOccured(object sender, SensorError error);

        /// <summary>
        /// Error in sensor event
        /// </summary>
        public event ErrorOccured OnSensorError;

        /// <summary>
        /// Updates error event
        /// </summary>
        /// <param name="error">String - error message</param>
        /// <param name="ex">Exception - details</param>
        private void updateError(String error, Exception ex = null)
        {
            if (OnSensorError != null)
            {
                OnSensorError(this, new SensorError(error, ex));
            }
        }
        #endregion

        #region MOTION
        /// <summary>
        /// Motion delegate - pattern for motion event
        /// </summary>
        /// <param name="sender">object - owner</param>
        /// <param name="IsMotion">bool - is motion</param>
        public delegate void MotionDetected(object sender, bool IsMotion);

        /// <summary>
        /// Motion event - if set then motion processing is executed and marked on image
        /// </summary>
        public event MotionDetected OnMotionDetected;

        /// <summary>
        /// Updates motion event
        /// </summary>
        /// <param name="isMotion">bool - determinates fact about motion presence</param>
        private void updateMotion(bool isMotion)
        {
            if(OnMotionDetected != null)
            {
                OnMotionDetected(this, isMotion);
            }
        }
        #endregion

        #region EYES_DETECTED
        /// <summary>
        /// Eyes detection delegate - pattern for event method
        /// </summary>
        /// <param name="sender">object - owner</param>
        /// <param name="eyes">Eyes[] - detected eyes, single, like left in face 1, right in face 2</param>
        public delegate void EyesDetected(object sender, Eye[] eyes);

        /// <summary>
        /// Eyes detection event
        /// </summary>
        public event EyesDetected OnEyesDetected;

        /// <summary>
        /// Update eyes detection event with new data
        /// </summary>
        /// <param name="rects">Rectangle[] - eyes set</param>
        private void updateEyeDetected(Rectangle[] rects)
        {
            if (OnEyesDetected != null)
            {
                OnEyesDetected(this, rects.ToEyes());
            }
        }
        #endregion

        #endregion

        #region FIELDS_AND_PROPS
        /// <summary>
        /// Defines if sensor uses directly connected camera or camera over IP protocol
        /// </summary>
        private bool isLocalCamera = true;

        /// <summary>
        /// Localy selected camera index, 0 means default and first in the list
        /// </summary>
        private int localCameraIndex = 0;

        /// <summary>
        /// Camera connection and image provider
        /// </summary>
        private IVideoSource video = null;

        /// <summary>
        /// Bitmap - used for processing purposes
        /// </summary>
        private Bitmap bmp = null;

        /// <summary>
        /// IP camera url address, like: http://192.168.1.5:80/mjpegvideo.cgi
        /// </summary>
        private String cameraUrl;

        /// <summary>
        /// IP camera login, if any
        /// </summary>
        private String cameraLogin;

        /// <summary>
        /// IP camera password, if any
        /// </summary>
        private String cameraPassword;

        /// <summary>
        /// Motion detector
        /// </summary>
        private MotionDetector motion;

        /// <summary>
        /// Motion marker
        /// </summary>
        private MotionAreaHighlighting motionMarker;

        /// <summary>
        /// Face detector
        /// </summary>
        private Accord.Vision.Detection.HaarObjectDetector detectorFace = null;

        /// <summary>
        /// Eye detector
        /// </summary>
        private Accord.Vision.Detection.HaarObjectDetector detectorEye = null;

        /// <summary>
        /// Object tracker - needs startup coordinates
        /// </summary>
        private Accord.Vision.Tracking.Camshift tracker = null;

        /// <summary>
        /// Rectangle shape marker
        /// </summary>
        private Accord.Imaging.Filters.RectanglesMarker marker = new Accord.Imaging.Filters.RectanglesMarker(Color.Fuchsia);

        /// <summary>
        /// Detecting mode flag
        /// </summary>
        private bool detecting = false;

        /// <summary>
        /// Tracking mode flag
        /// </summary>
        private bool tracking = false;

        /// <summary>
        /// Image scale factor for X
        /// </summary>
        private float scaleX;

        /// <summary>
        /// Image scale factor for Y
        /// </summary>
        private float scaleY;

        /// <summary>
        /// Face process frame width
        /// </summary>
        private static int processWidth = 160;

        /// <summary>
        /// Face process frame height
        /// </summary>
        private static int processHeight = 120;

        /// <summary>
        /// Previous rectangle area
        /// </summary>
        private Rectangle previous;

        /// <summary>
        /// Current rectangle area
        /// </summary>
        private Rectangle current;

        private bool isPreview
        {
            get
            {
                return OnFrameReceived != null;
            }
        }
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Default constructor - connects with first local camera
        /// </summary>
        public Sensor()
        {
            this.localCameraIndex = 0;
            this.isLocalCamera = true;
            this.init();
        }

        /// <summary>
        /// Second constructor - connects with specified local camera
        /// </summary>
        /// <param name="localCameraIndex">int - local camera index</param>
        public Sensor(int localCameraIndex)
        {
            this.localCameraIndex = localCameraIndex;
            this.isLocalCamera = true;
            this.init();
        }

        /// <summary>
        /// Third constructor - used to connectwith IP camera
        /// </summary>
        /// <param name="url">String - IP camera url</param>
        /// <param name="login">String - IP camera login, if any</param>
        /// <param name="password">String - IP camera password, if any</param>
        public Sensor(String url, String login = "", String password = "")
        {
            this.isLocalCamera = false;
            this.cameraUrl = url;
            this.cameraLogin = "";
            this.cameraPassword = "";
            this.init();
        }
        #endregion

        #region PUBLIC
        /// <summary>
        /// Run sensor
        /// </summary>
        public void WakeUp()
        {
            try
            {
                video.Start();
            }
            catch (Exception)
            {
                updateError("Brak połączenia z kamerą");
            }
        }

        /// <summary>
        /// Stop sensor
        /// </summary>
        public void GoSleep()
        {
            try
            {
                 video.SignalToStop();
            }
            catch (Exception)
            {
                updateError("Brak połączenia z kamerą");
            }
        }

        #endregion

        #region PRIVATE

        /// <summary>
        /// Initialize sensor
        /// </summary>
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
                
                //works
                detectorFace = Detector.Create(DetectorType.Face);

                //crap
                detectorEye = Detector.Create(DetectorType.Eye);
            }
            catch (Exception ex)
            {
                updateError(ex.Message);
            }
        }

        /// <summary>
        /// Camera errors sensor
        /// </summary>
        /// <param name="sender">object - owner</param>
        /// <param name="eventArgs">VideoSourceErrorEventArgs - args</param>
        private void processFrameError(object sender, VideoSourceErrorEventArgs eventArgs)
        {
            updateError(eventArgs.Description);
        }

        /// <summary>
        /// Image processor and the heart of sensor
        /// </summary>
        /// <param name="sender">object - owner</param>
        /// <param name="eventArgs">NewFrameEventArgs - args, contains frame from camera</param>
        private void processFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Rectangle rect = eventArgs.Frame.Bounds();
            PixelFormat pf = eventArgs.Frame.PixelFormat;

            Bitmap frame = eventArgs.Frame.Clone(rect, pf);
            scaleX = frame.Width / processWidth;
            scaleY = frame.Height / processHeight;
            Bitmap frameFace = eventArgs.Frame.Clone(rect, pf);            

            if (OnMotionDetected != null)
            {
                var dataMotion = frame.GetDirectAccess();
                var frameUI = dataMotion.GetUnmanaged();
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
                var dataFace = frameFace.GetDirectAccess();
                var faceUI = dataFace.GetUnmanaged();
                var downsample = faceUI.ResizeTo(processWidth, processHeight);
                var detections = detectorFace.ProcessFrame(downsample);
                

                if (detections.Length > 0)
                {                   
                    if (isPreview)
                    {
                        marker = new Accord.Imaging.Filters.RectanglesMarker(detectorFace.DetectedObjects.Scale(scaleX, scaleY));
                        marker.MarkerColor = Color.Yellow;
                        frame = marker.Apply(frame);
                    }

                    detectorEye.ProcessFrame(downsample);
                    
                    if (detectorEye.DetectedObjects.Length > 0)
                    {
                        if (isPreview)
                        {
                            marker = new Accord.Imaging.Filters.RectanglesMarker(detectorFace.DetectedObjects.Scale(scaleX, scaleY));
                            marker.MarkerColor = Color.Orange;
                            frame = marker.Apply(frame);
                        }
                    }
                    
                }

                frameFace.UnlockBits(dataFace);

                if (detectorFace.DetectedObjects != null && detectorFace.DetectedObjects.Length > 0)
                {
                    var faces = detectorFace.DetectedObjects.ToFaces();
                    for (int i = 0; i < faces.Length; i++)
                    {
                        var cutter = new AForge.Imaging.Filters.Crop(new Rectangle(
                                                             (int)(faces[i].Left * scaleX),
                                                             (int)(faces[i].Top * scaleY),
                                                             (int)(faces[i].Width * scaleX),
                                                             (int)(faces[i].Height * scaleY)
                                                             ));
                        faces[i].FaceImage = cutter.Apply(frameFace);
                    }
                    updateFaceDetected(faces);
                }

            }

            
            updateFrameReceived(frame);
            

        }
        #endregion
    }
}
