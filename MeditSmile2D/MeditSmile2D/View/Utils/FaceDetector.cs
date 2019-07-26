using DlibDotNet;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Point = OpenCvSharp.Point;

namespace MeditSmile2D.View.Utils
{
    class FaceDetector
    {
        #region class FacePoint

        // inner class
        public class FacePoint
        {
            public List<Point> eye;
            public List<Point> mouse;
            public List<Point> midline;
            public List<Point> nose;

            public int element_capacity = 2;

            public FacePoint()
            {
                eye = new List<Point>();
                mouse = new List<Point>();
                midline = new List<Point>();
                nose = new List<Point>();
            }
        }

        #endregion

        public FacePoint facePoint;
        public BitmapImage faceImg;

        public FaceDetector()
        {
            facePoint = new FacePoint();
        }

        public FaceDetector(string filename)
        {
            facePoint = new FacePoint();
            faceImg = FindFaceDetector(filename);
        }

        Mat origImg;
        private BitmapImage FindFaceDetector(string filename)
        {
            FrontalFaceDetector frontalFaceDetector = Dlib.GetFrontalFaceDetector();
            var shapePredictor = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat");

            origImg = Cv2.ImRead(filename, ImreadModes.AnyColor);
            var copyOrigImg = origImg;

            var arrayImg = Mat2array2D(copyOrigImg);
            var faces = frontalFaceDetector.Operator(arrayImg);
            foreach(var face in faces)
            {
                // Find the landmark points for this face
                var shape = shapePredictor.Detect(arrayImg, face);

                // Draw the landmark points on this image
                // Central point of Eye
                var centralEyeL1 = shape.GetPart((uint)36);
                var centralEyeL2 = shape.GetPart((uint)39);
                Point centralPointOfEyeL;
                centralPointOfEyeL.X = (centralEyeL1.X + centralEyeL2.X) / 2;
                centralPointOfEyeL.Y = (centralEyeL1.Y + centralEyeL2.Y) / 2;
                facePoint.eye.Add(centralPointOfEyeL);

                var centralEyeR1 = shape.GetPart((uint)36);
                var centralEyeR2 = shape.GetPart((uint)39);
                Point centralPointOfEyeR;
                centralPointOfEyeR.X = (centralEyeR1.X + centralEyeR2.X) / 2;
                centralPointOfEyeR.Y = (centralEyeR1.Y + centralEyeR2.Y) / 2;
                facePoint.eye.Add(centralPointOfEyeR);

                // MidPoint of Eyes
                Point midPointOfEyes;
                midPointOfEyes.X = (centralPointOfEyeL.X + centralPointOfEyeR.X) / 2;
                midPointOfEyes.Y = (centralPointOfEyeL.Y + centralPointOfEyeR.Y) / 2;
                facePoint.midline.Add(midPointOfEyes);

                // Mouth
                var mouth1 = shape.GetPart((uint)48);
                Point mouthLeftEndPoint;
                mouthLeftEndPoint.X = mouth1.X;
                mouthLeftEndPoint.Y = mouth1.Y;
                facePoint.mouse.Add(mouthLeftEndPoint);

                var mouth2 = shape.GetPart((uint)48);
                Point mouthRightEndPoint;
                mouthRightEndPoint.X = mouth2.X;
                mouthRightEndPoint.Y = mouth2.Y;
                facePoint.mouse.Add(mouthRightEndPoint);

                // Midline : from midPointOfEyes to midPointOfMouth
                // MidPoint of Mouth
                Point midPointOfMouth;
                midPointOfMouth.X = (mouthLeftEndPoint.X + mouthRightEndPoint.X) / 2;
                midPointOfMouth.Y = (mouthLeftEndPoint.Y + mouthRightEndPoint.Y) / 2;
                facePoint.midline.Add(midPointOfMouth);

                // Nose
                var noseEndL = shape.GetPart((uint)39);
                Point noseLeftEndPoint = new Point(noseEndL.X, noseEndL.Y);
                facePoint.nose.Add(noseLeftEndPoint);

                var noseEndR = shape.GetPart((uint)42);
                Point noseRightEndPoint = new Point(noseEndR.X, noseEndR.Y);
                facePoint.nose.Add(noseRightEndPoint);
            }

            BitmapImage detectedImg = Mat2Bmp(copyOrigImg);
            detectedImg.DecodePixelHeight = 1100;

            return detectedImg;
        }
        
        private Array2D<RgbPixel> Mat2array2D(Mat matImg)
        {
            // Save as bytes of image
            var temp = new byte[matImg.Width * matImg.Height * matImg.ElemSize()];
            Marshal.Copy(matImg.Data, temp, 0, temp.Length);

            var converted = Dlib.LoadImageData<RgbPixel>(temp, (uint)matImg.Height, (uint)matImg.Width,
                                                        (uint)(matImg.Width * matImg.ElemSize()));
            return converted;
        }

        private BitmapImage Mat2Bmp(Mat matImg)
        {
            Bitmap showImg = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(matImg);

            MemoryStream memoryStream = new MemoryStream();
            showImg.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

            var converted = new BitmapImage();
            converted.BeginInit();
            converted.StreamSource = memoryStream;
            converted.CacheOption = BitmapCacheOption.OnLoad;
            converted.EndInit();

            return converted;
        }
    }
}
