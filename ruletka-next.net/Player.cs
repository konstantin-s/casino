namespace ruletka_next.net
{
    public class Player
    {
        public Balance balance;
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
            balance = new Balance(Constants.StartBalance);
        }
    }
}