﻿using MarketPlace.Framework;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents.Session;
using System.Linq.Expressions;

namespace MarketPlace.Infrastructure
{
    public abstract class RavenDbProjection<T> : IProjection
    {
        protected IAsyncDocumentSession _session { get; }

        protected RavenDbProjection(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public abstract Task Project(object @event);

        protected Task Create(Func<Task<T>> model)
            => UsingSession(
                async session => await session.StoreAsync(await model())
            );

        protected Task UpdateOne(Guid id, Action<T> update)
            => UsingSession(
                session => UpdateItem(session,id,update)
            );
        protected Task UpdateWhere(Expression<Func<T, bool>> where, Action<T> update)
            => UsingSession(
                session => UpdateMultipleItems(session,where,update)
                );

        private static async Task UpdateItem(IAsyncDocumentSession session,Guid id,Action<T> update)
        {
            var item = await session.LoadAsync<T>(id.ToString());
            if (item == null) return;
            update(item);
        }

        private async Task UpdateMultipleItems(IAsyncDocumentSession session, Expression<Func<T,bool>> query,Action<T> update)
        {
            var items = await session.Query<T>().Where(query).ToListAsync();
            foreach(var item in items) 
                update(item);
        }
        protected async Task UsingSession(Func<IAsyncDocumentSession , Task> operation)
        {

            await operation(_session);
            await _session.SaveChangesAsync();
        }
    }
}
