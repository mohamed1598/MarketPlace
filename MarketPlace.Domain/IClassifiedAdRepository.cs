using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Domain
{
    public interface IClassifiedAdRepository
    {
        Task<bool> Exists(ClassifiedAdId id);

        Task<ClassifiedAd> Load(ClassifiedAdId id);

        Task Save(ClassifiedAd entity);
    }
}
