using MarketPlace.Users.UserProfiles;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Users.Auth
{
    public class AuthService
    {
        readonly IAsyncDocumentSession _session;

        public AuthService(IAsyncDocumentSession session)
            => _session = session;

        public async Task<bool> CheckCredentials(
            string userName,
            string password
        )
        {
            var userDetails =
                await _session.GetUserDetails(Guid.Parse(password));

            return userDetails != null && userDetails.DisplayName == userName;
        }
    }
}
