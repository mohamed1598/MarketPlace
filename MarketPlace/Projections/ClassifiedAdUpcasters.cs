//using EventStore.Client;
//using MarketPlace.Framework;
//using MarketPlace.Infrastructure;
//using static MarketPlace.Domain.ClassifiedAd.Events;
//using static MarketPlace.Projections.ClassifiedAdUpcastedEvents;

//namespace MarketPlace.Projections
//{
//    public class ClassifiedAdUpcasters : IProjection
//    {
//        private readonly EventStoreClient _eventStoreClient;
//        private readonly Func<Guid,Task<string>> _getUserPhoto;
//        private const string StreamName = "UpcastedClassifiedAdEvents";

//        public ClassifiedAdUpcasters(EventStoreClient eventStoreClient, Func<Guid, Task<string>> getUserPhoto)
//        {
//            _eventStoreClient = eventStoreClient;
//            _getUserPhoto = getUserPhoto;
//        }

//        public async Task Project(object @event)
//        {
//            switch (@event)
//            {
//                case ClassifiedAdPublished e:
//                    var photoUrl = await _getUserPhoto(e.OwnerId);
//                    var newEvent = new V1.ClassifiedAdPublished
//                    {
//                        Id = e.Id,
//                        OwnerId = e.OwnerId,
//                        ApprovedBy = e.ApprovedBy,
//                        SellersPhotoUrl = photoUrl,
//                    };
//                    await _eventStoreClient.AppendEvents(
//                        StreamName,
//                        0,
//                        newEvent
//                    );
//                    break;
//            }

//        }
//    }

//    public static class ClassifiedAdUpcastedEvents
//    {
//        public static class V1
//        {
//            public class ClassifiedAdPublished
//            {
//                public Guid Id { get; set; }
//                public Guid OwnerId {  get; set; }
//                public string SellersPhotoUrl { get; set; }
//                public Guid ApprovedBy { get; set; }
//            }

            
//        }
//    }
//}
