﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Framework
{
    public interface IProjection
    {
        Task Project(object @event);
    }
}
