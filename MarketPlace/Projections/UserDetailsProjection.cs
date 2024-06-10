using MarketPlace.Domain.UserProfile;
using MarketPlace.Infrastructure;
using Raven.Client.Documents.Session;

namespace MarketPlace.Projections
{
    public class UserDetailsProjection : RavenDbProjection<ReadModels.UserDetails>
    {
        public UserDetailsProjection(Func<IAsyncDocumentSession> getSession):base(getSession)
        {
            
        }
        public override Task Project(object @event)
        {
            return @event switch
            {
                Events.UserRegistered e =>
                    Create(
                        () => Task.FromResult(
                            new ReadModels.UserDetails
                            {
                                Id = e.UserId.ToString(),
                                DisplayName = e.DisplayName,
                            }
                        )
                    ),
                Events.UserDisplayNameUpdated e =>
                    UpdateOne(
                        e.UserId,
                        x => x.DisplayName = e.DisplayName
                    ),
                Events.ProfilePhotoUploaded e =>
                    UpdateOne(
                        e.UserId,
                        x => x.PhotoUrl = e.PhotoUrl
                    ),
                _ => Task.CompletedTask
            };
        }
    }
}
