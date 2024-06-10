﻿using Raven.Client.Documents.Session;
using static MarketPlace.Projections.ReadModels;

namespace MarketPlace.UserProfiles
{
    public static class Queries
    {
        public static Task<UserDetails> GetUserDetails(
            this IAsyncDocumentSession session,
            Guid id
        )
        {
            return session.LoadAsync<UserDetails>(id.ToString());
        }
    }
}
