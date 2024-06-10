
using Raven.Client.Documents.Session;
using static MarketPlace.ClassifiedAd.QueryModels;
using static MarketPlace.Projections.ReadModels;

namespace MarketPlace.ClassifiedAd
{
    public static class Queries
    {
        public static Task<ClassifiedAdDetails> Query(
            this IAsyncDocumentSession session,
            GetPublicClassifiedAd query
        ) => session.LoadAsync<ClassifiedAdDetails>(
                query.ClassifiedAdId.ToString()
            );
    }
}
