using System;

namespace Task5
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //    FileHandler arrayFile = new FileHandler("../../../ArrayData.txt");
            //    FileHandler sortFile = new FileHandler("../../../SortedArray.txt");
            //    FileHandler tmpVectorFile = new FileHandler("../../../TmpVector.txt");
            //    try
            //    {
            //        Vector.FileSplitMergeSort(arrayFile, sortFile, tmpVectorFile, 5, Trend.decrease);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }

            Matrix matrix = new Matrix(2, 2);
            matrix.FIll(Filling.vertical, Direction.up);
            Console.WriteLine(matrix);
            foreach (int item in matrix)
            {
                Console.WriteLine(item);
            }
        }
    }
}