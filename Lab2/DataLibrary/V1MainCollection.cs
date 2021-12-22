using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLibrary
{
    public class V1MainCollection
    {
        private readonly List<V1Data> Data = new();

        public int Count { get => Data.Count; }

        public V1Data this[int index] { get => Data[index]; }

        public bool Contains(string id)
        {
            foreach (var item in Data)
            {
                if (item.ID == id)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Add(V1Data v1Data)
        {
            if (Contains(v1Data.ID))
            {
                return false;
            }
            else
            {
                Data.Add(v1Data);
                return true;
            }
        }

        public string ToLongString(string format)
        {
            StringBuilder builder = new("V1MainCollection with items:\n");
            foreach (var item in Data)
            {
                builder.AppendLine(item.ToLongString(format));
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            StringBuilder builder = new("V1MainCollection with items:\n");
            foreach (var item in Data)
            {
                builder.AppendLine(item.ToString());
            }
            return builder.ToString();
        }

        public double AverageMagnitude
        {
            get
            {
                if (Data.Sum(col => col.Count) == 0)
                {
                    return double.NaN;
                }

                return (from col in Data
                        from item in col
                        select item.Magnitude).Average();
            }
        }

        public DataItem? MostDeviated
        {
            get
            {
                double average = AverageMagnitude;
                if (Double.IsNaN(average))
                {
                    return null;
                }

                return (from col in Data
                        from item in col
                        orderby Math.Abs(item.Magnitude - average) descending
                        select item).First();
            }
        }

        public IEnumerable<float> CommonXCoords
        {
            get
            {
                if (Count == 0)
                {
                    return null;
                }

                return (from col in Data
                        from item in col
                        group item.X by item.X into x_group
                        where x_group.Count() >= 2
                        select (float) x_group.Key);
            }
        }
    }
}
