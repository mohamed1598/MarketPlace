﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Users.Domain.Shared
{
    public delegate Task<bool> CheckTextForProfanity(string text);
}
