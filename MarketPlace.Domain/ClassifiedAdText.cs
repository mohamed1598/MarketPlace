namespace MarketPlace.Domain
{
    public class ClassifiedAdText
    {
        public string Value { get; }

        internal ClassifiedAdText(string text) => Value = text;

        public static ClassifiedAdText FromString(string text) =>
            new (text);

        public static implicit operator string(ClassifiedAdText text) =>
            text.Value;
    }
}