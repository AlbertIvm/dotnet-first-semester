using System;

using DataLibrary;

namespace Lab1
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("========== Print #1 ==========");
            V1DataArray array = new(
                id: "Array_0",
                timestamp: DateTime.Now,
                rowNumber: 3,
                colNumber: 3,
                rowSpacing: 0.1,
                colSpacing: 0.1,
                f: Field.FieldFacingInfinity);
            Console.WriteLine(array.ToLongString("F3"));


            Console.WriteLine("========== Print #2 ==========");
            V1DataList list = (V1DataList)array;
            Console.WriteLine(list.ToLongString("F3"));

            Console.WriteLine($"Array count: {array.Count}, average value: {array.AverageValue}");
            Console.WriteLine($"List count: {list.Count}, average value: {list.AverageValue}");


            Console.WriteLine("========== Print #3 ==========");
            V1MainCollection collection = new();
            collection.Add(new V1DataArray("Array_1", timestamp: DateTime.Now, 2, 2, 0.1, 0.1, Field.FieldFacingInfinity));
            collection.Add(new V1DataArray("Array_2", timestamp: DateTime.Now, 2, 2, 1, 1, Field.FieldFacingInfinity));
            collection.Add(new V1DataList("List_1", timestamp: DateTime.Now, 4, Field.FieldFacingInfinity));
            collection.Add(new V1DataList("List_2", timestamp: DateTime.Now, 4, Field.FieldFacingInfinity));

            Console.WriteLine(collection.ToLongString("F3"));

            for (int i = 0; i < collection.Count; i++)
            {
                Console.WriteLine($"Collection with id {collection[i].ID}, count: {collection[i].Count}, average value: {collection[i].AverageValue}");
            }
        }
    }
}
