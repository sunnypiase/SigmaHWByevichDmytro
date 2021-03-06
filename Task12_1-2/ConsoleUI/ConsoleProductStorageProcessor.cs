using System;
using System.Collections.Generic;
using System.Linq;
using Task11.ConsoleUI.ConsoleProductAdders;
using Task11.FileHandler;
using Task11.Readers;
using Task11.Validators;

namespace Task11.ConsoleUI
{
    internal class ConsoleProductStorageProcessor
    {
        private Dictionary<string, IConsoleProductReader> _consoleReaders;
        private Dictionary<string, IStringParser<IProduct>> _parsersByType;
        private ProductStorage<IProduct> _producStorage;
        private Menu _mainMenu;
        private Menu _addproductMenu;
        private List<Option> _mainMenuOptions;
        private List<Option> _productAddOptions;

        public ConsoleProductStorageProcessor(ProductStorage<IProduct> producStorage, Dictionary<string, IConsoleProductReader> consoleReaders, Dictionary<string, IStringParser<IProduct>> parsersByType)
        {
            _consoleReaders = consoleReaders;
            _producStorage = producStorage;
            _parsersByType = parsersByType;
            UpdateMainMenu();
        }
        public void PrintMenu()
        {
            _mainMenu.PrintMenu();
        }
        private void UpdateMainMenu()
        {
            InitializeAddProductMenu();
            _mainMenuOptions = new List<Option>()
            {
                {new Option("Додати продукт", ()=>_addproductMenu.PrintMenu()) },
                {new Option("Вивести склад на екран", ()=>PrintStorage() )},
                {new Option("Зчитати склад з файлу", ()=>ReadProductStorageFormFile()) },
                {new Option("Записати склад у файл", ()=>FileHandlerService.WriteToFile(_producStorage, "../../../Files/Result.txt")) },
                {new Option("Відсортувати склад", ()=>_producStorage.Sort() )},
                {new Option("Вивести сумарну ціну скалду", ()=>PrintStoragePrice() )},
                {new Option("Найдорощий продукт", ()=>PrintStorageMaxPrice() )}
            };

            _mainMenu = new Menu(_mainMenuOptions)
            {
                Title = "Головне меню"
            };
        }
        private void InitializeAddProductMenu()
        {
            _productAddOptions = new List<Option>();
            foreach (KeyValuePair<string, IConsoleProductReader> item in _consoleReaders)
            {
                _productAddOptions.Add(new Option(item.Key, () => _producStorage.Add(item.Value.ConsoleReadProduct())));
            }
            _addproductMenu = new Menu(_productAddOptions)
            {
                Title = "Додавання нового продукту"
            };
        }
        private void ReadProductStorageFormFile()
        {
            FileHandlerService.ReadToCollection
                (
                    obj: ref _producStorage,
                    collectionReader: new TXTSerializedStorageReader<IProduct>(Logger.Instance.Log),
                    parser: _parsersByType,
                    path: "../../../Files/ProductsData.txt"
                );
            Console.WriteLine("Файл успішно прочитан");
        }
        private void PrintStorage()
        {
            if (_producStorage.Count > 0)
            {
                Console.WriteLine(_producStorage);
            }
            else
            {
                Console.WriteLine("Склад пустий");
            }

        }
        private void PrintStoragePrice()
        {
            Console.WriteLine($"Сумарна ціна скалду: {_producStorage.Pirice}");
        }
        private void PrintStorageMaxPrice()
        {
            Console.WriteLine($"Максимальна ціна на складі: {_producStorage.GetAll<IProduct>(product => product.Price == _producStorage.MaxPrice).ToList()[0]}");
        }
    }
}
