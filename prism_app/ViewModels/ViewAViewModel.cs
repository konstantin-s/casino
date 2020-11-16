using Prism.Mvvm;
using System;
using Prism.Commands;
using Prism.Mvvm;

namespace prism_app.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        public ViewAViewModel(AppLog logger)
        {
            _logger = logger;
            _logger.Log($@"ViewAViewModel HERE ☺!");
        }

        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set { SetProperty(ref _playerName, value); }
        }

        private DelegateCommand _fieldName;
        private AppLog _logger;

        public DelegateCommand Identificate =>
            _fieldName ?? (_fieldName = new DelegateCommand(ExecuteIdentificate));

        void ExecuteIdentificate()
        {
            _logger.Log("ExecuteIdentificate call");
        }
    }
}