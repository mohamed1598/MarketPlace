using EventStore.Client;
using MarketPlace.Framework;
using Raven.Client.Documents.Session;

namespace MarketPlace.Infrastructure
{
    public class RavenDbCheckpointStore:ICheckPointStore
    {
        private readonly IAsyncDocumentSession _session;
        private readonly string _checkpointName;

        public RavenDbCheckpointStore(IAsyncDocumentSession session, string checkpointName)
        {
            _session = session;
            _checkpointName = checkpointName;
        }
        public async Task<Position> GetCheckpoint()
        {
            var checkpoint = await _session
                .LoadAsync<Checkpoint>(_checkpointName);
            return checkpoint?.Position ?? Position.Start;
        }
        
        public async Task StoreCheckpoint(Position position)
        {
            var checkpoint = await _session.LoadAsync<Checkpoint>(_checkpointName);
            if(checkpoint is null)
            {
                checkpoint = new Checkpoint
                {
                    Id = _checkpointName
                };
                await _session.StoreAsync(checkpoint);
            }
            checkpoint.Position = position;
            await _session.SaveChangesAsync();
        }
    }
}
