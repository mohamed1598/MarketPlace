using MarketPlace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketPlace.Domain
{
    //factory pattern
    public class ClassifiedAdTitle:Value<ClassifiedAdTitle>
    {
        //factory method
        public static ClassifiedAdTitle FromString(string title)
        {
            CheckValidity(title);
            return new(title);
        }
        public static ClassifiedAdTitle FromHtml(string htmlTitle)
        {
            var supportedTagsReplaced = htmlTitle
            .Replace("<i>", "*")
            .Replace("</i>", "*")
            .Replace("<b>", "**")
            .Replace("</b>", "**");
            var value = Regex.Replace(supportedTagsReplaced,
            "<.*?>", string.Empty);
            CheckValidity(value);
            return new ClassifiedAdTitle(value);
        }
        private static void CheckValidity(string title)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan<int>(title.Length, 100, "Title cannot be longer than 100 characters");
        }

        private readonly string _value;
        private ClassifiedAdTitle(string value)
        {
            _value = value;
        }
        public static implicit operator string(ClassifiedAdTitle self) => self._value;

    }
}
