using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using AForge.Imaging;

namespace AgentSensorFaceLib
{
    /// <summary>
    /// Useful extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets full error
        /// </summary>
        /// <param name="ex">Exception - error ex</param>
        /// <returns>String - error message</returns>
        public static String GetError(this Exception ex)
        {
            String extra = "";

            return extra + " " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "");
        }

        /// <summary>
        /// Get the largest rectangle from set
        /// </summary>
        /// <param name="rects">Rectangle[] - set of rectangles</param>
        /// <returns>Rectangle - the biggest</returns>
        public static Rectangle GetBiggest(this Rectangle[] rects)
        {
            return rects.Aggregate((r1, r2) => (r1.Height * r1.Width) > (r2.Height * r2.Width) ? r1 : r2);
        }

        /// <summary>
        /// Gets bitmap area rectangle
        /// </summary>
        /// <param name="bmp">Bitmap - image</param>
        /// <returns>Rectangle - image bounds</returns>
        public static Rectangle Bounds(this Bitmap bmp)
        {
            return new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        /// <summary>
        /// Get image data direct access/raw access
        /// </summary>
        /// <param name="bmp">Bitmap - image</param>
        /// <returns>BitmapData</returns>
        public static BitmapData GetDirectAccess(this Bitmap bmp)
        {
            return bmp.LockBits(bmp.Bounds(), ImageLockMode.ReadWrite, bmp.PixelFormat);
        }

        /// <summary>
        /// Scale rectangle by factor
        /// </summary>
        /// <param name="rect">Rectangle - original</param>
        /// <param name="scale">int - scale</param>
        /// <returns>Rectangle - rescaled rectangle</returns>
        public static Rectangle MulBy(this Rectangle rect, int scale)
        {
            rect.X *= scale;
            rect.Y *= scale;
            rect.Width *= scale;
            rect.Height *= scale;
            return rect;
        }

        /// <summary>
        /// Apply rectangle marker
        /// </summary>
        /// <param name="marker">Accord.Imaging.Filters.RectanglesMarker</param>
        /// <param name="um">AForge.Imaging.UnmanagedImage</param>
        /// <param name="rect">Rectangle</param>
        public static void Set(this Accord.Imaging.Filters.RectanglesMarker marker, ref AForge.Imaging.UnmanagedImage um, Rectangle rect)
        {
            marker.Rectangles = new Rectangle[] { rect };
            marker.ApplyInPlace(um);
        }

        /// <summary>
        /// Convert REctangle to Face
        /// </summary>
        /// <param name="rect">Rectangle</param>
        /// <returns>Face</returns>
        public static Face ToFace(this Rectangle rect)
        {
            var f = new Face();
            f.Left = rect.X;
            f.Top = rect.Y;
            f.Width = rect.Width;
            f.Height = rect.Height;
            return f;
        }

        /// <summary>
        /// Convert Rectangle to Eye
        /// </summary>
        /// <param name="rect">Rectangle</param>
        /// <returns>Eye</returns>
        public static Eye ToEye(this Rectangle rect)
        {
            var e = new Eye();
            e.Left = rect.X;
            e.Top = rect.Y;
            e.Width = rect.Width;
            e.Height = rect.Height;
            return e;
        }

        /// <summary>
        /// Convert Rectangle[] to Face[]
        /// </summary>
        /// <param name="rects">Rectangle[]</param>
        /// <returns>Face[]</returns>
        public static Face[] ToFaces(this Rectangle[] rects)
        {
            List<Face> faces = new List<Face>();
            foreach (var rect in rects)
            {
                faces.Add(rect.ToFace());
            }
            return faces.ToArray();
        }

        /// <summary>
        /// Convert Rectangle[] to Eye[]
        /// </summary>
        /// <param name="rects">Rectangle[]</param>
        /// <returns>Eye[]</returns>
        public static Eye[] ToEyes(this Rectangle[] rects)
        {
            List<Eye> eyes = new List<Eye>();
            foreach (var rect in rects)
            {
                eyes.Add(rect.ToEye());
            }
            return eyes.ToArray();
        }

        /// <summary>
        /// Get first Rectangle from set
        /// </summary>
        /// <param name="rects">Rectangle[]</param>
        /// <returns>Rectangle</returns>
        public static Rectangle First(this Rectangle[] rects)
        {
            return rects[0];
        }

        /// <summary>
        /// Resize raw image to specified size
        /// </summary>
        /// <param name="ui">UnmanagedImage - raw image</param>
        /// <param name="w">int - new width</param>
        /// <param name="h">int - new height</param>
        /// <returns>UnmanagedImage - resized</returns>
        public static UnmanagedImage ResizeTo(this UnmanagedImage ui, int w, int h)
        {
            AForge.Imaging.Filters.ResizeNearestNeighbor resize = new AForge.Imaging.Filters.ResizeNearestNeighbor(w, h);
            return resize.Apply(ui);
        }

        /// <summary>
        /// Draws horizontal axis
        /// </summary>
        /// <param name="im">UnmanagedImage</param>
        /// <param name="tracker">Accord.Vision.Tracking.Camshift - by reference</param>
        public static void DrawHorizontalAxis(this UnmanagedImage im, ref Accord.Vision.Tracking.Camshift tracker)
        {
            AForge.Math.Geometry.LineSegment axis = tracker.TrackingObject.GetAxis();
            Drawing.Line(im, axis.Start.Round(), axis.End.Round(), Color.Red);
        }

        /// <summary>
        /// Gets unmanaged
        /// </summary>
        /// <param name="bd">BitmapData</param>
        /// <returns>UnmanagedImage</returns>
        public static UnmanagedImage GetUnmanaged(this BitmapData bd)
        {
            return new UnmanagedImage(bd);
        }

        /// <summary>
        /// Scale Rectangle set
        /// </summary>
        /// <param name="rects">Rectangle[]</param>
        /// <param name="scaleX">float - x scale factor</param>
        /// <param name="scaleY">float - y scale factor</param>
        /// <returns>Rectangle[]</returns>
        public static Rectangle[] Scale(this Rectangle[] rects, float scaleX, float scaleY)
        {
            List<Rectangle> rl = new List<Rectangle>();
            foreach (var r in rects)
            {
                Rectangle rr = new Rectangle();
                rr.X = (int)(r.X * scaleX);
                rr.Y = (int) (r.Y * scaleY);
                rr.Width = (int) (r.Width * scaleX);
                rr.Height = (int) (r.Height * scaleY);
                rl.Add(rr);
            }
            return rl.ToArray();
        }

        /// <summary>
        /// Gets rectangle start position
        /// </summary>
        /// <param name="rect">Rectangle</param>
        /// <returns>Position</returns>
        public static Position Current(this Rectangle rect)
        {
            return new Position(rect.X, rect.Y);
        }
    }
}
