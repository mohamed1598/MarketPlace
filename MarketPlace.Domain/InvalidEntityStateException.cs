using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Domain
{
    public class InvalidEntityStateException(object entity,string message):Exception(
        $"Entity {entity.GetType().Name} state change rejected, {message}")
    {
    }
}
