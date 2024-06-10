﻿using EventStore.Client;
using MarketPlace.Framework;
using Raven.Client.Documents.Session;

namespace MarketPlace.Infrastructure
{
    public class RavenDbCheckpointStore:ICheckPointStore
    {
        private readonly Func<IAsyncDocumentSession> _getSession;
        private readonly string _checkpointName;

        public RavenDbCheckpointStore(Func<IAsyncDocumentSession> getSession, string checkpointName)
        {
            _getSession = getSession;
            _checkpointName = checkpointName;
        }
        public async Task<Position> GetCheckpoint()
        {
            using var session = _getSession();
            var checkpoint = await session
                .LoadAsync<Checkpoint>(_checkpointName);
            return checkpoint?.Position ?? Position.Start;
        }
        
        public async Task StoreCheckpoint(Position position)
        {
            using var session = _getSession();

            var checkpoint = await session.LoadAsync<Checkpoint>(_checkpointName);
            if(checkpoint is null)
            {
                checkpoint = new Checkpoint
                {
                    Id = _checkpointName
                };
                await session.StoreAsync(checkpoint);
            }
            checkpoint.Position = position;
            await session.SaveChangesAsync();
        }
    }
}
