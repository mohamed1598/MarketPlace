using MarketPlace.Framework;

namespace MarketPlace.Users.Domain.UserProfile
{
    public class UserId : AggregateId<UserProfile>
    {
        public UserId(Guid value): base(value) { }
    }
}