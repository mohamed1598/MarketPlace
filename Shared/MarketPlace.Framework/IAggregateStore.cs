﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Framework
{
    public interface IAggregateStore
    {
        Task<bool> Exists<T>(AggregateId<T> aggregateId) where T:AggregateRoot;

        Task Save<T>(T aggregate) where T : AggregateRoot;

        Task<T> Load<T>(AggregateId<T> aggregateId) where T : AggregateRoot;
    }

    public interface IFunctionalAggregateStore
    {
        Task Save<T>(long version, AggregateState<T>.Result update)
            where T : class, IAggregateState<T>, new();

        Task<T> Load<T>(Guid id)
            where T : IAggregateState<T>, new();
    }
}
