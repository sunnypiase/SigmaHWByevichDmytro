using System;
using System.Collections.Generic;

namespace Task2
{
    internal class Menu
    {
        private List<Option> _options;
        public Menu()
        {
            _options = new List<Option>();
        }
        public Menu(List<Option> options)
        {
            _options = new List<Option>(options);
        }
        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Select option: ");
            for (int i = 0; i < _options.Count; i++)
            {
                Console.WriteLine($"{i} > {_options[i]};");
            }
            Console.WriteLine("X > Close menu");
            Console.Write("Input > ");
            if (InvokeSelectedOption())
            {
                PrintMenu();
            }
        }
        private bool InvokeSelectedOption()
        {
            string inputedValue = Console.ReadLine();

            if (inputedValue.ToLower() == "x")
            {
                Console.Clear();
                return false;
            }

            int selectedOption = int.Parse(inputedValue);

            if (selectedOption < 0 || selectedOption >= _options.Count)
            {
                throw new IndexOutOfRangeException("option does not exist");
            }

            _options[selectedOption].Run();
            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
            return true;
        }

    }
}
