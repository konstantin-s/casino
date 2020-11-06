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
                var inputName = GetPlayerName();

                var player = new Player(inputName);

                Console.Out.WriteLine("Приветствую, {0}!\n\n", player.Name);
                Console.Out.WriteLine("Угадайте число от {0:D} до {1:D} и выигайте полцарства!", Constants.RangeFrom, Constants.RangeTo);

                var secretNumber = new Random().Next(Constants.RangeFrom, Constants.RangeTo);
                var tryCount = 0;
                do
                {
                    int playerNumber = ReadNumber();
                    tryCount++;
                    if (secretNumber.Equals(playerNumber))
                    {
                        break;
                    }

                    Console.Out.WriteLine("Неа, не угадал, давай еще разок!");
                    string hint = secretNumber.CompareTo(playerNumber) == -1 ? "меньше" : "больше";

                    Console.Out.WriteLine($"Подсказка: загаданное число {hint}");

                    Console.Out.WriteLine("Угадайте число от {0:D} до {1:D} и выигайте полцарства!", Constants.RangeFrom, Constants.RangeTo);
                } while (true);

                string declension = DeclensionGenerate(tryCount, "попытку", "попытки", "попыток");
                string sn = secretNumber.ToString();
                Console.Out.WriteLine($"Урашечки! Угадал, это действительно {sn}! Угадал всего за {tryCount} {declension}!\n ");
                Console.Out.WriteLine("Всё, пока!");
                Console.In.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string GetPlayerName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            string inputName;
            do
            {
                var msg = $@"Представьтесь пожалуйста:";
                Console.Out.WriteLine("-----\n\t\t{0}\n------\n", msg);

                inputName = Console.In.ReadLine();
                Console.Clear();
                if (string.IsNullOrWhiteSpace(inputName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Разве это имя ...");
                    continue;
                }

                inputName = new string(inputName.Trim().Where(char.IsLetter).ToArray());

                if (inputName.Length < Constants.PlayerNameMin)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.WriteLine("Имя не может быть короче {0} букв!", Constants.PlayerNameMin.ToString());
                }
                else
                {
                    break;
                }
            } while (true);

            Console.ForegroundColor = ConsoleColor.Yellow;
            return inputName;
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

        /// <summary>
        /// Возвращает слова в падеже, зависимом от заданного числа
        /// </summary>
        /// <param name="number">Число от которого зависит выбранное слово</param>
        /// <param name="nominativ">Именительный падеж слова. Например "день"</param>
        /// <param name="genetiv">Родительный падеж слова. Например "дня"</param>
        /// <param name="plural">Множественное число слова. Например "дней"</param>
        /// <returns></returns>
        public static string DeclensionGenerate(int number, string nominativ, string genetiv, string plural)
        {
            var titles = new[] {nominativ, genetiv, plural};
            var cases = new[] {2, 0, 1, 1, 1, 2};
            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
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