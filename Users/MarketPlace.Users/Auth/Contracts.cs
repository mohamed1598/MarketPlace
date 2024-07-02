using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Users.Auth
{
    public static class Contracts
    {
        public class Login
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
