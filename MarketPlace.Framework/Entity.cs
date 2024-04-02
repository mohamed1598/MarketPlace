using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Framework
{
    public abstract class Entity
    {
        private readonly List<object> _events;
        protected Entity() => _events = [];
        protected void Raise(object @event) => _events.Add(@event);
        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _events.Add(@event);
        }
        protected abstract void When(object @event);
        public IEnumerable<object> GetChanges() => _events.AsEnumerable();
        public void ClearChanges() => _events.Clear();
        protected abstract void EnsureValidState();
    }
}
