using MarketPlace.Framework;

namespace MarketPlace.Ads.Domain.Shared
{
    public class UserId : Value<UserId>
    {
        protected UserId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(
                    nameof(value),
                    "The Id cannot be empty"
                );

            Value = value;
        }

        public static UserId FromGuid(Guid value) => new UserId(value);

        public Guid Value { get; }

        public static implicit operator Guid(UserId self) => self.Value;

        public override string ToString() => Value.ToString();
    }
}