using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Users.Domain.Shared
{
    public static class DomainExceptions
    {
        public class InvalidEntityState(object entity, string message) : Exception($"Entity {entity.GetType().Name} state change rejected, {message}") {
        }
        public class ProfanityFound(string text) : Exception($"Profanity found in text: {text}")
        {
        }
    }
}
