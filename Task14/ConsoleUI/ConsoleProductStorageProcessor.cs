using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Task14.ConsoleUI.ConsoleProductReaders;
using Task14.FileHandler;
using Task14.Readers;
using Task14.Validators;
using Task14.Serialize;

namespace Task14.ConsoleUI
{
    internal class ConsoleProductStorageProcessor<T>
         where T : class, IProduct
    {
        private Dictionary<string, IConsoleProductReader<T>> _consoleReaders;
        private Dictionary<string, ITXTSerializedParametersParser<T>> _parsersByType;
        public IStreamSerializer<ProductStorage<T>> StreamStorageSerializer { get; set; }
        private ProductStorage<T> _producStorage;
        private Menu _mainMenu;
        private Menu _actionMenu;
        private Menu _addproductMenu;
        private List<Option> _mainMenuOptions;
        private List<Option> _actionMenuOptions;
        private List<Option> _productAddOptions;
        private Action<T> _currentActionForActionMenu;
        public ConsoleProductStorageProcessor(ProductStorage<T> producStorage, Dictionary<string, IConsoleProductReader<T>> consoleReaders, Dictionary<string, ITXTSerializedParametersParser<T>> parsersByType)
        {
            _consoleReaders = new(consoleReaders);
            _producStorage = producStorage;
            _parsersByType = new(parsersByType);
            UpdateMainMenu();
        }
        private void UpdateMainMenu()
        {
            UpdateAddProductMenu();
            _mainMenuOptions = new List<Option>()
            {
                {new("Додати продукт",               () => _addproductMenu.PrintMenu() )},

                {new("Вивести склад на екран",       () => PrintStorage() )},

                {new("Зчитати склад з файлу",        () => ReadProductStorageFormFile() )},

                {new("Записати склад у файл",        () => FileHandlerService.WriteToFileCollection(_producStorage,new TxtSerializer(), "../../../Files/Result.txt") )},

                {new("Відсортувати склад",           () => _producStorage.Sort() )},

                {new("Вивести сумарну ціну скалду",  () => PrintStoragePrice() )},

                {new("Найдорощий продукт",           () => PrintStorageMaxPrice() )},

                {new("Видалити продукт",             () => DoActionOnProductMenu(DeleteProduct) )},

                {new("Змінити ціну продукту",        () => DoActionOnProductMenu(ChangeProductPrice) )},

                {new("Серіалізувати ",               () => SerializeStorage() )},

                {new("Десеріалізувати ",               () => DeSerializeStorage() )},
            };
            if (_mainMenu is null)
            {
                _mainMenu = new Menu(_mainMenuOptions)
                {
                    Title = "Головне меню"
                };
            }
            else
            {
                _mainMenu.ChangeOption(_mainMenuOptions);
            }
        }
        public void PrintMenu()
        {
            _mainMenu.PrintMenu();
        }
        private void UpdateAddProductMenu()
        {
            _productAddOptions = new List<Option>();
            foreach (KeyValuePair<string, IConsoleProductReader<T>> reader in _consoleReaders)
            {
                _productAddOptions.Add(new Option(reader.Key, () => ConsoleReadProduct(reader.Value)));
            }
            if (_addproductMenu is null)
            {
                _addproductMenu = new Menu(_productAddOptions)
                {
                    Title = "Додавання нового продукту"
                };
            }
            else
            {
                _addproductMenu.ChangeOption(_productAddOptions);
            }

        }
        private void UpdateActionMenu()
        {
            _actionMenuOptions = new List<Option>();
            foreach (T item in _producStorage)
            {
                _actionMenuOptions.Add(new(item.ToString(), () => DoActionOnProduct<int>(item)));
            }
            if (_actionMenu is null)
            {
                _actionMenu = new Menu(_actionMenuOptions, "Оберить продукт: ");
            }
            else
            {
                _actionMenu.ChangeOption(_actionMenuOptions);
            }
        }
        private void DoActionOnProductMenu(Action<T> action)
        {
            _currentActionForActionMenu = action;
            UpdateActionMenu();
            _actionMenu.PrintMenu();
        }
        private void DoActionOnProduct<G>(T product)
        {
            try
            {
                _currentActionForActionMenu?.Invoke(product);
            }
            catch (Exception)
            {
                Console.WriteLine("Не вдалося виконати цю дію, непердбачена помилка");
            }
        }
        private void DeleteProduct(T product)
        {
            if (_producStorage.Remove(product))
            {
                Console.WriteLine("Продукт успішно видалено");
            }
            else
            {
                Console.WriteLine("Продукт не вдалося видалити");
            }
            UpdateActionMenu();
        }
        private void ChangeProductPrice(T product)
        {
            Console.Write("Введіть процент на який змінити ціну: ");
            try
            {
                product.ChangePrice(int.Parse(Console.ReadLine()));
                Console.WriteLine("Ціна успішно змінена");
            }
            catch (Exception)
            {
                Console.WriteLine("Не вдалося змінити ціну");
            }
            UpdateActionMenu();
        }
        private void ConsoleReadProduct(IConsoleProductReader<T> consoleProductReader)
        {
            try
            {
                _producStorage.Add(consoleProductReader.ConsoleReadProduct());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void ReadProductStorageFormFile()
        {
            FileHandlerService.ReadToCollection
                (
                    obj: ref _producStorage,
                    collectionReader: new TXTSerializedStorageReader<T>(Logger.Instance.Log),
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
            Console.WriteLine($"Максимальна ціна на складі: {_producStorage.GetAll<T>(product => product.Price == _producStorage.MaxPrice).ElementAt(0)}");
        }

        private void SerializeStorage()
        {
            using (Stream stream = File.OpenWrite("../../../Files/Serialize.xml"))
            {
                StreamStorageSerializer.Serialize(_producStorage, stream);
            }

        }
        private void DeSerializeStorage()
        {
            using (Stream stream = File.OpenRead("../../../Files/Serialize.xml"))
            {
                _producStorage = StreamStorageSerializer.Deserialize(stream);
            }

        }
    }
}
