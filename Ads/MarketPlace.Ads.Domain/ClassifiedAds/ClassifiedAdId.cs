using MarketPlace.Framework;
using System;

namespace MarketPlace.Ads.Domain.ClassifiedAds
{
    public class ClassifiedAdId : AggregateId<ClassifiedAd>
    {
        public ClassifiedAdId(Guid value) : base(value) { }

        public static ClassifiedAdId FromGuid(Guid value)
            => new ClassifiedAdId(value);
    }
}