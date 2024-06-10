using Raven.Client.Documents.Session;
using static MarketPlace.Projections.ReadModels;

namespace MarketPlace.UserProfiles
{
    public static class Queries
    {
        public static Task<UserDetails> GetUserDetails(
            this Func<IAsyncDocumentSession> getSession,
            Guid id
        )
        {
            using var session = getSession();

            return session.LoadAsync<UserDetails>(id.ToString());
        }
    }
}
