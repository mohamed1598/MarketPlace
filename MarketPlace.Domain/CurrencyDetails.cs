using MarketPlace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }
    public class Currency : Value<Currency>
    {
        public string CurrencyCode { get; set; } = null!;
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }
        public static Currency None = new () { InUse = false };
    }
}
