using System.Text.RegularExpressions;

namespace lab30v4
{
    public class Validator
    {
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        // Перевірка на валідність номера телефону
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            string pattern = @"^\+?\d{10,15}$";
            return Regex.IsMatch(phone, pattern);
        }

        public bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (password.Length < 8) return false;

            bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
            bool hasLower = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"[0-9]");
            bool hasSpecial = Regex.IsMatch(password, @"[\W_]");

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}