using EventStore.Client;
using Marketplace.Framework;
using MarketPlace.Framework;
using Newtonsoft.Json;
using System.Text;

namespace MarketPlace.Infrastructure
{
    public partial class EsAggregateStore:IAggregateStore
    {
        private readonly EventStoreClient _connection;

        public EsAggregateStore(EventStoreClient connection) => _connection = connection;

        public async Task Save<T, TId>(T aggregate) where T : AggregateRoot<TId> where TId:Value<TId>
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            var changes = aggregate.GetChanges().ToArray();

            if (!changes.Any()) return;

            var streamName = GetStreamName<T, TId>(aggregate);
            await _connection.AppendEvents(
                streamName,
               aggregate.Version,
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

            aggregate.Load(page.Select(resolvedEvent => resolvedEvent.Desterilize()).ToArray());

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
    }
}
