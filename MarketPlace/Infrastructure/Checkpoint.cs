using EventStore.Client;

namespace MarketPlace.Infrastructure
{
    public class Checkpoint
    {
        public string Id { get; set; }
        public Position Position { get; set; }
    }
}
