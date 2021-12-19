using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary
{
    public class V1DataList : V1Data
    {
        public List<DataItem> Data { get; }

        public V1DataList(string id, DateTime timestamp, int nItems = 5) : base(id, timestamp)
        {
            Data = new List<DataItem>(nItems);
        }

        public V1DataList(string id, DateTime timestamp, int nItems, FdblComplex f) : this(id, timestamp, nItems)
        {
            AddDefaults(nItems, f);
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

        public int AddDefaults(int nItems, FdblComplex f)
        {
            int addedCount = 0;
            Random rand = new();
            int boxMultiplier = 10; // Hardcoded here, can be made a property as well
            for (int i = 0; i < nItems; i++)
            {
                // Points will be clustered around (0, 0),
                // but can get arbitrarily far if there are many of them
                double x = i * boxMultiplier * (-1 + 2 * rand.NextDouble());
                double y = i * boxMultiplier * (-1 + 2 * rand.NextDouble());
                var item = new DataItem(x, y, f(x, y));
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
            return $"V1DataList with ID: {ID}, " +
                   $"Timestamp: {Timestamp}, contains {Count} items";
        }

        public override string ToLongString(string format)
        {
            StringBuilder builder = new(ToString() + ":\n");
            foreach (var item in Data)
            {
                builder.AppendLine(item.ToLongString(format));
            }
            return builder.ToString();
        }
        public override IEnumerator<DataItem> GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }
}
