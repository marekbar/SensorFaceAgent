using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using AForge.Imaging;

namespace AgentSensorFaceLib
{
    public static class Extensions
    {
        public static String GetError(this Exception ex)
        {
            String extra = "";

            return extra + " " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "");
        }
        public static Rectangle GetBiggest(this Rectangle[] rects)
        {
            return rects.Aggregate((r1, r2) => (r1.Height * r1.Width) > (r2.Height * r2.Width) ? r1 : r2);
        }

        public static Rectangle Bounds(this Bitmap bmp)
        {
            return new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public static BitmapData GetDirectAccess(this Bitmap bmp)
        {
            return bmp.LockBits(bmp.Bounds(), ImageLockMode.ReadWrite, bmp.PixelFormat);
        }

        public static Rectangle MulBy(this Rectangle rect, int scale)
        {
            rect.X *= scale;
            rect.Y *= scale;
            rect.Width *= scale;
            rect.Height *= scale;
            return rect;
        }

        public static void Set(this Accord.Imaging.Filters.RectanglesMarker marker, ref AForge.Imaging.UnmanagedImage um, Rectangle rect)
        {
            marker.Rectangles = new Rectangle[] { rect };
            marker.ApplyInPlace(um);
        }

        public static Face ToFace(this Rectangle rect)
        {
            var f = new Face();
            f.Left = rect.X;
            f.Top = rect.Y;
            f.Width = rect.Width;
            f.Height = rect.Height;
            return f;
        }

        public static Face[] ToFaces(this Rectangle[] rects)
        {
            List<Face> faces = new List<Face>();
            foreach (var rect in rects)
            {
                faces.Add(rect.ToFace());
            }
            return faces.ToArray();
        }

        public static Rectangle First(this Rectangle[] rects)
        {
            return rects[0];
        }

        public static UnmanagedImage ResizeTo(this UnmanagedImage ui, int w, int h)
        {
            AForge.Imaging.Filters.ResizeNearestNeighbor resize = new AForge.Imaging.Filters.ResizeNearestNeighbor(w, h);
            return resize.Apply(ui);
        }

        public static void DrawHorizontalAxis(this UnmanagedImage im, ref Accord.Vision.Tracking.Camshift tracker)
        {
            AForge.Math.Geometry.LineSegment axis = tracker.TrackingObject.GetAxis();
            Drawing.Line(im, axis.Start.Round(), axis.End.Round(), Color.Red);
        }

        public static UnmanagedImage GetUnmanaged(this BitmapData bd)
        {
            return new UnmanagedImage(bd);
        }

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

        public static Position Current(this Rectangle rect)
        {
            return new Position(rect.X, rect.Y);
        }
    }
    public class Position
    {
        public Position(int x, int y)
        {
            X = x; Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
