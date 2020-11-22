using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using prism_app.Validators;

namespace prism_app.ViewModels
{
    public class ViewBViewModel : BindableBase, INotifyDataErrorInfo
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

        private void ValidatePlayerStake()
        {
            string varName = nameof(PlayerStake);
            ClearErrors(varName);

            try
            {
                int check = Convert.ToInt32(PlayerStake);
            }
            catch (OverflowException)
            {
                AddError(varName, "слишком большое число");
            }
            catch (FormatException)
            {
                AddError(varName, "это не число");
            }

            if (PlayerStake > _game.Player.Balance.Value)
            {
                AddError(varName, "на балансе столько нет");
            }

            if (PlayerStake < Constants.StakeMin)
            {
                AddError(varName, $"не может быть менее {Constants.StakeMin}");
            }

            IsRollAllowed = !HasErrors;
        }

        private void ValidatePlayerNumber()
        {
            string varName = nameof(PlayerNumber);
            ClearErrors(varName);

            try
            {
                int check = Convert.ToInt32(PlayerNumber);
            }
            catch (OverflowException)
            {
                AddError(varName, "слишком большое число");
            }
            catch (FormatException)
            {
                AddError(varName, "это не число");
            }

            if (PlayerNumber > Constants.RangeTo || PlayerNumber < Constants.RangeFrom)
            {
                AddError(varName, $"число должно быть от {Constants.RangeFrom} до {Constants.RangeTo}");
            }

            IsRollAllowed = !HasErrors;
        }

        #endregion

        #region Infos

        public string WinMult { get; private set; }
        public string RangeTo { get; private set; }
        public string RangeFrom { get; private set; }
        public string PlayerName { get; private set; }

        private int _balanceValue;
        private int _progress;

        public int BalanceValue
        {
            get => _balanceValue;
            set => SetProperty(ref _balanceValue, value);
        }

        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        #endregion

        #region UserInputs

        private int _playerStake;
        private int _playerNumber;

        public int PlayerStake
        {
            get => _playerStake;
            set
            {
                SetProperty(ref _playerStake, value);
                ValidatePlayerStake();
            }
        }

        public int PlayerNumber
        {
            get => _playerNumber;
            set
            {
                SetProperty(ref _playerNumber, value);
                ValidatePlayerNumber();
            }
        }

        #endregion

        #region Bools

        private bool _isStakesAllowed;
        private bool _isRollAllowed;
        private bool _isSpinning;

        public bool IsStakesAllowed
        {
            get => _isStakesAllowed;
            set => SetProperty(ref _isStakesAllowed, value);
        }


        public bool IsRollAllowed
        {
            get => _isRollAllowed;
            set => SetProperty(ref _isRollAllowed, value);
        }


        public bool IsSpinning
        {
            get => _isSpinning;
            set => SetProperty(ref _isSpinning, value);
        }

        #endregion

        public DelegateCommand DoRoll { get; private set; }

        void ExecuteDoRoll()
        {
            _logger.Log("Command ExecuteDoRoll call");
            IsSpinning = true;
            IsRollAllowed = false;
            IsStakesAllowed = false;

            StarRolltProgressEmulation();
        }

        public ObservableCollection<GameHistoryItem> GameHistory { get; set; }

        private readonly Game _game;
        private readonly AppLog _logger;

        public ViewBViewModel(Game game, AppLog logger)
        {
            _game = game;
            _logger = logger;

            PlayerName = _game.Player.Name;
            BalanceValue = _game.Player.Balance.Value;
            GameHistory = _game.GameHistory;

            RangeFrom = Constants.RangeFrom.ToString();
            RangeTo = Constants.RangeTo.ToString();
            WinMult = Constants.WinMult.ToString();

            DoRoll = new DelegateCommand(ExecuteDoRoll);

            IsStakesAllowed = _game.CanStake();
            IsRollAllowed = _game.CanRoll();

            _logger.Log("ViewBViewModel constructor");
            _logger.Log($"RangeFrom: {RangeFrom}, RangeTo: {RangeTo}, WinMult: {WinMult}");
        }

        private void StarRolltProgressEmulation()
        {
            _logger.Log("StarRolltProgressEmulation call");
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += RollEnded;

            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IsSpinning = true;
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(10);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }

        void RollEnded(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            _game.DoRoll(PlayerNumber, PlayerStake);

            IsSpinning = false;
            Progress = 0;

            BalanceValue = _game.Player.Balance.Value;
            PlayerStake = _game.Stake;
            PlayerNumber = _game.Number;
            IsStakesAllowed = true;
        }
    }
}