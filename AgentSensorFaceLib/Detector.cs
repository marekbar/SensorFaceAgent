namespace AgentSensorFaceLib
{
    /// <summary>
    /// Detection types
    /// </summary>
    public enum DetectorType
    {
        /// <summary>
        /// Face detection
        /// </summary>
        Face = 1,

        /// <summary>
        /// Eye detection
        /// </summary>
        Eye = 2,

        /// <summary>
        /// Nose detection
        /// </summary>
        Nose = 3,

        /// <summary>
        /// Mouth detection
        /// </summary>
        Mouth = 4,

        /// <summary>
        /// Left ear detection
        /// </summary>
        EarLeft = 5,

        /// <summary>
        /// Right ear detection
        /// </summary>
        EarRight = 6,

        FaceProfile = 7,
    }
    
    public static class Detector
    {
        public static Accord.Vision.Detection.HaarObjectDetector Face()
        {
            return Create(DetectorType.Face);
        }

        public static Accord.Vision.Detection.HaarObjectDetector FaceProfile()
        {
            return Create(DetectorType.FaceProfile);
        }

        public static Accord.Vision.Detection.HaarObjectDetector Eye()
        {
            return Create(DetectorType.Eye);
        }
        public static Accord.Vision.Detection.HaarObjectDetector Nose()
        {
            return Create(DetectorType.Nose);
        }
        public static Accord.Vision.Detection.HaarObjectDetector Mouth()
        {
            return Create(DetectorType.Mouth);
        }
        public static Accord.Vision.Detection.HaarObjectDetector EarLeft()
        {
            return Create(DetectorType.EarLeft);
        }
        public static Accord.Vision.Detection.HaarObjectDetector EarRight()
        {
            return Create(DetectorType.EarRight);
        }

        private static Accord.Vision.Detection.HaarObjectDetector Create(DetectorType type)
        {
            Accord.Vision.Detection.HaarCascade cascade = null;
            float scalingFactor = 1.2f;
            string data = "";
            switch (type)
            {
                case DetectorType.Face:
                    {
                       data = Properties.Resources.haarcascade_frontalface_default;
                       scalingFactor = 1.2f;
                    }break;

                case DetectorType.Eye:
                    {
                        data = Properties.Resources.haarcascade_eye;
                        scalingFactor = 1.1f;
                    } break;

                case DetectorType.Nose:
                    {
                        data = Properties.Resources.haarcascade_mcs_nose;                         
                        scalingFactor = 1.2f;
                    }break;

                case DetectorType.Mouth:
                    {
                        data = Properties.Resources.haarcascade_mcs_mouth;  
                        scalingFactor = 1.05f;
                    } break;

                case DetectorType.EarLeft:
                    {
                        data = Properties.Resources.haarcascade_mcs_leftear;
                        scalingFactor = 1.15f;
                    } break;

                case DetectorType.EarRight:
                    {
                        data = Properties.Resources.haarcascade_mcs_rightear; 
                        scalingFactor = 1.15f;
                    } break;
                case DetectorType.FaceProfile:
                    {
                        data = Properties.Resources.haarcascade_profileface;
                        scalingFactor = 1.2f;
                    } break;
            }

            cascade = Accord.Vision.Detection.HaarCascade.FromXml(new System.IO.StringReader(data));
            var detector = new Accord.Vision.Detection.HaarObjectDetector(cascade);
            detector.MinSize = new System.Drawing.Size(10, 10);
            detector.ScalingFactor = scalingFactor;
            detector.ScalingMode = Accord.Vision.Detection.ObjectDetectorScalingMode.SmallerToGreater;
            detector.SearchMode = Accord.Vision.Detection.ObjectDetectorSearchMode.Single;
            detector.UseParallelProcessing = true;
            return detector;
        }
    }
}
