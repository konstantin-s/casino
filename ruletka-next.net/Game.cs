using System;

namespace ruletka_next.net
{
    internal class Game
    {
        private readonly Player _player;
        private readonly Dealer _dealer;

        public Game(Player player, Dealer dealer)
        {
            _player = player;
            _dealer = dealer;
        }

        public void Run()
        {
            int roundCount = 0;
            do
            {
                roundCount++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine($"Раунд № {roundCount}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine($"Угадайте число от {Constants.RangeFrom} до {Constants.RangeTo}");
                Console.Out.WriteLine($"и выйграйте в {Constants.WinMult} раза больше, чем поставили!!\n");

                int stake = GetStake();
                var secretNumber = new Random().Next(Constants.RangeFrom, Constants.RangeTo);

                Console.Out.WriteLine($"На какое число ставите?");
                int playerNumber = Helper.ReadNumber();
                if (secretNumber == playerNumber)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Out.WriteLine($":) Урашечки! Угадал, это действительно {secretNumber}!");
                    Console.ForegroundColor = ConsoleColor.Green;
                    try
                    {
                        checked
                        {
                            _player.balance.Value += stake * Constants.WinMult;
                        }
                    }
                    catch (OverflowException)
                    {
                        _player.balance.Value = Constants.StartBalance;
                        Console.WriteLine("О нет! Баланс лопнул!");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Out.WriteLine($":(  Не угадал, на рулетке выпало {secretNumber}!\n ");
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                if (_player.balance.Value <= 0)
                {
                    Console.Out.WriteLine($":(  А денег-то больше нет! \n ");
                    break;
                }

                Console.In.ReadLine();
            } while (true);
        }

        private int GetStake()
        {
            int stake;
            do
            {
                Console.Out.WriteLine($"Баланс: {_player.balance.Value}");
                Console.Out.WriteLine("Ваша ставка?");
                stake = Helper.ReadNumber();
                if (stake <= 0)
                {
                    Console.Out.WriteLine($"Ставка {stake}? Прикольно, но нет!");
                    continue;
                }

                if (stake > _player.balance.Value)
                {
                    Console.Out.WriteLine($"Ставка {stake} больше, чем денег на балансе: {_player.balance.Value}");
                    continue;
                }

                _player.balance.Value -= stake;
                Console.Out.WriteLine($"Ставка {stake} принята! Если что, на балансе: {_player.balance.Value}");
                break;
            } while (true);

            return stake;
        }
    }
}