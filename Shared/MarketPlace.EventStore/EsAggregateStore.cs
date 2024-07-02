using EventStore.Client;
using MarketPlace.Framework;
using Newtonsoft.Json;
using System.Text;

namespace MarketPlace.EventStore
{
    public partial class EsAggregateStore:IAggregateStore
    {
        private readonly EventStoreClient _connection;

        public EsAggregateStore(EventStoreClient connection) => _connection = connection;

        public async Task Save<T>(T aggregate) where T : AggregateRoot
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            var changes = aggregate.GetChanges().ToArray();

            if (!changes.Any()) return;

            var streamName = GetStreamName(aggregate);
            await _connection.AppendEvents(
                streamName,
               aggregate.Version,
                changes);

            aggregate.ClearChanges();
        }

        public async Task<T> Load<T>(AggregateId<T> aggregateId)
            where T : AggregateRoot
        {
            if (aggregateId == null)
                throw new ArgumentNullException(nameof(aggregateId));

            var stream = GetStreamName<T>(aggregateId);
            var aggregate = (T)Activator.CreateInstance(typeof(T), true);

            var page =  await _connection.ReadStreamAsync(Direction.Forwards,
            stream,
            StreamPosition.Start).ToListAsync();

            aggregate.Load(page.Select(resolvedEvent => resolvedEvent.Deserialze()).ToArray());

            return aggregate;
        }

        public async Task<bool> Exists<T>(AggregateId<T> aggregateId) where T:AggregateRoot
        {
            var stream = GetStreamName<T>(aggregateId);
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

        private static string GetStreamName<T>(AggregateId<T> aggregateId) where T : AggregateRoot
            => $"{typeof(T).Name}-{aggregateId}";

        private static string GetStreamName<T>(T aggregate)
            where T : AggregateRoot
            => $"{typeof(T).Name}-{aggregate.Id}";
    }
}
