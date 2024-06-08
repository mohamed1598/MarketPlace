using MarketPlace.Infrastructure;
using MarketPlace.UserProfiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileCommandsApi : ControllerBase
    {
        private readonly UserProfileApplicationService _applicationService;

        public UserProfileCommandsApi(UserProfileApplicationService applicationService)
            => _applicationService = applicationService;

        [HttpPost]
        public Task<IActionResult> Post(Contracts.V1.RegisterUser request)
            => RequestHandler.HandleRequest(request, _applicationService.Handle );

        [Route("fullname")]
        [HttpPut]
        public Task<IActionResult> Put(Contracts.V1.UpdateUserFullName request)
            => RequestHandler.HandleRequest(request, _applicationService.Handle );

        [Route("displayname")]
        [HttpPut]
        public Task<IActionResult> Put(Contracts.V1.UpdateUserDisplayName request)
            => RequestHandler.HandleRequest(request, _applicationService.Handle );

        [Route("photo")]
        [HttpPut]
        public Task<IActionResult> Put(Contracts.V1.UpdateUserProfilePhoto request)
            => RequestHandler.HandleRequest(request, _applicationService.Handle );
    }
}
