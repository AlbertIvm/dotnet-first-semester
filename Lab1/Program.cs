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
            return $"Coordinates: ({X:format}, {Y:format}), " +
                   $"field value: ({Value.Real:format}, {Value.Imaginary:format}), " +
                   $"field magnitude: {Value.Magnitude:format}";
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

    delegate Complex FbdlComplex(double x, double y);

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
