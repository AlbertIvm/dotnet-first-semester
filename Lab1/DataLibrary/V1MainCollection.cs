using System.Collections.Generic;
using System.Text;

namespace DataLibrary
{
    public class V1MainCollection
    {
        private List<V1Data> Data;

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
    }
}
