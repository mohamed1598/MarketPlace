﻿using Raven.Client.Documents.Session;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.RavenDb
{
    public static class RavenDbExtensions
    {
        public static async Task<ActionResult<T>> RunApiQuery<T>(
            this IAsyncDocumentSession _session,
            Func<IAsyncDocumentSession, Task<T>> query
        )
        {

            try
            {
                return new OkObjectResult(await query(_session));
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(
                    new
                    {
                        error = e.Message,
                        stackTrace = e.StackTrace
                    }
                );
            }
        }

        public static async Task Update<T>(
            this IAsyncDocumentSession session,
            string id,
            Action<T> update
        )
        {
            var item = await session.LoadAsync<T>(id);

            if (item == null) return;

            update(item);
        }

        public static Task Del(
            this IAsyncDocumentSession session,
            string id
        )
        {
            session.Delete(id);
            return Task.CompletedTask;
        }

        public static async Task UpsertItem<T>(
            this IAsyncDocumentSession session,
            string id,
            Action<T> update,
            Func<T> create
        )
        {
            var item = await session.LoadAsync<T>(id);

            if (item == null)
            {
                item = create();
                await session.StoreAsync(item);
            }

            update(item);
        }

        public static async Task UpdateMultipleItems<T>(
            this IAsyncDocumentSession session,
            Expression<Func<T, bool>> query,
            Action<T> update
        )
        {
            var items = await session
                .Query<T>()
                .Where(query)
                .ToListAsync();

            foreach (var item in items)
                update(item);
        }

        public static Task Create<T>(
            this IAsyncDocumentSession session,
            Func<T> createDocument
        )
            => session.StoreAsync(createDocument());

        public static Task Create<T>(
            this IAsyncDocumentSession session,
            Action<T> populate
        ) where T : new()
        {
            var doc = new T();
            populate(doc);
            return session.StoreAsync(doc);
        }

        public static void CheckAndCreateDatabase(
            this IDocumentStore store,
            string databaseName
        )
        {
            var record = store.Maintenance.Server.Send(
                new GetDatabaseRecordOperation(databaseName)
            );

            if (record == null)
                store.Maintenance.Server.Send(
                    new CreateDatabaseOperation(
                        new DatabaseRecord(databaseName)
                    )
                );
        }
    }
}
