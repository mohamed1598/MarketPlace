using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Domain.UserProfile
{
    public class FullName:Value<FullName>
    {
        public string Value { get; set; }
        internal FullName(string value) => Value = value;

        public static FullName FromString(string fullName)
        {
            if(string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException(nameof(fullName));
            return new FullName(fullName);
        }
        public static implicit operator string(FullName fullName) { return fullName.Value;}

        protected FullName() { }
    }
}
