using EventStore.Client;
using MarketPlace.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.EventStore
{
    public class FunctionalStore : IFunctionalAggregateStore
    {
        readonly EventStoreClient _connection;

        public FunctionalStore(EventStoreClient connection)
            => _connection = connection;

        public Task Save<T>(
            long version,
            AggregateState<T>.Result update
        )
            where T : class, IAggregateState<T>, new()
            => _connection.AppendEvents(
                update.State.StreamName, version, update.Events.ToArray()
            );

        public Task<T> Load<T>(Guid id) where T : IAggregateState<T>, new()
            => Load<T>(id, (x, e) => x.When(x, e));

        async Task<T> Load<T>(Guid id, Func<T, object, T> when)
            where T : IAggregateState<T>, new()
        {
            const int maxSliceSize = 4096;

            var state = new T();
            var streamName = state.GetStreamName(id);

            var position = StreamPosition.Start;
            bool endOfStream;
            var events = new List<object>();

            do
            {
                var result = _connection.ReadStreamAsync(Direction.Forwards,
                        streamName,
                        position, maxSliceSize);
                 var slice = await result.ToListAsync();
            
                position += maxSliceSize;
                endOfStream = result.LastStreamPosition <= position;

                events.AddRange(
                    slice.Select(x => x.Deserialze())
                );
            } while (!endOfStream);

            return events.Aggregate(state, when);
        }
    }
}
