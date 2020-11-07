using System;
using System.Linq;

namespace ruletka_next.net
{
    public class Dealer
    {
        public void Greet()
        {
            Console.Clear();
            Console.Out.WriteLine("Приветствую Вас в нашем Жёванном казино!");
        }

        public Player Meet()
        {
            var inputName = GetPlayerName();

            var player = new Player(inputName);

            return player;
        }

        private static string GetPlayerName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            string inputName;
            do
            {
                var msg = $@"Представьтесь пожалуйста:";
                Console.Out.WriteLine("{0}", msg);

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

        public void GameInfo(Player player)
        {
            Console.Out.WriteLine("Здравствуйте {0}!\n", player.Name);
            Console.Out.WriteLine("Ваш баланс: {0}!\n\n", player.balance.Value);
        }
    }
}