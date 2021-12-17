using System;
using System.Numerics;

namespace DataLibrary
{
    public static class Field
    {
        public static Complex UniformField(double x, double y)
        {
            return new Complex(3.0, 4.0);
        }

        public static Complex FieldFacingInfinity(double x, double y)
        {
            double radius = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            if (radius < 1e-6)
            {
                return Complex.Zero; // For safety
            }
            return new Complex(x / Math.Pow(radius, 3), y / Math.Pow(radius, 3));
        }
    }
}
