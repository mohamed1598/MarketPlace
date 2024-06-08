using Marketplace.Framework;
using ClassifiedAdDomain = MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Shared;
using static MarketPlace.ClassifiedAd.Commands;
using MarketPlace.Framework;

namespace MarketPlace.ClassifiedAd
{
    public class ClassifiedAdsApplicationService : IApplicationService
    {
        private readonly IAggregateStore _store;
        private readonly ICurrencyLookup _currencyLookup;

        public ClassifiedAdsApplicationService( IAggregateStore store,
            ICurrencyLookup currencyLookup
        )
        {
            _store = store;
            _currencyLookup = currencyLookup;
        }

        public Task Handle(object command) =>
            command switch
            {
                V1.Create cmd => HandleCreate(cmd),
                V1.SetTitle cmd =>
                    HandleUpdate(
                        cmd.Id,
                        c => c.SetTitle(
                            ClassifiedAdDomain.ClassifiedAdTitle.FromString(cmd.Title)
                        )
                    ),
                V1.UpdateText cmd =>
                    HandleUpdate(
                        cmd.Id,
                        c => c.UpdateText(
                            ClassifiedAdDomain.ClassifiedAdText.FromString(cmd.Text)
                        )
                    ),
                V1.UpdatePrice cmd =>
                    HandleUpdate(
                        cmd.Id,
                        c => c.UpdatePrice(
                            ClassifiedAdDomain.Price.FromDecimal(
                                cmd.Price,
                                cmd.Currency,
                                _currencyLookup
                            )
                        )
                    ),
                V1.RequestToPublish cmd =>
                    HandleUpdate(
                        cmd.Id,
                        c => c.RequestToPublish()
                    ),
                _ => Task.CompletedTask
            };

        private async Task HandleCreate(V1.Create cmd)
        {
            if (await _store.Exists<ClassifiedAdDomain.ClassifiedAd, ClassifiedAdDomain.ClassifiedAdId>(cmd.Id.ToString()))
                throw new InvalidOperationException(
                    $"Entity with id {cmd.Id} already exists"
                );

            var classifiedAd = new ClassifiedAdDomain.ClassifiedAd(
                new ClassifiedAdDomain.ClassifiedAdId(cmd.Id),
                new UserId(cmd.OwnerId)
            );

            await _store.Save<ClassifiedAdDomain.ClassifiedAd, ClassifiedAdDomain.ClassifiedAdId>(classifiedAd);
        }

        private async Task HandleUpdate(
            Guid classifiedAdId,
            Action<ClassifiedAdDomain.ClassifiedAd> update
        )
        {
            this.HandleUpdate(_store, new ClassifiedAdDomain.ClassifiedAdId(classifiedAdId), update);
        }
    }
}
