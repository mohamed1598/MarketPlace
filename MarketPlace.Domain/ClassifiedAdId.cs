using MarketPlace.Framework;

namespace MarketPlace.Domain
{
    public class ClassifiedAdId:Value<ClassifiedAdId>
    {
        private readonly Guid _value;
        public ClassifiedAdId(Guid value)
        {
            ArgumentNullException.ThrowIfNull(nameof(_value), "Classified Ad Id cannot be empty");
            _value = value;
        }
        public static implicit operator Guid(ClassifiedAdId self) => self._value;

    }
}
