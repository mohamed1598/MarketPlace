
namespace MarketPlace.Framework
{
    public interface ICheckPointStore
    {
        Task<ulong> GetCheckpoint();
        Task StoreCheckpoint(ulong checkpoint);
    }
}
