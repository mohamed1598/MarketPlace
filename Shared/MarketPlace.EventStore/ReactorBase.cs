using MarketPlace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.EventStore
{
    public abstract class ReactorBase : ISubscription
    {
        public ReactorBase(Reactor reactor) => _reactor = reactor;

        readonly Reactor _reactor;

        public Task Project(object @event)
        {
            var handler = _reactor(@event);

            if (handler == null) return Task.CompletedTask;

            return handler();
        }

        public delegate Func<Task> Reactor(object @event);
    }
}
