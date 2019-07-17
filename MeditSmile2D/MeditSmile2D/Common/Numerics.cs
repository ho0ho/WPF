using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
