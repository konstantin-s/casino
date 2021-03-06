using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using prism_app.Events;
using prism_app.Views;

namespace prism_app.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "";

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private Visibility _coolVisibility = System.Windows.Visibility.Collapsed;

        public Visibility CoolVisibility
        {
            get => _coolVisibility;
            set => SetProperty(ref _coolVisibility, value);
        }

        public ObservableCollection<string> CoolList { get; set; }

        private string _coolInputText = "";

        public string CoolInputText
        {
            get => _coolInputText;
            set => SetProperty(ref _coolInputText, value);
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                SetProperty(ref _isEnabled, value);
                ExecuteDelegateCommand.RaiseCanExecuteChanged();
            }
        }

        private string _updateText;

        public string UpdateText
        {
            get { return _updateText; }
            set { SetProperty(ref _updateText, value); }
        }

        private string _debugText;

        public string DebugText
        {
            get => _debugText;
            set => SetProperty(ref _debugText, value);
        }

        private AppLog _logger;
        private string _appLog;

        public string AppLog
        {
            get => _appLog;
            set => SetProperty(ref _appLog, value);
        }

        public DelegateCommand CoolCommand { get; private set; }
        public DelegateCommand CoolCommandY { get; private set; }
        public DelegateCommand CoolCommandN { get; private set; }
        public DelegateCommand ExecuteDelegateCommand { get; private set; }

        public DelegateCommand<string> ExecuteGenericDelegateCommand { get; private set; }

        public DelegateCommand DelegateCommandObservesProperty { get; private set; }

        public DelegateCommand DelegateCommandObservesCanExecute { get; private set; }

        IEventAggregator _eventAggregator;

        IContainerExtension _container;
        IRegionManager _regionManager;
        IRegion _region;
        Game _game;

        ViewA _viewA;
        ViewB _viewB;
        Start _start;

        public MainWindowViewModel(AppLog logger, IEventAggregator ea, IContainerExtension container, IRegionManager regionManager, Game game)
        {
            _logger = logger;
            _logger.SetTrackProp(this, "AppLog");

            _eventAggregator = ea;
            _container = container;
            _regionManager = regionManager;
            _game = game;

            Title = _game.GetTitle();

            _logger.Log("call " + System.Reflection.MethodBase.GetCurrentMethod().Name);

            ea.GetEvent<GameStateChangeEvent>().Subscribe((payload) =>
            {
                _logger.Log($"GameStateChangeEvent {payload}");
                if (payload == GameState.Identificate)
                {
                    _logger.Log($"☺ activating ViewA");
                    _region.Activate(_viewA);
                }

                if (payload == GameState.Play)
                {
                    _logger.Log($"☺ GameState.Play={GameState.Play} activating ViewB");
                    _viewB = _container.Resolve<ViewB>();
                    _region.Add(_viewB);

                    _region.Activate(_viewB);
                    _logger.Log($"☺ activatED ViewB");
                }
            });

            CoolList = new ObservableCollection<string>();
            CoolList.Add("Add @ MainWindowViewModel");

            CoolCommand = new DelegateCommand(ExecuteCoolCommand);
            CoolCommandY = new DelegateCommand(ExecuteCoolCommandY);
            CoolCommandN = new DelegateCommand(ExecuteCoolCommandN);

            ExecuteDelegateCommand = new DelegateCommand(Execute, CanExecute);

            DelegateCommandObservesProperty = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => IsEnabled);

            DelegateCommandObservesCanExecute = new DelegateCommand(Execute).ObservesCanExecute(() => IsEnabled);

            ExecuteGenericDelegateCommand = new DelegateCommand<string>(ExecuteGeneric).ObservesCanExecute(() => IsEnabled);
        }


        public void MainWindow_Loaded()
        {
            _logger.Log(System.Reflection.MethodBase.GetCurrentMethod().Name + "  @ " + this.GetType().Name);

            _start = _container.Resolve<Start>();
            _viewA = _container.Resolve<ViewA>();

            _region = _regionManager.Regions["ContentRegion"];

            _region.Add(_start);
            _region.Add(_viewA);
        }

        private void ExecuteCoolCommand()
        {
            DebugText += $"ExecuteCoolCommand call \n";
            CoolVisibility = System.Windows.Visibility.Visible;
        }

        private void ExecuteCoolCommandY()
        {
            DebugText += $"ExecuteCoolCommandY call \n";
            YesButton_Click();
        }

        private void ExecuteCoolCommandN()
        {
            DebugText += $"ExecuteCoolCommandN call \n";
            NoButton_Click();
        }


        private void CoolButton_Click()
        {
            // CoolButton Clicked! Let's show our InputBox.
            CoolVisibility = System.Windows.Visibility.Visible;
        }

        private void YesButton_Click()
        {
            // YesButton Clicked! Let's hide our InputBox and handle the input text.
            CoolVisibility = System.Windows.Visibility.Collapsed;

            // Do something with the Input
            CoolList.Add(CoolInputText);
            CoolInputText = "";
            // MyListBox.Items.Add(input); // Add Input to our ListBox.

            // Clear InputBox.
            // InputTextBox.Text = String.Empty;
        }

        private void NoButton_Click()
        {
            // NoButton Clicked! Let's hide our InputBox.
            CoolVisibility = System.Windows.Visibility.Collapsed;
            CoolInputText = "";

            // Clear InputBox.
            // InputTextBox.Text = String.Empty;
        }

        private void Execute()
        {
            UpdateText = $"Updated: {DateTime.Now}";
        }

        private void ExecuteGeneric(string parameter)
        {
            UpdateText = parameter;
        }

        private bool CanExecute()
        {
            return IsEnabled;
        }
    }
}