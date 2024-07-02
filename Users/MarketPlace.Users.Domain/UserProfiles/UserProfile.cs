using MarketPlace.Framework;
using MarketPlace.Users.Messages;

namespace MarketPlace.Users.Domain.UserProfile
{
    public class UserProfile : AggregateRoot
    {
        //Aggregate state properties
        public FullName FullName { get; private set; }
        public DisplayName DisplayName { get; private set; }
        public string? PhotoUrl { get; private set; }

        public static UserProfile Create(UserId id, FullName fullName, DisplayName displayName)
        {
            var profile = new UserProfile();
            profile.Apply(new Events.V1.UserRegistered
            {
                UserId = id,
                FullName = fullName,
                DisplayName = displayName
            });
            return profile;
        } 

        public void UpdateFullName(FullName fullName)
            => Apply(new Events.V1.UserFullNameUpdated { 
                UserId = Id, 
                FullName = fullName 
            });

        public void UpdateDisplayName(DisplayName displayName)
            => Apply(new Events.V1.UserDisplayNameUpdated
            {
                UserId = Id,
                DisplayName = displayName
            });

        public void UpdateProfilePhoto(Uri photoUrl)
            => Apply(new Events.V1.ProfilePhotoUploaded
            {
                UserId = Id,
                PhotoUrl = photoUrl.ToString()
            });

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.V1.UserRegistered e:
                    Id = new UserId(e.UserId);
                    FullName = new FullName(e.FullName);
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case Events.V1.UserFullNameUpdated e:
                    FullName = new FullName(e.FullName);
                    break;
                case Events.V1.UserDisplayNameUpdated e:
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case Events.V1.ProfilePhotoUploaded e:
                    PhotoUrl = e.PhotoUrl;
                    break;
            }
        }
        protected override void EnsureValidState()
        {
        }
        protected UserProfile() { }
    }
}
