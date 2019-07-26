using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCVPoint = OpenCvSharp.Point;
using WindowsPoint = System.Windows.Point;

namespace MeditSmile2D.View.Utils
{
    class DrawFaceAlign
    {
        FaceDetector.FacePoint facePoint;
        private MainWindow main;

        public DrawFaceAlign()
        {
            facePoint = new FaceDetector.FacePoint();
            main = Application.Current.MainWindow as MainWindow;
        }

        double heightOfImg;
        public DrawFaceAlign(FaceDetector.FacePoint facePoint, double height)
        {
            heightOfImg = height;
            this.facePoint = facePoint;
            InitFaceAlign(facePoint);
        }


        public LineGeometry midline, eyeline, lipline, noselineL, noselineR;
        public EllipseGeometry eyeL, eyeR, mouthEndL, MouthEndR;
        private void InitFaceAlign(FaceDetector.FacePoint facePoint)
        {
            /*
             * line of face
             *  - mid   : 0
             *  - nose  : 1~2
             *  - eye   : 3
             *  - mouth : 4
             */


            // midline 연장하기
            midline = new LineGeometry();
            
            // down: from midPointOfEyes to midPointOfMouth
            WindowsPoint down = slope(facePoint.midline[1].X, facePoint.midline[1].Y,
                facePoint.midline[0].X, facePoint.midline[0].Y, heightOfImg);
            // up: from midPointOfMouth to midPointOfMouth
            WindowsPoint up = slope(facePoint.midline[1].X, facePoint.midline[1].Y,
                facePoint.midline[0].X, facePoint.midline[0].Y);

            midline.StartPoint = up;
            midline.EndPoint = down;

            // noseline 연장하기
            OpenCVPoint mid_eyes = new OpenCVPoint(facePoint.midline[0].X, facePoint.midline[0].Y);
            OpenCVPoint mid_mouth = new OpenCVPoint(facePoint.midline[1].X, facePoint.midline[1].Y);
            double slopeNoseL = ((mid_eyes.X - mid_mouth.X) / (mid_eyes.Y - mid_mouth.Y));

            // noselineL
            noselineL = new LineGeometry();
            double bottomRX = slopeNoseL * ((double)heightOfImg - (double)facePoint.nose[0].Y) + (double)facePoint.nose[0].X;
            double top -09876]X = slopeNoseL * (0 - (double)facePoint.nose[0].Y) + (double)facePoint.nose[0].X;

            noselineL.StartPoint = new WindowsPoint(topX, 0);
            noselineL.EndPoint = new WindowsPoint(bottomX, heightOfImg);

            // noselineR
            noselineR = new LineGeometry();
            double bottomX = slopeNoseL * ((double)heightOfImg - (double)facePoint.nose[0].Y) + (double)facePoint.nose[0].X;
            double topX = slopeNoseL * (0 - (double)facePoint.nose[0].Y) + (double)facePoint.nose[0].X;








        }

        private WindowsPoint slope(double x1, double y1, double x2, double y2, double height = 0)
        {
            double slope = (x2 - x1) / (y2 - y1);
            double x = slope * (height - y1) + x1;
            double y = height;

            return new WindowsPoint(x, y);
        }
    }
}
