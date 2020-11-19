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
        IContainerExtension _container;
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;

        public Game(IContainerExtension container, IRegionManager regionManager, IEventAggregator ea)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = ea;
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

        public void Identificated()
        {
            State = GameState.Play;
            _eventAggregator.GetEvent<GameStateChangeEvent>().Publish(State);
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