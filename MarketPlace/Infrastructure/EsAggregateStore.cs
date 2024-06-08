using EventStore.Client;
using Marketplace.Framework;
using MarketPlace.Framework;
using Newtonsoft.Json;
using System.Text;

namespace MarketPlace.Infrastructure
{
    public class EsAggregateStore:IAggregateStore
    {
        private readonly EventStoreClient _connection;

        public EsAggregateStore(EventStoreClient connection) => _connection = connection;

        public async Task Save<T, TId>(T aggregate) where T : AggregateRoot<TId> where TId:Value<TId>
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            var changes = aggregate.GetChanges()
                .Select(@event =>
                    new EventData(
                        eventId: Uuid.NewUuid(),
                        type: @event.GetType().Name,
                        data: Serialize(@event),
                        metadata: Serialize(new EventMetadata
                        { ClrType = @event.GetType().AssemblyQualifiedName })
                    ))
                .ToArray();

            if (!changes.Any()) return;

            var streamName = GetStreamName<T, TId>(aggregate);

            await _connection.AppendToStreamAsync(
                streamName,
                StreamState.Any,
                changes);

            aggregate.ClearChanges();
        }

        public async Task<T> Load<T, TId>(TId aggregateId)
            where T : AggregateRoot<TId> where TId:Value<TId>
        {
            if (aggregateId == null)
                throw new ArgumentNullException(nameof(aggregateId));

            var stream = GetStreamName<T, TId>(aggregateId);
            var aggregate = (T)Activator.CreateInstance(typeof(T), true);

            var page =  await _connection.ReadStreamAsync(Direction.Forwards,
            stream,
            StreamPosition.Start).ToListAsync();

            aggregate.Load(page.Select(resolvedEvent =>
            {
                var meta = JsonConvert.DeserializeObject<EventMetadata>(
                    Encoding.UTF8.GetString(resolvedEvent.Event.Metadata.ToArray()));
                var dataType = Type.GetType(meta.ClrType);
                var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
                var data = JsonConvert.DeserializeObject(jsonData, dataType);
                return data;
            }).ToArray());

            return aggregate;
        }

        public async Task<bool> Exists<T, TId>(TId aggregateId)
        {
            var stream = GetStreamName<T, TId>(aggregateId);
            var result = _connection.ReadStreamAsync(
                Direction.Forwards,
                stream,
                10,
                20
            );

            return await result.ReadState != ReadState.StreamNotFound;
        }

        private static byte[] Serialize(object data)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        private static string GetStreamName<T, TId>(TId aggregateId)
            => $"{typeof(T).Name}-{aggregateId.ToString()}";

        private static string GetStreamName<T, TId>(T aggregate)
            where T : AggregateRoot<TId> where TId:Value<TId>
            => $"{typeof(T).Name}-{aggregate.Id.ToString()}";

        private class EventMetadata
        {
            public string ClrType { get; set; }
        }
    }
}
