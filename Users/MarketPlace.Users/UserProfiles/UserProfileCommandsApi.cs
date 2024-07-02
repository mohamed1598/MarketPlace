using MarketPlace.Users.Domain.UserProfile;
using MarketPlace.Users.UserProfiles;
using MarketPlace.WebAPI;
using Microsoft.AspNetCore.Mvc;
using static MarketPlace.Users.Messages.Commands;

namespace MarketPlace.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class UserProfileCommandsApi : CommandApi<UserProfile>
    {
        public UserProfileCommandsApi(
            UserProfileApplicationService applicationService)
            : base(applicationService) { }

        [HttpPost]
        public Task<IActionResult> Post([FromBody] V1.RegisterUser request)
            => HandleCommand(request);

        [Route("fullname"), HttpPut]
        public Task<IActionResult> Put(V1.UpdateUserFullName request)
            => HandleCommand(request);

        [Route("displayname"), HttpPut]
        public Task<IActionResult> Put(
            V1.UpdateUserDisplayName request)
            => HandleCommand(request);

        [Route("photo"), HttpPut]
        public Task<IActionResult> Put(
            V1.UpdateUserProfilePhoto request)
            => HandleCommand(request);
    }
}
