using System;

namespace Task10_2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Matrix matrix = new Matrix(4, 3);
            matrix.FIll(Filling.diagonal, Direction.down);

            Console.WriteLine(matrix);

            foreach (int item in matrix.GetDiagonalSnakeEnumerator())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            foreach (int item in matrix.GetHorizontalSnakeEnumerator())
            {
                Console.WriteLine(item);
            }
        }
    }
}
