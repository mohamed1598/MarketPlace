using Marketplace.Domain;

namespace MarketPlace.Data
{
    public class ClassifiedAdRepository : IClassifiedAdRepository
    {
        private readonly ClassifiedAdDbContext _dbContext;

        public ClassifiedAdRepository(ClassifiedAdDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(ClassifiedAd entity)
        => _dbContext.ClassifiedAds.AddAsync(entity);

        public async Task<bool> Exists(ClassifiedAdId id)
         => await _dbContext.ClassifiedAds.FindAsync(id) is not null;


        public async Task<ClassifiedAd?> Load(ClassifiedAdId id)
            => await _dbContext.ClassifiedAds.FindAsync(id);
    }
}
