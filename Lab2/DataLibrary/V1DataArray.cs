using System;
using System.Numerics;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataLibrary
{
    [Serializable]
    public class V1DataArray : V1Data
    {
        public int RowNumber { get; private set; }
        public int ColNumber { get; private set; }
        public double RowSpacing { get; private set; }
        public double ColSpacing { get; private set; }
        public Complex[,] Data { get; private set; }

        public V1DataArray(string id, DateTime timestamp) : base(id, timestamp)
        {
            Data = new Complex[0, 0];
        }

        public V1DataArray(
                string id, DateTime timestamp,
                int rowNumber, int colNumber,
                double rowSpacing, double colSpacing, FdblComplex f) : base(id, timestamp)
        {
            RowNumber = rowNumber;
            ColNumber = colNumber;
            RowSpacing = rowSpacing;
            ColSpacing = colSpacing;

            Data = new Complex[rowNumber, colNumber];
            for (int i = 0; i < rowNumber; i++)
            {
                for (int j = 0; j < colNumber; j++)
                {
                    Data[i, j] = f(i * colSpacing, j * rowSpacing);
                }
            }
        }

        public override int Count { get => RowNumber * ColNumber; }

        public override double AverageValue
        {
            get
            {
                double result = 0;
                for (int i = 0; i < RowNumber; i++)
                {
                    for (int j = 0; j < ColNumber; j++)
                    {
                        result += Data[i, j].Magnitude;
                    }
                }
                return result / Count;
            }
        }

        public override string ToString()
        {
            return $"V1DataArray with ID: {ID}, Timestamp: {Timestamp}, " +
                   $"RowNumber: {RowNumber}, ColNumber: {ColNumber}";
        }

        public override string ToLongString(string format)
        {
            StringBuilder builder = new(ToString() + ", items:\n");
            for (int i = 0; i < RowNumber; i++)
            {
                for (int j = 0; j < ColNumber; j++)
                {
                    builder.AppendLine(
                            $"({(j * ColSpacing).ToString(format)}, {(i * RowSpacing).ToString(format)}): " +
                            $"field value {Data[i, j].ToString(format)}, " +
                            $"field magnitude {Data[i, j].Magnitude.ToString(format)}");
                }
            }
            return builder.ToString();
        }

        public static explicit operator V1DataList(V1DataArray array)
        {
            // Note: we're not initializing V1DataList.Count, we are just allocating memory
            V1DataList result = new(array.ID, array.Timestamp, array.Count);
            for (int i = 0; i < array.RowNumber; i++)
            {
                for (int j = 0; j < array.ColNumber; j++)
                {
                    result.Add(array.ItemInGrid(i, j));
                }
            }
            return result;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            return new V1DataArrayEnumerator(this);
        }

        public DataItem ItemInGrid(int rowIndex, int colIndex)
        {
            return new DataItem(colIndex * ColSpacing, rowIndex * RowSpacing, Data[rowIndex, colIndex]);
        }

        public static bool SaveBinary(string filename, V1DataArray v1)
        {
            try
            {
                using FileStream fileStream = File.Create(filename);
                BinaryFormatter formatter = new();
                formatter.Serialize(fileStream, v1);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured during serialization: {ex}");
                return false;
            }
        }

        public static bool LoadBinary(string filename, ref V1DataArray v1)
        {
            try
            {
                using FileStream fileStream = File.OpenRead(filename);
                BinaryFormatter formatter = new();
                v1 = formatter.Deserialize(fileStream) as V1DataArray;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred during deserialization: {ex}");
                return false;
            }
        }
    }
}
