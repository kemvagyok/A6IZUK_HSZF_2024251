using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace A6IZUK_HSZF_2024251.Model
{
    public class RailwayLineRaw
    {
        public string LineNumber { get; set; }
        public string LineName { get; set; }
        [JsonProperty("Service")]
        [JsonConverter(typeof(SingleOrArrayConverter<ServiceRaw>))]
        public List<ServiceRaw> Services { get; set; } = new List<ServiceRaw>();
    }


    public class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<List<T>>();
            }
            return new List<T> { token.ToObject<T>() };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var items = value as List<T>;
            if (items == null)
            {
                writer.WriteNull();
                return;
            }

            // Ha egyetlen elem van, objektumként írjuk ki; ha több, akkor tömbként
            if (items.Count == 1)
            {
                serializer.Serialize(writer, items[0]);
            }
            else
            {
                serializer.Serialize(writer, items);
            }
        }
    }
}
