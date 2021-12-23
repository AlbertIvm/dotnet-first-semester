using System;
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

        }
    }
}
