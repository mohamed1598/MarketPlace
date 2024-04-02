using MarketPlace.Domain;
using Xunit;

namespace MarketPlace.Tests
{
    public class MoneyTest
    {
        private static readonly ICurrencyLookup CurrencyLookup =
 new FakeCurrencyLookup();
        [Fact]
        public void Two_of_same_amount_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, CurrencyLookup, "EUR");
            var secondAmount = Money.FromDecimal(5, CurrencyLookup, "EUR");
            Assert.Equal(firstAmount, secondAmount);
        }
        [Fact]
        public void Two_of_same_amount_but_differentCurrencies_should_not_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, CurrencyLookup, "EUR");
            var secondAmount = Money.FromDecimal(5,CurrencyLookup, "USD");
            Assert.NotEqual(firstAmount, secondAmount);
        }
        [Fact]
        public void FromString_and_FromDecimal_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5,CurrencyLookup, "EUR");
            var secondAmount = Money.FromString("5.00", CurrencyLookup, "EUR");
            Assert.Equal(firstAmount, secondAmount);
        }
        [Fact]
        public void Sum_of_money_gives_full_amount()
        {
            var coin1 = Money.FromDecimal(1, CurrencyLookup, "EUR");
            var coin2 = Money.FromDecimal(2, CurrencyLookup, "EUR");
            var coin3 = Money.FromDecimal(2, CurrencyLookup, "EUR");
            var banknote = Money.FromDecimal(5, CurrencyLookup, "EUR");
            Assert.Equal(banknote, coin1 + coin2 + coin3);
        }
        [Fact]
        public void Unused_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() =>
            Money.FromDecimal(100, CurrencyLookup, "DEM")
            );
        }
        [Fact]
        public void Unknown_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() =>
            Money.FromDecimal(100, CurrencyLookup, "WHAT?")
            );
        }
        [Fact]
        public void Throw_when_too_many_decimal_places()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            Money.FromDecimal(100.123m, CurrencyLookup, "EUR")
            );
        }
        [Fact]
        public void Throws_on_adding_different_currencies()
        {
            var firstAmount = Money.FromDecimal(5,CurrencyLookup, "USD");
            var secondAmount = Money.FromDecimal(5,CurrencyLookup, "EUR");
            Assert.Throws<CurrencyMismatchException>(() =>
                firstAmount + secondAmount
            );
        }
        [Fact]
        public void Throws_on_substracting_different_currencies()
        {
            var firstAmount = Money.FromDecimal(5,CurrencyLookup, "USD");
            var secondAmount = Money.FromDecimal(5,CurrencyLookup, "EUR");
            Assert.Throws<CurrencyMismatchException>(() =>
            firstAmount - secondAmount
       );
    }
  }
}
