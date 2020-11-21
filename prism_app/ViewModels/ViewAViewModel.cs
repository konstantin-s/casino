using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using Prism.Commands;
using prism_app.Validators;

namespace prism_app.ViewModels
{
    public class ViewAViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region ERROR HANDLING

        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ValidateUserName()
        {
            IsIdentificateAllowed = false;

            ClearErrors(nameof(PlayerName));

            PlayerNameValidationRule playerNameValidationRule = new PlayerNameValidationRule();

            ValidationResult validationResult = playerNameValidationRule.Validate(PlayerName, CultureInfo.CurrentCulture);
            if (!validationResult.IsValid)
            {
                AddError(nameof(PlayerName), validationResult.ErrorContent.ToString());
            }

            IsIdentificateAllowed = !HasErrors;
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        #endregion

        private readonly Game _game;
        private readonly AppLog _logger;

        public ViewAViewModel(Game game, AppLog logger)
        {
            _game = game;
            _logger = logger;

            _logger.Log(@"ViewAViewModel HERE ☺!");
        }


        private bool _isIdentificateAllowed;

        public bool IsIdentificateAllowed
        {
            get => _isIdentificateAllowed;
            set => SetProperty(ref _isIdentificateAllowed, value);
        }

        private string _playerName;

        public string PlayerName
        {
            get => _playerName;

            set
            {
                SetProperty(ref _playerName, value);
                ValidateUserName();
            }
        }

        private DelegateCommand _identificateCommand;

        public DelegateCommand DoIdentificate
        {
            get { return _identificateCommand ?? (_identificateCommand = new DelegateCommand(ExecuteIdentificate)); }
        }

        void ExecuteIdentificate()
        {
            _logger.Log("ExecuteIdentificate call");
            _game.Identificated();
        }
    }
}