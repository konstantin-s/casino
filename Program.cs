using System;
using System.Linq;

namespace ruletka
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var msg = $@"Здрасьте! Представьтесь пожалуйста:";
                Console.Out.WriteLine("-----\n\t\t{0}\n------\n", msg);


                var inputName = "";

                while (String.IsNullOrWhiteSpace(inputName))
                {
                    inputName = Console.In.ReadLine();
                    if (inputName == null)
                    {
                        continue;
                    }

                    inputName = inputName.Trim();
                    inputName = new string(inputName.Where(c => Char.IsLetter(c)).ToArray());

                    if (inputName.Length < Constants.PlayerNameMin)
                    {
                        Console.Out.WriteLine("Имя не может быть короче {0} букв!", Constants.PlayerNameMin.ToString());
                        inputName = "";
                    }
                }

                var player = new Player(inputName);

                Console.Out.WriteLine("Приветствую, {0}!\n\n", player.Name);
                Console.Out.WriteLine("Угадайте число от {0:D} до {1:D} и выигайте полцарства!", Constants.RangeFrom, Constants.RangeTo);

                var secretNumber = new Random().Next(Constants.RangeFrom, Constants.RangeTo);

                do
                {
                    int playerNumber = ReadNumber();
                    if (secretNumber.Equals(playerNumber))
                    {
                        break;
                    }

                    Console.Out.WriteLine("Неа, не угадал, давай еще разок!");
                    Console.Out.WriteLine("Угадайте число от {0:D} до {1:D} и выигайте полцарства!", Constants.RangeFrom, Constants.RangeTo);
                } while (true);


                Console.Out.WriteLine("Урашечки! Угадал, это действительно {0}\n", secretNumber.ToString());
                Console.Out.WriteLine("Всё, пока!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static int ReadNumber()
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

        private static string ReadNumberByChar()
        {
            string input = "";

            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (char.IsNumber(keyInfo.KeyChar))
                {
                    input = input + keyInfo.KeyChar;
                    Console.Write(keyInfo.KeyChar);
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            } while (true);

            return input;
        }
    }

    static class Constants
    {
        public const int PlayerNameMin = 2;
        public const int RangeFrom = 1;
        public const int RangeTo = 10;
    }

    internal class Player
    {
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
        }
    }
}