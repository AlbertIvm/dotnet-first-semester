using System;
using System.Numerics;
using DataLibrary;

namespace Lab2
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("========== Test serialization ==========");
            TestSerialization();

            Console.WriteLine("\n========== Test LINQ ==========");
            TestLINQ();
        }


        static void TestSerialization()
        {
            V1DataArray array = new(
                id: "Array",
                timestamp: DateTime.Now,
                rowNumber: 3,
                colNumber: 3,
                rowSpacing: 0.1,
                colSpacing: 0.1,
                f: Field.FieldFacingInfinity);
            Console.WriteLine("Original array:");
            Console.WriteLine(array.ToLongString("F3"));

            if (!V1DataArray.SaveBinary("array.serialized", array))
            {
                return;
            }

            V1DataArray array_restored = null;
            if (V1DataArray.LoadBinary("array.serialized", ref array_restored))
            {
                Console.WriteLine("\nRestored array:");
                Console.WriteLine(array_restored.ToLongString("F3"));
            }
            else
            {
                return;
            }

            V1DataList list = new ("List", timestamp: DateTime.Now, 4, Field.FieldFacingInfinity);
            Console.WriteLine("Original list:");
            Console.WriteLine(list.ToLongString("F3"));

            if (!V1DataList.SaveAsText("list.json", list))
            {
                return;
            }

            V1DataList list_restored = null;
            if (V1DataList.LoadAsText("list.json", ref list_restored))
            {
                Console.WriteLine("\nRestored list:");
                Console.WriteLine(list_restored.ToLongString("F3"));
            }
        }

        static void TestLINQ()
        {
            V1MainCollection collection = new();
            Console.WriteLine("---------- Method calls for empty collection: ----------");
            Console.WriteLine($"AverageMagnitude: {collection.AverageMagnitude}");
            Console.WriteLine($"MostDeviated: {collection.MostDeviated}");
            Console.WriteLine($"CommonXCoords:");
            var iterator = collection.CommonXCoords;
            if (iterator != null)
            {
                foreach (var item in iterator)
                {
                    Console.WriteLine($"  {item}");
                }
            }
            else
            {
                Console.WriteLine($"no elements, got null");
            }

            collection.Add(new V1DataArray("Empty array", DateTime.Now, 0, 0, 0, 0, Field.UniformField));
            collection.Add(new V1DataList("Empty list", DateTime.Now, 0));
            collection.Add(new V1DataArray("Uniform array", DateTime.Now, 2, 2, 1.5, 1.5, Field.UniformField));

            V1DataList extreme_list = new("List with extreme value", DateTime.Now);
            extreme_list.Add(new DataItem(10, 20, new Complex(100, 100)));
            extreme_list.Add(new DataItem(42, -10, new Complex(0, 0)));

            V1DataList x_list = new("List with convenient X's", DateTime.Now);
            x_list.Add(new DataItem(1.5, 1000, new Complex(0, 0)));
            x_list.Add(new DataItem(42, 12, new Complex(0, 0)));

            collection.Add(extreme_list);
            collection.Add(x_list);

            Console.WriteLine("---------- MainCollection: ----------");
            Console.WriteLine(collection.ToLongString("F3"));


            Console.WriteLine("---------- Method calls for full collection: ----------");
            Console.WriteLine($"AverageMagnitude: {collection.AverageMagnitude}");
            Console.WriteLine($"MostDeviated: {collection.MostDeviated}");
            Console.WriteLine($"CommonXCoords:");
            iterator = collection.CommonXCoords;
            if (iterator != null)
            {
                foreach (var item in iterator)
                {
                    Console.WriteLine($"  {item}");
                }
            }
            else
            {
                Console.WriteLine($"no elements, got null");
            }
        }
    }
}
