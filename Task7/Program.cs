using System;

namespace Task7
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;


            try
            {

                MainStorageMenu mainStorageMenu = new MainStorageMenu("../../../Products.txt", "../../../StorageLog.txt");
                mainStorageMenu.PrintMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }




        }

    }
}
