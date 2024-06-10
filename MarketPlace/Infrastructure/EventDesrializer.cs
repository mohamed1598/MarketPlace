using EventStore.Client;
using Newtonsoft.Json;
using System.Text;

namespace MarketPlace.Infrastructure
{
    public static class EventDesrializer
    {
        public static object Desterilize(this ResolvedEvent resolvedEvent)
        {
            var meta = JsonConvert.DeserializeObject<EventMetadata>(
                    Encoding.UTF8.GetString(resolvedEvent.Event.Metadata.ToArray()));
            var dataType = Type.GetType(meta.ClrType);
            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
            var data = JsonConvert.DeserializeObject(jsonData, dataType);
            return data;
        }
    }
}
