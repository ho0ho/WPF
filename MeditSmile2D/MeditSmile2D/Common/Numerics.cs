using MeditSmile2D.View;
using MeditSmile2D.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace MeditSmile2D.Common
{
    class Numerics
    {

        public const double Epsilon = 0.00001;

        public static bool FloatEquals(float f1, float f2) { return Math.Abs(f1 - f2) < Epsilon; }

        public static bool FloatEquals(float f1, float f2, double error)
        {
            if ((float.IsNegativeInfinity(f1) && float.IsNegativeInfinity(f2)) || (float.IsPositiveInfinity(f1) && float.IsPositiveInfinity(f2)))
                return true;
            return Math.Abs(f1 - f2) < error;
        }

        public static bool DoubleEquals(double d1, double d2)
        {
            if ((double.IsNegativeInfinity(d1) && double.IsNegativeInfinity(d2)) || (double.IsPositiveInfinity(d1) && double.IsPositiveInfinity(d2)))
                return true;
            return Math.Abs(d1 - d2) < Epsilon;
        }

        public static float RadianToDegreeConvert(float radian)
        {
            // return (180*radian)/Math.PI;
            return 57.9577951308232f * radian;
        }

        public static double DegreeToRadianConvert(double degree)
        {
            return (degree * Math.PI) / 180;
        }

        #region Get Points for Tooth


        public static Point GetMinXY_Tooth(IEnumerable pts)
        {
            double xMin = double.MaxValue;
            double yMin = double.MaxValue;
            foreach (IEnumerable points in pts)
            {
                foreach (PointViewModel point in points)
                {
                    if (point.X < xMin)
                        xMin = point.X;
                    if (point.Y < yMin)
                        yMin = point.Y;
                }
            }
            return new Point(xMin, yMin);
        }

        public static Point GetMaxXY_Tooth(IEnumerable pts)
        {
            double xMax = double.MinValue;
            double yMax = double.MinValue;

            foreach (IEnumerable points in pts)
            {
                foreach (PointViewModel point in points)
                {
                    if (point.X > xMax)
                        xMax = point.X;
                    if (point.Y > yMax)
                        yMax = point.Y;
                }
            }
            return new Point(xMax, yMax);
        }

        #endregion

        #region Get Points for Teeth

        public static Point GetMinX_Teeth(List<Point> pts)
        {
            Point p = new Point(double.MaxValue, 0);
            foreach (Point pt in pts)
                if (pt.X < p.X)
                    p = pt;
            return p;
        }

        public static Point GetMinY_Teeth(List<Point> pts)
        {
            Point p = new Point(0, double.MaxValue);
            foreach (Point pt in pts)
                if (pt.Y < p.Y)
                    p = pt;
            return p;
        }

        public static Point GetMaxX_Teeth(List<Point> pts)
        {
            Point p = new Point(double.MinValue, 0);
            foreach (Point pt in pts)
                if (pt.X > p.X)
                    p = pt;
            return p;
        }

        public static Point GetMaxY_Teeth(List<Point> pts)
        {
            Point p = new Point(0, double.MinValue);
            foreach (Point pt in pts)
                if (pt.Y > p.Y)
                    p = pt;
            return p;
        }

        #endregion

        public static List<Point> TeethToList(Teeth teeth)
        {
            List<Point> list = new List<Point>();
            //Teeth th = teeth as Teeth;
            //if (teeth.GetType() == Type.GetType("MeditSmile2D.View.DrawTeeth"))
            //    th = (DrawTeeth)teeth;
            //else if (teeth.GetType() == Type.GetType("MeditSmile2D.View.WrapTeeth"))
            //    th = teeth as WrapTeeth;
            //else if (teeth.GetType() == Type.GetType("MeditSmile2D.View.RotateTeeth"))
            //    th = teeth as RotateTeeth;

            foreach (PointViewModel p in teeth.Points)
                    list.Add(new Point(p.X, p.Y));
            return list;
        }

        public static List<List<Point>> ToothToList(FrameworkElement tooth)
        {
            WrapTooth wrap = null;
            if (tooth is UpperTooth)
            {
                UpperTooth th = tooth as UpperTooth;
                wrap = th.WrapTooth_UpperTooth;
            }
            else
            {
                LowerTooth th = tooth as LowerTooth;
                wrap = th.WrapTooth_LowerTooth;
            }

            List<List<Point>> list = new List<List<Point>>();
            foreach (ObservableCollection<PointViewModel> t in wrap.Points)
            {
                List<Point> sublist = new List<Point>();
                foreach (PointViewModel p in t)
                    sublist.Add(new Point(p.X, p.Y));
                list.Add(sublist);
            }
            return list;
        }
    }
}
