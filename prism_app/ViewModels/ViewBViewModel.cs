using Prism.Commands;
using Prism.Mvvm;

namespace prism_app.ViewModels
{
    public class ViewBViewModel : BindableBase
    {
        #region Infos

        public string WinMult { get; private set; }
        public string RangeTo { get; private set; }
        public string RangeFrom { get; private set; }
        public string PlayerName { get; private set; }
        public int BalanceValue { get; private set; }

        #endregion

        #region UserInputs

        private int _playerStake;
        private int _playerNumber;

        public int PlayerStake
        {
            get => _playerStake;
            set => SetProperty(ref _playerStake, value);
        }

        public int PlayerNumber
        {
            get => _playerNumber;
            set => SetProperty(ref _playerNumber, value);
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
            _game.DoRoll();
        }

        private readonly Game _game;
        private readonly AppLog _logger;

        public ViewBViewModel(Game game, AppLog logger)
        {
            _game = game;
            _logger = logger;

            PlayerName = game.Player.Name;
            BalanceValue = game.Player.Balance.Value;

            RangeFrom = Constants.RangeFrom.ToString();
            RangeTo = Constants.RangeTo.ToString();
            WinMult = Constants.WinMult.ToString();

            DoRoll = new DelegateCommand(ExecuteDoRoll);

            IsStakesAllowed = _game.CanStake();
            IsRollAllowed = _game.CanRoll();
            
            _logger.Log("ViewBViewModel constructor");
            _logger.Log($"RangeFrom: {RangeFrom}, RangeTo: {RangeTo}, WinMult: {WinMult}");
        }
    }
}