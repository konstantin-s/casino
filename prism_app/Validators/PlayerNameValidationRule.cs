using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace prism_app.Validators
{
    public class PlayerNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringData = value as string;
            if (stringData == null)
            {
                return new ValidationResult(false, "Имя не может быть пустым");
            }

            stringData = stringData.Trim();

            Regex rgx = new Regex(@"^[a-zA-Zа-яА-Я0-9_\-\.\s☺]+$");
            if (!rgx.IsMatch(stringData))
            {
                return new ValidationResult(false, "Имя должно состоять из букв, цифр, знаков _-☺");
            }

            if (stringData.Length < Constants.PlayerNameMin)
            {
                return new ValidationResult(false, $"Имя должно быть не короче {Constants.PlayerNameMin} символов");
            }

            if (stringData.Length > 33)
            {
                return new ValidationResult(false, "Имя должно быть не длиннее 33 символов");
            }

            return ValidationResult.ValidResult;
        }
    }
}