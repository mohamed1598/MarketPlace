using Marketplace.Framework;
using MarketPlace.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Domain.UserProfile
{
    public class DisplayName : Value<DisplayName>
    {
        public string Value { get; private set; }
        internal DisplayName(string displayName) => Value = displayName;
        public static DisplayName FromString(string displayName,
            CheckTextForProfanity hasProfanity)
        {
            if(string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException(nameof(displayName));
            if(hasProfanity(displayName))
                throw new DomainExceptions.ProfanityFound(displayName);
            return new DisplayName(displayName);
        }
        public static implicit operator string(DisplayName displayName) => displayName.Value;

        //just for serialization requirments
        protected DisplayName() { }
    }
}
