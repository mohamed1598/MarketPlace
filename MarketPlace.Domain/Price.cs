namespace MarketPlace.Domain
{
    public class Price : Money
    {
        public Price(decimal amount,ICurrencyLookup currencyLookup,string currencyCode="EUR") : base(amount,currencyLookup,currencyCode)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan<decimal>(amount,0,"Price cannot be negative");
        }
        internal Price(decimal amount, string currencyCode)
            : base(amount, new Currency { CurrencyCode = currencyCode })
        {
        }

        public new static Price FromDecimal(decimal amount,ICurrencyLookup currencyLookup, string currency) =>
            new (amount, currencyLookup, currency);
    }
}