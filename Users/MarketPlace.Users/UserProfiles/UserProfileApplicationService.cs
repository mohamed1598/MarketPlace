
using MarketPlace.Framework;
using MarketPlace.Users.Domain.Shared;
using MarketPlace.Users.Domain.UserProfile;
using static MarketPlace.Users.Messages.Commands;


namespace MarketPlace.Users.UserProfiles
{
    public class UserProfileApplicationService : ApplicationService<UserProfile>
    {

        public UserProfileApplicationService(
            CheckTextForProfanity checkText,
            IAggregateStore store) : base(store)
        {
            CreateWhen<V1.RegisterUser>(
                cmd => new UserId(cmd.UserId),
                (cmd, id) => UserProfile.Create(
                    new UserId(id), FullName.FromString(cmd.FullName),
                    DisplayName.FromString(cmd.DisplayName, checkText)
                )
            );

            UpdateWhen<V1.UpdateUserFullName>(
                cmd => new UserId(cmd.UserId),
                (user, cmd)
                    => user.UpdateFullName(FullName.FromString(cmd.FullName))
            );

            UpdateWhen<V1.UpdateUserDisplayName>(
                cmd => new UserId(cmd.UserId),
                (user, cmd) => user.UpdateDisplayName(
                    DisplayName.FromString(cmd.DisplayName, checkText)
                )
            );

            UpdateWhen<V1.UpdateUserProfilePhoto>(
                cmd => new UserId(cmd.UserId),
                (user, cmd) => user.UpdateProfilePhoto(new Uri(cmd.PhotoUrl))
            );
        }
    }
}
