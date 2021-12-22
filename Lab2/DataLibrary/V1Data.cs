using System;
using System.Collections;
using System.Collections.Generic;

namespace DataLibrary
{
    public abstract class V1Data : IEnumerable<DataItem>
    {
        public string ID { get; protected set; }
        public DateTime Timestamp { get; protected set; }
        public abstract int Count { get; }
        public abstract double AverageValue { get; }
        public abstract string ToLongString(string format);
        public abstract IEnumerator<DataItem> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public V1Data(string id, DateTime timestamp)
        {
            ID = id;
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            return $"V1Data(DataItemID:{ID}, Timestamp:{Timestamp})";
        }

    }
}
