using OpenCvSharp;
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
    public class DrawFaceAlign
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
            InitFaceAlign();
        }


        public LineGeometry midline, eyeline, lipline, noselineL, noselineR;
        public EllipseGeometry eyeL, eyeR, mouthEndL, mouthEndR, noseL, noseR;
        private void InitFaceAlign()
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
            
            //// down: from midPointOfEyes to midPointOfMouth
            //WindowsPoint down = slope(facePoint.midline[1].X, facePoint.midline[1].Y,
            //    facePoint.midline[0].X, facePoint.midline[0].Y, heightOfImg);
            //// up: from midPointOfMouth to midPointOfMouth
            //WindowsPoint up = slope(facePoint.midline[1].X, facePoint.midline[1].Y,
            //    facePoint.midline[0].X, facePoint.midline[0].Y);

            // down: from midPointOfEyes to midPointOfMouth
            WindowsPoint up = slope(facePoint.midline[0].X, facePoint.midline[0].Y,
                facePoint.midline[1].X, facePoint.midline[1].Y);
            // up: from midPointOfMouth to midPointOfMouth
            WindowsPoint down = slope(facePoint.midline[0].X, facePoint.midline[0].Y,
                facePoint.midline[1].X, facePoint.midline[1].Y, heightOfImg);

            midline.StartPoint = up;
            midline.EndPoint = down;

            // noseline 연장하기
            WindowsPoint mid_mouth = new WindowsPoint(facePoint.midline[0].X, facePoint.midline[0].Y);
            WindowsPoint mid_eyes = new WindowsPoint(facePoint.midline[1].X, facePoint.midline[1].Y);
            double slopeNose = ((mid_eyes.X - mid_mouth.X) / (mid_eyes.Y - mid_mouth.Y));

            // noselineL
            noselineL = new LineGeometry();
            double bottomLX = slopeNose * ((double)heightOfImg - (double)facePoint.nose[0].Y) + (double)facePoint.nose[0].X;
            double topLX = slopeNose * (0 - (double)facePoint.nose[0].Y) + (double)facePoint.nose[0].X;

            noselineL.StartPoint = new WindowsPoint(topLX, 0);
            noselineL.EndPoint = new WindowsPoint(bottomLX, heightOfImg);

            // noselineR
            noselineR = new LineGeometry();
            double bottomRX = slopeNose * ((double)heightOfImg - (double)facePoint.nose[1].Y) + (double)facePoint.nose[1].X;
            double topRX = slopeNose * (0 - (double)facePoint.nose[1].Y) + (double)facePoint.nose[1].X;

            noselineR.StartPoint = new WindowsPoint(topRX, 0);
            noselineR.EndPoint = new WindowsPoint(bottomRX, heightOfImg);

            // eyeline 연장하기
            eyeline = new LineGeometry();
            eyeline.StartPoint = new WindowsPoint(facePoint.eye[0].X, facePoint.eye[0].Y);
            eyeline.EndPoint = new WindowsPoint(facePoint.eye[1].X, facePoint.eye[1].Y);

            // lipline 연장하기
            lipline = new LineGeometry();
            lipline.StartPoint = new WindowsPoint(facePoint.mouth[0].X, facePoint.mouth[0].Y);
            lipline.EndPoint = new WindowsPoint(facePoint.mouth[1].X, facePoint.mouth[1].Y);

            /*
             *  mark of face
             *  - eye   : 5~6
             *  - mouth : 7~8
             *  
             */
            // mark of eyes
            eyeL = new EllipseGeometry();
            eyeL.Center = OpenCVPointToWindowsPoint(facePoint.eye[0]);
            // eyeL.Center = new WindowsPoint(facePoint.eye[0].X, facePoint.eye[0].Y);
            eyeL.RadiusX = 1;
            eyeL.RadiusY = 1;

            eyeR = new EllipseGeometry();
            eyeR.Center = OpenCVPointToWindowsPoint(facePoint.eye[1]);
            eyeR.RadiusX = 1;
            eyeR.RadiusY = 1;

            // nose
            noseL = new EllipseGeometry();
            noseL.Center = new WindowsPoint(facePoint.nose[0].X, facePoint.nose[0].Y);
            noseL.RadiusX = 1;
            noseL.RadiusY = 1;

            noseR = new EllipseGeometry();
            noseR.Center = new WindowsPoint(facePoint.nose[1].X, facePoint.nose[1].Y);
            noseR.RadiusX = 1;
            noseR.RadiusY = 1;            

            // mark of mouth
            mouthEndL = new EllipseGeometry();
            mouthEndL.Center = OpenCVPointToWindowsPoint(facePoint.mouth[0]);
            mouthEndL.RadiusX = 1;
            mouthEndL.RadiusY = 1;

            mouthEndR = new EllipseGeometry();
            mouthEndR.Center = OpenCVPointToWindowsPoint(facePoint.mouth[1]);
            mouthEndR.RadiusX = 1;
            mouthEndR.RadiusY = 1;
        }

        private WindowsPoint OpenCVPointToWindowsPoint(OpenCVPoint point)
        {
            return new WindowsPoint(point.X, point.Y);
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
