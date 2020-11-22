using System;
using System.ComponentModel;
using Unity;
using Prism.Regions;
using System.Windows;
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

        public void DoRoll()
        {
            _logger.Log("Game DoRoll call");
        }

        public bool CanStake()
        {
            return true;
        }

        public bool CanRoll()
        {
            return true;
        }
    }

    public enum GameState
    {
        Welcome,
        Identificate,
        Play,
        End
    }
}