namespace AgentSensorFaceLib
{
    public enum DetectorType
    {
        Face = 1,
        Eye = 2
    }

    public static class Detector
    {
        public static Accord.Vision.Detection.HaarObjectDetector Create(DetectorType type)
        {
            Accord.Vision.Detection.HaarCascade cascade = null;
            float scalingFactor = 1.2f;
            switch (type)
            {
                case DetectorType.Face:
                    {
                        cascade = Accord.Vision.Detection.HaarCascade.FromXml(
                            new System.IO.StringReader(Properties.Resources.haarcascade_frontalface_default));
                        scalingFactor = 1.2f;
                    }break;
                case DetectorType.Eye:
                    {
                        cascade = Accord.Vision.Detection.HaarCascade.FromXml(
                            new System.IO.StringReader(Properties.Resources.haarcascade_eye));
                        scalingFactor = 1.1f;
                    } break;
            }
            
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
