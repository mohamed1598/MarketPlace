using MarketPlace.Framework;
using MarketPlace.Users.Domain.Shared;

namespace MarketPlace.Users.Domain.UserProfile
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
            if(hasProfanity(displayName).Result)
                throw new DomainExceptions.ProfanityFound(displayName);
            return new DisplayName(displayName);
        }
        public static implicit operator string(DisplayName displayName) => displayName.Value;

        //just for serialization requirments
        protected DisplayName() { }
    }
}
