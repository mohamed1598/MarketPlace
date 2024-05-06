using MarketPlace.Framework;
using static MarketPlace.Domain.Picture;

namespace MarketPlace.Domain
{
    public class Picture : Entity<PictureId>
    {
        public Picture(Action<object> applier) : base(applier)
        {
        }

        internal ClassifiedAdId ParentId { get; private set; }
        internal PictureSize Size { get; private set; }
        internal Uri Location { get; private set; }
        internal int Order { get; private set; }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToAClassifiedAd e:
                    ParentId = new ClassifiedAdId(e.ClassifiedAdId);
                    Id = new PictureId(e.PictureId);
                    Location = new Uri(e.Url);
                    Size = new PictureSize { Height = e.Height, Width = e.Width };
                    Order = e.Order;
                    break;
                case Events.ClassifiedAdPictureResized e:
                    Size = new PictureSize { Height = e.Height, Width = e.Width };
                    break;
            }
        }
        public void Resize(PictureSize newSize)
            => Apply(new Events.ClassifiedAdPictureResized
            {
                PictureId = Id.Value,
                ClassifiedAdId = ParentId,
                Height = newSize.Height,
                Width = newSize.Width
            });

        internal bool HasCorrectSize()
        {
            return Size.Height > 0 && Size.Width > 0;
        }
    }

    public class PictureId : Value<PictureId>
    {
        public PictureId(Guid value) => Value = value;
        public Guid Value { get; }
    }
}