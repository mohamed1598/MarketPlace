using MarketPlace.Framework;
using Raven.Client.Documents.Session;

namespace MarketPlace.RavenDb
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
        public async Task<ulong> GetCheckpoint()
        {
            var checkpoint = await _session
                .LoadAsync<Checkpoint>(_checkpointName);
            return checkpoint?.Position ?? 0;
        }
        
        public async Task StoreCheckpoint(ulong position)
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
        class Checkpoint
        {
            public string Id { get; set; }
            public ulong? Position { get; set; }
        }
    }
}
