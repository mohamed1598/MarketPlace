using MarketPlace.Infrastructure;
using Raven.Client.Documents.Session;
using static MarketPlace.ClassifiedAd.Commands;
using static MarketPlace.Domain.ClassifiedAd.Events;
using static MarketPlace.Domain.UserProfile.Events;
using static MarketPlace.Projections.ReadModels;

namespace MarketPlace.Projections
{
    public class ClassifiedAdDetailsProjection:RavenDbProjection<ClassifiedAdDetails>
    {
        private readonly Func<Guid, Task<string>> _getUserDisplayName;

        public ClassifiedAdDetailsProjection(IAsyncDocumentSession session, Func<Guid, Task<string>> getUserDisplayName)
            :base(session) 
        {
            _getUserDisplayName = getUserDisplayName;
        }

        public override Task Project(object @event)
        {
            return @event switch
            {
                ClassifiedAdCreated e =>
                    Create(async () =>
                        new ClassifiedAdDetails
                        {
                            Id = e.Id.ToString(),
                            SellerId = e.OwnerId,
                            SellersDisplayName = await _getUserDisplayName(e.OwnerId)
                        }),
                ClassifiedAdTitleChanged e =>
                   UpdateOne(e.Id, ad => ad.Title = e.Title),
                ClassifiedAdTextUpdated e => UpdateOne(e.Id, ad => ad.Description = e.AdText),
                ClassifiedAdPriceUpdated e => UpdateOne(e.Id, ad =>
                    {
                        ad.Price = e.Price;
                        ad.CurrencyCode = e.CurrencyCode;
                    }),
                UserDisplayNameUpdated e=> 
                    UpdateWhere(
                        x=> x.SellerId == e.UserId,
                        x => x.SellersDisplayName = e.DisplayName
                    ),
                ClassifiedAdUpcastedEvents.V1.ClassifiedAdPublished e =>
                    UpdateOne(
                        e.Id,
                        ad => ad.SellersPhotoUrl = e.SellersPhotoUrl
                    ),
                _ => Task.CompletedTask
            };
        }
    }
}

