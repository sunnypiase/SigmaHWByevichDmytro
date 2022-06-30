﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Lesson16_06
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            try
            {
                Dictionary<string, string> dictionary = TextReader.ReadDictionary("../../../Dictionary.txt");
                IEnumerable<string> text = TextReader.ReadText("../../../Text.txt");

                Translator translator = new Translator();
                translator.SetPath("../../../Dictionary.txt");

                translator.SetDictionary(dictionary);

                translator.OnNotFoundedInDictionary += AddToDictionary;
                foreach (string line in text)
                {
                    translator.AddText(line);
                }

                Console.WriteLine(translator.TranslateWords());

            }
            catch (WordNotInDictionaryException)
            {
                Console.WriteLine("Не знайдено слово, та не вдалося успішно додати");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не знайдено");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void AddToDictionary(string word, ref Dictionary<string, string> dictionary)
        {
            bool isEnd = false;
            while (isEnd == false)
            {
                Console.Write($"Введіть переклад слова {word}> ");
                try
                {
                    string tmpWord = Console.ReadLine();
                    if (string.IsNullOrEmpty(tmpWord) == false)
                    {
                        dictionary[word] = tmpWord;
                        TextReader.AddToDictionary("../../../Dictionary.txt", word, tmpWord);
                        isEnd = true;
                    }
                    else
                    {
                        Console.WriteLine("Помилка додавання слова.");
                        Console.WriteLine("Натисність 1 щоб спробувати ще раз");
                        Console.WriteLine("Натисність будь яку кнопку щоб закінчити програму");
                        Console.Write(" > ");
                        if (Console.ReadKey(true).Key != ConsoleKey.D1)
                        {
                            throw new WordNotInDictionaryException();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}
