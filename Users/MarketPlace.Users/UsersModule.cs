using EventStore.Client;
using MarketPlace.EventStore;
using MarketPlace.RavenDb;
using MarketPlace.Users.Auth;
using MarketPlace.Users.Domain.Shared;
using MarketPlace.Users.Projections;
using MarketPlace.Users.UserProfiles;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarketPlace.Users.Projections.ReadModels;

namespace MarketPlace.Users
{
    public static class UsersModule
    {
        const string SubscriptionName = "usersSubscription";

        public static IServiceCollection AddUsersModule(
            this IServiceCollection services,
            string databaseName,
            CheckTextForProfanity profanityCheck
        )
        {

            services.AddSingleton(
                    c =>
                        new UserProfileApplicationService(
                            profanityCheck, new EsAggregateStore(
                                c.GetRequiredService<EventStoreClient>()
                            )
                        )
                )
                .AddSingleton<IAsyncDocumentSession>(
                    c =>
                    {
                        var store = c.GetRequiredService<IDocumentStore>();
                        store.CheckAndCreateDatabase(databaseName);

                        return store.OpenAsyncSession();
                    }
                )
                .AddSingleton(
                    c =>
                    {
                        var session =
                            c.GetRequiredService<IAsyncDocumentSession>();
                        return new SubscriptionManager(
                            c.GetRequiredService<EventStoreClient>(),
                            new RavenDbCheckpointStore(
                                session,
                                SubscriptionName
                            ),
                            SubscriptionName,
                            new RavenDbProjection<UserDetails>(
                                session,
                                UserDetailsProjection.GetHandler
                            )
                        );
                    }
                )
                .AddSingleton<AuthService>();


            return services;
        }
    }
}
