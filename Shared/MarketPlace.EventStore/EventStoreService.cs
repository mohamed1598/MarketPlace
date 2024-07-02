using EventStore.Client;
using Microsoft.Extensions.Hosting;

namespace MarketPlace.EventStore
{
    public class EventStoreService : BackgroundService
    {
        private readonly IEnumerable<SubscriptionManager> _subscriptionManagers;
        public EventStoreService(IEnumerable<SubscriptionManager> subscriptionManagers,EventStoreClient esConnection)
        {
            _subscriptionManagers = subscriptionManagers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(
                _subscriptionManagers.Select(projection => projection.Start())
                );
        }
        
    }
}
