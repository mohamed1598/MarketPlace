using EventStore.Client;
using MarketPlace.Framework;
using static EventStore.Client.EventStoreClient;

namespace MarketPlace.EventStore
{
    public class SubscriptionManager
    {
        private readonly EventStoreClient _connection;
        private readonly ICheckPointStore _checkpointStore;
        private readonly ISubscription[] _subscriptions;
        private StreamSubscriptionResult _subscription;
        readonly string _name;

        public SubscriptionManager(EventStoreClient connection, ICheckPointStore checkpointStore, string name, params ISubscription[] subscriptions)
        {
            _connection = connection;
            _checkpointStore = checkpointStore;
            _subscriptions = subscriptions;
            _name = name;
        }

        public async Task Start()
        {
            var position = await _checkpointStore.GetCheckpoint();
            _subscription = _connection.SubscribeToAll(
                FromAll.After(new Position(position,position))
            );
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
                var @event = resolvedEvent.Deserialze();
                foreach(var item in _subscriptions)
                {
                    await item.Project(@event);
                }

                await _checkpointStore.StoreCheckpoint(resolvedEvent.OriginalPosition.Value.CommitPosition);
            }catch(Exception ex) { }

           
        }

    }
}
