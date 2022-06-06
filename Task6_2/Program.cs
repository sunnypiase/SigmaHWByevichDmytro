﻿using System;
using System.IO;

namespace Task6_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var sr = new StreamReader("../../../Text.txt"))
                {
                    TextHandler textHandler = new TextHandler(sr);
                    Console.WriteLine(textHandler.GetMaxMinWordsInSentences());
                    using (var sw = new StreamWriter("../../../Result.txt"))
                    {
                        textHandler.WriteSentancesInFile(sw);
                    }
                }
                

            }
            catch (Exception e )
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
