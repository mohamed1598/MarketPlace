using MarketPlace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Tests
{
    public class ClassifiedAd_Publish_Spec
    {
            private readonly ClassifiedAd _classifiedAd;
            public ClassifiedAd_Publish_Spec()
            {
                _classifiedAd = new ClassifiedAd(
                new ClassifiedAdId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()));
            }
            [Fact]
            public void Can_publish_a_valid_ad()
            {
                _classifiedAd.SetTitle(
                ClassifiedAdTitle.FromString("Test ad"));
                _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Please buy my stuff"));
                _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, new FakeCurrencyLookup(), "EUR"));
                _classifiedAd.RequestToPublish();
                Assert.Equal(ClassifiedAd.ClassifiedAdState.PendingReview,
                _classifiedAd.State);
            }
            [Fact]
            public void Cannot_publish_without_title()
            {
                _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Please buy my stuff"));
                _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m,new FakeCurrencyLookup(), "EUR"));
                Assert.Throws<InvalidEntityStateException>(() =>
                _classifiedAd.RequestToPublish());
            }
            [Fact]
            public void Cannot_publish_without_text()
            {
                _classifiedAd.SetTitle(
                ClassifiedAdTitle.FromString("Test ad"));
                _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m,new FakeCurrencyLookup(), "EUR"));
                Assert.Throws<InvalidEntityStateException>(
                () => _classifiedAd.RequestToPublish());
            }
            [Fact]
            public void Cannot_publish_without_price()
            {
                _classifiedAd.SetTitle(
                ClassifiedAdTitle.FromString("Test ad"));
                _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Please buy my stuff"));
                Assert.Throws<InvalidEntityStateException>(
                () => _classifiedAd.RequestToPublish());
            }
            [Fact]
            public void Cannot_publish_with_zero_price()
            {
                _classifiedAd.SetTitle(
                ClassifiedAdTitle.FromString("Test ad"));
                _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Please buy my stuff"));
                _classifiedAd.UpdatePrice(
                Price.FromDecimal(0.0m,new FakeCurrencyLookup(), "EUR"));
                Assert.Throws<InvalidEntityStateException>(
                () => _classifiedAd.RequestToPublish());
            }
        }
    }
