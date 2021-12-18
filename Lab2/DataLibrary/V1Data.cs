using System;

namespace DataLibrary
{
    public abstract class V1Data
    {
        public string ID { get; }
        public DateTime Timestamp { get; }
        public abstract int Count { get; }
        public abstract double AverageValue { get; }
        public abstract string ToLongString(string format);

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
