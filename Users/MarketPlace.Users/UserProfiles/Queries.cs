using Raven.Client.Documents.Session;
using static MarketPlace.Users.Projections.ReadModels;

namespace MarketPlace.Users.UserProfiles
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
