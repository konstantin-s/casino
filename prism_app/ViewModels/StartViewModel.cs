using Prism.Mvvm;
using Prism.Commands;

namespace prism_app.ViewModels
{
    public class StartViewModel : BindableBase
    {
        private readonly Game _game;
        private AppLog _logger;
        
        private string _title = "";

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public StartViewModel(Game game, AppLog logger)
        {
            _game = game;
            _logger = logger;
            
            Title = _game.GetTitle();
            
            _logger.Log($@"StartViewModel HERE ☺!");
        }

        private DelegateCommand _fieldName;

        public DelegateCommand Enter =>
            _fieldName ?? (_fieldName = new DelegateCommand(ExecuteEnter));

        void ExecuteEnter()
        {
            _logger.Log("ExecuteEnter call");
            _game.Enter();
        }
    }
}