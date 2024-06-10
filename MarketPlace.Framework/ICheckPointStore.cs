using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Framework
{
    public interface ICheckPointStore
    {
        Task<Position> GetCheckpoint();
        Task StoreCheckpoint(Position checkpoint);
    }
}
