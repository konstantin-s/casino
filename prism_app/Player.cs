namespace prism_app
{
    public class Player
    {
        public string Name { get; }
        public Balance Balance;

        public Player(string name)
        {
            Name = name;
            Balance = new Balance(Constants.StartBalance);
        }
    }
}