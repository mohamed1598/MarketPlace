using EventStore.Client;
using Google.Protobuf.Compiler;
using MarketPlace.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.EventStore
{
    public class EsCheckpointStore : ICheckPointStore
    {
        const string CheckpointStreamPrefix = "checkpoint:";
        readonly EventStoreClient _connection;
        readonly string _streamName;

        public EsCheckpointStore(
            EventStoreClient connection,
            string subscriptionName)
        {
            _connection = connection;
            _streamName = CheckpointStreamPrefix + subscriptionName;
        }

        public async Task<ulong> GetCheckpoint()
        {
            var slice = await _connection.ReadStreamAsync(
                Direction.Backwards,
                _streamName,
                StreamPosition.End,
                1
                ).FirstOrDefaultAsync();


            if (slice.Equals(default(ResolvedEvent)))
            {
                await StoreCheckpoint(0);
                await SetStreamMaxCount();
                return 0;
            }

            return (ulong)slice.Deserialze<Checkpoint>()?.Position!;
        }

        public Task StoreCheckpoint(ulong checkpoint)
        {
            var @event = new Checkpoint { Position = checkpoint };

            var preparedEvent = new List<EventData>()
            {
                 new EventData(
                        Uuid.NewUuid(),
                        "$checkpoint",
                        Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(@event)
                        ),
                        null
                    )
            };


            return _connection.AppendToStreamAsync(
                _streamName,
                StreamRevision.None,
                preparedEvent
            );
        }

        async Task SetStreamMaxCount()
        {
            var metadata = await _connection.GetStreamMetadataAsync(_streamName);

            if (!metadata.Metadata.MaxCount.HasValue)
                await _connection.SetStreamMetadataAsync(
                    _streamName, StreamRevision.None,
                    new StreamMetadata()
                );
        }

        class Checkpoint
        {
            public ulong Position { get; set; }
        }
    }
}
