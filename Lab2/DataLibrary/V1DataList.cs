using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataLibrary
{
    [JsonConverter(typeof(V1DataListJsonConverter))]
    public class V1DataList : V1Data
    {
        public List<DataItem> Data { get; }

        public V1DataList(string id, DateTime timestamp, int nItems = 5) : base(id, timestamp)
        {
            Data = new List<DataItem>(nItems);
        }

        public V1DataList(string id, DateTime timestamp, in List<DataItem> list) : base(id, timestamp)
        {
            Data = list;
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

        public static bool SaveAsText(string filename, V1DataList v1)
        {
            try
            {
                string serialized = JsonSerializer.Serialize<V1DataList>(v1);
                File.WriteAllText(filename, serialized);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred during serialization: {ex}");
                return false;
            }
        }

        public static bool LoadAsText(string filename, ref V1DataList v1)
        {
            try
            {
                using FileStream fileStream = File.OpenRead(filename);
                v1 = JsonSerializer.Deserialize<V1DataList>(fileStream);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured during deserialization: {ex}");
                return false;
            }
        }
    }
}
