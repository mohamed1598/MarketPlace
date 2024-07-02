using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Framework
{
    public static class ApplicationServiceExtensions
    {
        public static async Task HandleUpdate<T>(this IApplicationService service,
            IAggregateStore store, AggregateId<T> aggregateId, Action<T> operation)
            where T : AggregateRoot
        {
            var aggregate = await store.Load<T>(aggregateId);
            if (aggregate == null)
                throw new InvalidOperationException($"Entity with id {aggregateId.ToString()} cannot be found");

            operation(aggregate);
            await store.Save<T>(aggregate);
        }
    }
}
