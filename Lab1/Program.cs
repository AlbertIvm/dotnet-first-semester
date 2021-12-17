using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lab1
{
    struct DataItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Complex Value { get; set; }
        public double Magnitude { get => Value.Magnitude; }

        public DataItem(double x, double y, Complex value)
        {
            this.X = x;
            this.Y = y;
            this.Value = value;
        }

        public string ToLongString(string format)
        {
            return String.Format(
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

    public delegate Complex FbdlComplex(double x, double y);

    abstract class V1Data
    {
        public string ID { get; }
        public DateTime Timestamp { get; }
        public abstract int Count { get; }
        public abstract double AverageValue { get; }
        public abstract string ToLongString(string format);

        public V1Data(string id, DateTime timestamp)
        {
            this.ID = id;
            this.Timestamp = timestamp;
        }

        public override string ToString()
        {
            return $"V1Data(DataItemID:{ID}, Timestamp:{Timestamp})";
        }
    }

    class V1DataList : V1Data
    {
        public List<DataItem> Data { get; }

        public V1DataList(string id, DateTime timestamp) : base(id, timestamp)
        {
            Data = new List<DataItem>(5);
        }

        public bool Add(DataItem newItem)
        {
            foreach (var item in Data)
            {
                if (DataItem.Distance(newItem, item) < 1e-5)
                {
                    return false;
                }
            }
            Data.Add(newItem);
            return true;
        }

        public int AddDefaults(int nItems, FbdlComplex F)
        {
            int addedCount = 0;
            Random rand = new Random();
            int boxMultiplier = 10; // Hardcoded here, can be made a property as well
            for (int i = 0; i < nItems; i++)
            {
                // Points will be clustered around (0, 0),
                // but can get arbitrarily far if there are many of them
                var x = i * boxMultiplier * (-1 + 2 * rand.NextDouble());
                var y = i * boxMultiplier * (-1 + 2 * rand.NextDouble());
                var item = new DataItem(x, y, F(x, y));
                addedCount += Convert.ToInt32(Add(item));
            }
            return addedCount;
        }

        public override int Count { get => Data.Count; }

        public override double AverageValue
        {
            get
            {
                double sum = 0;
                foreach (var item in Data)
                {
                    sum += item.Magnitude;
                }
                return sum / Count;
            }
        }

        public override string ToString()
        {
            return $"V1DataList with ID: {base.ID}, Timestamp: {base.Timestamp}, contains {Count} items";
        }

        public override string ToLongString(string format)
        {
            StringBuilder builder = new StringBuilder(ToString() + ":\n");
            foreach (var item in Data)
            {
                builder.AppendLine(item.ToLongString(format));
            }
            return builder.ToString();
        }
    }

    class V1MainCollection
    {

    }

    static class Field
    {

    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
