using MarketPlace.Domain.Shared;
using Marketplace.Framework;
using MarketPlace.Domain.UserProfile;
using static MarketPlace.UserProfiles.Contracts;
using MarketPlace.Framework;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MarketPlace.UserProfiles
{
    public class UserProfileApplicationService : IApplicationService
    {
        private readonly IAggregateStore _store;
        private readonly CheckTextForProfanity _checkText;

        public UserProfileApplicationService(
            CheckTextForProfanity checkText,
            IAggregateStore store)
        {
            _checkText = checkText;
            _store = store;
        }

        public Task Handle(object command) =>
            command switch
            {
                V1.RegisterUser cmd =>
                    HandleCreate(cmd),
                V1.UpdateUserFullName cmd =>
                    HandleUpdate(
                        cmd.UserId,
                        profile => profile.UpdateFullName(
                            FullName.FromString(cmd.FullName)
                        )
                    ),
                V1.UpdateUserDisplayName cmd =>
                    HandleUpdate(
                        cmd.UserId,
                        profile => profile.UpdateDisplayName(
                            DisplayName.FromString(cmd.DisplayName, _checkText)
                        )
                    ),
                V1.UpdateUserProfilePhoto cmd =>
                    HandleUpdate(
                        cmd.UserId,
                        profile => profile.UpdateProfilePhoto(new Uri(cmd.PhotoUrl))
                    ),
                _ => Task.CompletedTask
            };

        private async Task HandleCreate(V1.RegisterUser cmd)
        {
            if (await _store.Exists<UserProfile,UserId>(new UserId(cmd.UserId)))
                throw new InvalidOperationException(
                    $"Entity with id {cmd.UserId} already exists"
                );

            var userProfile = new UserProfile(
                new UserId(cmd.UserId),
                FullName.FromString(cmd.FullName),
                DisplayName.FromString(cmd.DisplayName, _checkText)
            );

            await _store
                .Save< UserProfile, UserId>(
                    userProfile
                );
        }

        private async Task HandleUpdate(
            Guid userProfileId,
            Action< UserProfile> update
        )
        {
            this.HandleUpdate(
                _store,
                new UserId(userProfileId),
                update
            );
        }
    }
}
