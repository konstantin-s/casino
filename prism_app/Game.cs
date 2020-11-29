using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Regions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using prism_app.Events;

namespace prism_app
{
    //TODO переделать
    public class Game : BindableBase
    {
        private Player _player;

        public Player Player
        {
            get => _player;
            private set => _player = value;
        }

        IContainerExtension _container;
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;
        private readonly AppLog _logger;

        public Game(IContainerExtension container, IRegionManager regionManager, IEventAggregator ea, AppLog logger)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = ea;
            _logger = logger;
        }

        private GameState _state;

        public GameState State
        {
            get => _state;
            private set => SetProperty(ref _state, value);
        }

        public void Enter()
        {
            State = GameState.Identificate;
            _eventAggregator.GetEvent<GameStateChangeEvent>().Publish(State);
        }

        public void Identificated(string playerName)
        {
            Player = new Player(playerName);

            State = GameState.Play;
            _eventAggregator.GetEvent<GameStateChangeEvent>().Publish(State);
        }

        public int Stake { get; set; }
        public int Number { get; set; }

        public int LastStake = 0;
        public int LastSecretNumber = 0;
        public int LastPlayerNumber = 0;
        public GameResult LastResult;

        public void DoRoll(int playerNumber, int playerStake)
        {
            Number = playerNumber;
            Stake = playerStake;

            _logger.Log("Game DoRoll call");

            _player.Balance.Value -= Stake;

            GameResult result = GameResult.Loose;
            int winAmount = 0;

            var secretNumber = new Random().Next(Constants.RangeFrom, Constants.RangeTo);

            if (secretNumber == Number)
            {
                winAmount = Stake * Constants.WinMult;
                result = GameResult.Win;
            }

            _player.Balance.Value += winAmount;

            LastSecretNumber = secretNumber;
            LastPlayerNumber = Number;
            LastStake = Stake;
            LastResult = result;

            GameHistory.Insert(0, new GameHistoryItem(Number, secretNumber, Stake, _player.Balance.Value, result, winAmount));

            _logger.Log(
                $"LastPlayerNumber: {LastPlayerNumber}, LastSecretNumber: {LastSecretNumber}, LastStake: {LastStake}, LastResult: {LastResult.ToString()}");

            if (Stake > _player.Balance.Value)
            {
                Stake = 0;
            }

            Number = 0;

            _logger.Log($"Game DoRoll end. Stake: {Stake}, Number: {Number}");

            if (_player.Balance.Value <= 0)
            {
                EndGame();
            }
        }

        public void EndGame()
        {
            _logger.Log($"Игра окончена: на балансе {_player.Balance.Value} рублей");
            State = GameState.End;
            _eventAggregator.GetEvent<GameStateChangeEvent>().Publish(State);
        }

        public bool CanStake()
        {
            return _player.Balance.Value > 0;
        }

        public bool CanRoll()
        {
            return Stake > 0 && Number > 0;
        }

        public ObservableCollection<GameHistoryItem> GameHistory = new ObservableCollection<GameHistoryItem>();

        public void Restart()
        {
            //Todo Переделать чтобы событие не перезагружало вид, а только рестартовало игру на этом виде
            Player.Balance.Value = Constants.StartBalance;
            GameHistory.Clear();
            State = GameState.Play;
            _eventAggregator.GetEvent<GameStateChangeEvent>().Publish(State);
        }

        public bool IsStakeAllowed(int playerStake)
        {
            return playerStake <= _player.Balance.Value && playerStake > Constants.StakeMin;
        }

        public bool IsNumberAllowed(int playerNumber)
        {
            return playerNumber <= Constants.RangeTo && playerNumber >= Constants.RangeFrom;
        }
    }


    public readonly struct GameHistoryItem
    {
        public string DT { get; }
        public int Number { get; }
        public int SecretNumber { get; }
        public int Stake { get; }
        public int BalanceValue { get; }
        public GameResult Result { get; }
        public int WinAmount { get; }

        public GameHistoryItem(int number, int secretNumber, int stake, int balanceValue, GameResult result, int winAmount)
        {
            DT = DateTime.Now.ToString("u");
            Number = number;
            SecretNumber = secretNumber;
            Stake = stake;
            BalanceValue = balanceValue;
            Result = result;
            WinAmount = winAmount;
        }
    }

    public enum GameState
    {
        Welcome,
        Identificate,
        Play,
        End
    }

    public enum GameResult
    {
        Win,
        Loose
    }
}