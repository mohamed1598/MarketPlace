using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarketPlace.Users.Projections.ReadModels;

namespace MarketPlace.Users.UserProfiles
{
    [Route("api/profile")]
    public class UserProfileQueryApi : ControllerBase
    {
        readonly IAsyncDocumentSession _session;

        public UserProfileQueryApi(
            IAsyncDocumentSession session)
            => _session = session;

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDetails>> Get(Guid userId)
        {
            var user = await _session.GetUserDetails(userId);

            if (user == null) return NotFound();

            return user;
        }
    }
}
