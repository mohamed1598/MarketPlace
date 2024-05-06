
using Marketplace.Framework;

namespace MarketPlace.Data
{
    public class EfCoreUnitOfWork(ClassifiedAdDbContext dbContext) : IUnitOfWork
    {
        private readonly ClassifiedAdDbContext _dbContext = dbContext;

        public Task Commit()
        => _dbContext.SaveChangesAsync();
    }
}
