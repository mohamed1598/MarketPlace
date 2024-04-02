using MarketPlace.Framework;

namespace MarketPlace.Domain
{
    public class UserId:Value<UserId>
    {
        private readonly Guid _value;
        public UserId(Guid value)
        {
            ArgumentNullException.ThrowIfNull(nameof(_value), "User Id cannot be empty");
            _value = value;
        }
        public static implicit operator Guid(UserId self) => self._value;
    }
}
