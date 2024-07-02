using EventStore.Client;
using MarketPlace.Ads.ClassifiedAds;
using MarketPlace.Ads.Domain.Shared;
using MarketPlace.Ads.Projections;
using MarketPlace.EventStore;
using MarketPlace.RavenDb;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarketPlace.Ads.Projections.ReadModels;

namespace MarketPlace.Ads
{
    public static class AdsModule
    {
        const string SubscriptionName = "adsSubscription";

        public static IServiceCollection AddAdsModule(
            this IServiceCollection services,
            string databaseName,
            ICurrencyLookup currencyLookup,
            UploadFile uploadFile
        )
        {

            services.AddSingleton(
                c =>
                    new ClassifiedAdsCommandService(
                        new EsAggregateStore(c.GetEsConnection()),
                        currencyLookup,
                        uploadFile
                    )
            );

            services.AddSingleton(
                c =>
                {
                    var store = c.GetRavenStore();
                    store.CheckAndCreateDatabase(databaseName);

                    IAsyncDocumentSession session = store
                            .OpenAsyncSession(databaseName);

                    return new SubscriptionManager(
                        c.GetEsConnection(),
                        new RavenDbCheckpointStore(
                            session, SubscriptionName
                        ),
                        SubscriptionName,
                        new RavenDbProjection<ClassifiedAdDetails>(
                            session,
                            ClassifiedAdDetailsProjection.GetHandler
                        ),
                        new RavenDbProjection<MyClassifiedAds>(
                            session,
                            MyClassifiedAdsProjection.GetHandler
                        )
                    );
                }
            );

            return services;
        }

        static IDocumentStore GetRavenStore(
            this IServiceProvider provider
        )
            => provider.GetRequiredService<IDocumentStore>();

        static EventStoreClient GetEsConnection(
            this IServiceProvider provider
        )
            => provider.GetRequiredService<EventStoreClient>();
    }
}
