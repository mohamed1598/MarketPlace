using MarketPlace.Framework;
using Raven.Client.Documents.Session;
using System.Security.Cryptography;

namespace MarketPlace.RavenDb
{
    public class RavenDbProjection<T> : ISubscription
    {
        static readonly string ReadModelName = typeof(T).Name;

        public RavenDbProjection(
            IAsyncDocumentSession session,
            Projector projector
        )
        {
            _projector = projector;
            _session = session;
        }

        IAsyncDocumentSession _session { get; }
        readonly Projector _projector;

        public async Task Project(object @event)
        {
            var handler = _projector(_session, @event);

            if (handler == null) return;

            await handler();
            await _session.SaveChangesAsync();
        }

        public delegate Func<Task> Projector(
            IAsyncDocumentSession session,
            object @event
        );
    }
}
