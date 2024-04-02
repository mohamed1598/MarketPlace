using MarketPlace.Framework;

namespace MarketPlace.Domain
{
    public class Money : Value<Money>
    {
        public const string DefaultCurrency = "EUR";
        public static Money FromDecimal(decimal amount,ICurrencyLookup currencyLookup, string currency = DefaultCurrency) =>
            new Money(amount, currencyLookup, currency);
        public static Money FromString(string amount,ICurrencyLookup currencyLookup, string currency = DefaultCurrency) =>
            new Money(decimal.Parse(amount), currencyLookup, currency);
        protected Money(decimal amount,ICurrencyLookup currencyLookup, string currencyCode = DefaultCurrency)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(currencyCode,"Currency Code must be specified");
            var currency = currencyLookup.FindCurrency(currencyCode);
            if (!currency.InUse)
                throw new ArgumentException($"Currency {currencyCode} is not valid");
            ArgumentOutOfRangeException.ThrowIfNotEqual(decimal.Round(amount, 2), amount
                , "Amount cannot have more than two decimals");
            Amount = amount;
            Currency = currency;
        }
        protected Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }
        public decimal Amount { get; }
        public Currency Currency { get; }

        public Money Add(Money Summand)
        {
            CurrencyMismatchException.ThrowIfNotEqual(Currency, Summand.Currency,"Cannot sum amounts with different currencies");
            return new Money(Amount + Summand.Amount,Currency);
        }
        public Money Subtract(Money subtrahend)
        {
            CurrencyMismatchException.ThrowIfNotEqual(Currency, subtrahend.Currency,"Cannot subtract amounts with different currencies");
            return new Money(Amount - subtrahend.Amount, Currency);
        }
        public static Money operator +(Money lhs, Money rhs) => lhs.Add(rhs);
        public static Money operator -(Money lhs, Money rhs) => lhs.Subtract(rhs);
    }
    public class CurrencyMismatchException(string message) : Exception(message)
    {
        public static void ThrowIfNotEqual<T>(T currency, T otherCurrency, string message) where T : Value<T>
        {
            if(currency != otherCurrency)
                throw new CurrencyMismatchException(message);
        }
    }
}
