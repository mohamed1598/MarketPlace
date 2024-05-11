//using Marketplace.Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit.Sdk;

//namespace MarketPlace.Tests
//{
//    public class PriceTest
//    {
//        private static readonly ICurrencyLookup CurrencyLookup =
//            new FakeCurrencyLookup();
//        [Fact]
//        public void Price_objects_with_the_same_amount_should_be_equal()
//        {
//            var firstAmount = new Price(5,CurrencyLookup);
//            var secondAmount = new Price(5, CurrencyLookup);
//            Assert.Equal(firstAmount, secondAmount);
//        }
//        [Fact]
//        public void Check_price_objects_with_the_Negatives_Amount()
//        {
//            try
//            {
//                var firstAmount = new Price(-5, CurrencyLookup);
//                Assert.False(true);
//            }
//            catch (ArgumentOutOfRangeException) {
//                Assert.True(true);
//            }

//        }
//    }
//}
