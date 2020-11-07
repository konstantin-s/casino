using System;

namespace ruletka_next.net
{
    internal static class Helper
    {
        public static int ReadNumber()
        {
            {
                int number;

                do
                {
                    string readLine = Console.ReadLine();
                    if (string.IsNullOrEmpty(readLine))
                    {
                        Console.WriteLine("Ну введи уже число...");
                        continue;
                    }

                    try
                    {
                        number = Convert.ToInt32(readLine);
                        break;
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("{0} - ну это слишком большое число, давай поменьше!", readLine);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Что-то вот это вот: '{0}' - не похоже на число!", readLine);
                    }
                } while (true);

                return number;
            }
        }
    }
}