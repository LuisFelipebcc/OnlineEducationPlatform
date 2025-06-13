using System.Text.RegularExpressions;

namespace PaymentBilling.Domain.ValueObjects
{
    public class CardData
    {
        public string CardNumber { get; }
        public string CardholderName { get; }
        public string ExpirationDate { get; }
        public string SecurityCode { get; }

        public CardData()
        {
            CardNumber = string.Empty;
            CardholderName = string.Empty;
            ExpirationDate = string.Empty;
            SecurityCode = string.Empty;
        }

        public CardData(
            string cardNumber,
            string cardholderName,
            string expirationDate,
            string securityCode)
        {
            if (!ValidateCardNumber(cardNumber))
                throw new ArgumentException("Invalid card number", nameof(cardNumber));

            if (string.IsNullOrWhiteSpace(cardholderName))
                throw new ArgumentException("Cardholder name is required", nameof(cardholderName));

            if (!ValidateExpirationDate(expirationDate))
                throw new ArgumentException("Invalid expiration date", nameof(expirationDate));

            if (!ValidateSecurityCode(securityCode))
                throw new ArgumentException("Invalid security code", nameof(securityCode));

            CardNumber = MaskCardNumber(cardNumber);
            CardholderName = cardholderName;
            ExpirationDate = expirationDate;
            SecurityCode = securityCode;
        }

        public bool IsValid()
        {
            return ValidateCardNumber(CardNumber) &&
                   !string.IsNullOrWhiteSpace(CardholderName) &&
                   ValidateExpirationDate(ExpirationDate) &&
                   ValidateSecurityCode(SecurityCode);
        }

        private bool ValidateCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            // Remove spaces and dashes
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            // Check if contains only numbers
            if (!Regex.IsMatch(cardNumber, @"^\d+$"))
                return false;

            // Check if has between 13 and 19 digits
            if (cardNumber.Length < 13 || cardNumber.Length > 19)
                return false;

            return true;
        }

        private bool ValidateExpirationDate(string expirationDate)
        {
            if (string.IsNullOrWhiteSpace(expirationDate))
                return false;

            // Expected format: MM/YY
            if (!Regex.IsMatch(expirationDate, @"^(0[1-9]|1[0-2])/([0-9]{2})$"))
                return false;

            var parts = expirationDate.Split('/');
            var month = int.Parse(parts[0]);
            var year = int.Parse(parts[1]);

            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year % 100;
            var currentMonth = currentDate.Month;

            if (year < currentYear || (year == currentYear && month < currentMonth))
                return false;

            return true;
        }

        private bool ValidateSecurityCode(string securityCode)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
                return false;

            // Check if contains only numbers
            if (!Regex.IsMatch(securityCode, @"^\d+$"))
                return false;

            // Check if has 3 or 4 digits
            if (securityCode.Length < 3 || securityCode.Length > 4)
                return false;

            return true;
        }

        private string MaskCardNumber(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");
            return $"**** **** **** {cardNumber.Substring(cardNumber.Length - 4)}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CardData other)
                return false;

            return CardNumber == other.CardNumber &&
                   CardholderName == other.CardholderName &&
                   ExpirationDate == other.ExpirationDate &&
                   SecurityCode == other.SecurityCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                CardNumber,
                CardholderName,
                ExpirationDate,
                SecurityCode
            );
        }
    }
}