using EventStore.Client;
using MarketPlace.Framework;
using static EventStore.Client.EventStoreClient;

namespace MarketPlace.Infrastructure
{
    public class ProjectionManager
    {
        private readonly EventStoreClient _connection;
        private readonly ICheckPointStore _checkpointStore;
        private readonly IProjection[] _projections;
        private StreamSubscriptionResult _subscription;

        public ProjectionManager(EventStoreClient connection, ICheckPointStore checkpointStore,params IProjection[] projections)
        {
            _connection = connection;
            _checkpointStore = checkpointStore;
            _projections = projections;
        }

        public async Task Start()
        {
            var position = await _checkpointStore.GetCheckpoint();
            _subscription = _connection.SubscribeToAll(
            FromAll.After(position));
            await foreach (var message in _subscription.Messages)
            {
                switch (message)
                {
                    case StreamMessage.Event(var evnt):
                        await EventAppeared(evnt);
                        break;
                }
            }
        }

        public void Stop() => _subscription.Dispose();

        private async Task EventAppeared(ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) return;
            try
            {
                var @event = resolvedEvent.Desterilize();

                await Task.WhenAll(_projections.Select(x => x.Project(@event)));

                await _checkpointStore.StoreCheckpoint(resolvedEvent.OriginalPosition.Value);
            }catch(Exception ex) { }

           
        }

    }
}
