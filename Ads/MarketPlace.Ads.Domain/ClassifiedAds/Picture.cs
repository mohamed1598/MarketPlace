using MarketPlace.Framework;
using static MarketPlace.Ads.Messages.Events;
namespace MarketPlace.Ads.Domain.ClassifiedAds
{
    public class Picture : Entity<PictureId>
    {
        // Properties to handle the persistence
        public Guid PictureId
        {
            get => Id.Value;
            set { }
        }

        protected Picture() { }

        // Entity state
        public ClassifiedAdId ParentId { get; private set; }
        public PictureSize Size { get; private set; }
        public string Location { get; private set; }
        public int Order { get; private set; }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case V1.PictureAddedToAClassifiedAd e:
                    ParentId = new ClassifiedAdId(e.ClassifiedAdId);
                    Id = new PictureId(e.PictureId);
                    Location = e.Url;
                    Size = new PictureSize { Height = e.Height, Width = e.Width };
                    Order = e.Order;
                    break;
                case V1.ClassifiedAdPictureResized e:
                    Size = new PictureSize { Height = e.Height, Width = e.Width };
                    break;
            }
        }

        public void Resize(PictureSize newSize)
            => Apply(new V1.ClassifiedAdPictureResized
            {
                PictureId = Id.Value,
                ClassifiedAdId = ParentId.Value,
                Height = newSize.Width,
                Width = newSize.Width
            });

        public Picture(Action<object> applier) : base(applier)
        {
        }
    }

    public class PictureId : Value<PictureId>
    {
        public PictureId(Guid value) => Value = value;

        public Guid Value { get; }

        protected PictureId() { }
    }
}