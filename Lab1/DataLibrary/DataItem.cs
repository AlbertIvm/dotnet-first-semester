using System;
using System.Numerics;

namespace DataLibrary
{
    public struct DataItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Complex Value { get; set; }
        public double Magnitude { get => Value.Magnitude; }

        public DataItem(double x, double y, Complex value)
        {
            X = x;
            Y = y;
            Value = value;
        }

        public string ToLongString(string format)
        {
            return string.Format(
                   $"Coordinates: ({X.ToString(format)}, {Y.ToString(format)}), " +
                   $"field value: ({Value.Real.ToString(format)}, {Value.Imaginary.ToString(format)}), " +
                   $"field magnitude: {Value.Magnitude.ToString(format)}"
                   );
        }

        public override string ToString()
        {
            return $"DataItem(X:{X}, Y:{Y}, Value:{Value})";
        }

        public static double Distance(DataItem a, DataItem b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}
