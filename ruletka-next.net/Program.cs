using System;

namespace ruletka_next.net
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Dealer dealer = new Dealer();
                dealer.Greet();
                
                Player player = dealer.Meet();

                dealer.GameInfo(player);

                Game game = new Game(player, dealer);
                game.Run();

                Console.Out.WriteLine("Всё, пока!");
                Console.In.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}