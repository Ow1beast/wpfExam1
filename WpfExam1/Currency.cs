using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Currency
{

    public partial class Welcome
    {
        [JsonProperty("rss")]
        public Rss Rss { get; set; }
    }

    public partial class Rss
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }

    public partial class Channel
    {
        [JsonProperty("generator")]
        public string Generator { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("item")]
        public List<Item> Item { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("pubDate")]
        public PubDate PubDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quant")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Quant { get; set; }

        [JsonProperty("index")]
        public Index Index { get; set; }

        [JsonProperty("change")]
        public string Change { get; set; }

        [JsonProperty("link")]
        public object Link { get; set; }
    }

    public enum Index { Down, Empty, Up };

    public enum PubDate { The050105, The210219, The311210, The311213 };

    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, Currency.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, Currency.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                IndexConverter.Singleton,
                PubDateConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class IndexConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Index) || t == typeof(Index?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Index.Empty;
                case "DOWN":
                    return Index.Down;
                case "UP":
                    return Index.Up;
            }
            throw new Exception("Cannot unmarshal type Index");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Index)untypedValue;
            switch (value)
            {
                case Index.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Index.Down:
                    serializer.Serialize(writer, "DOWN");
                    return;
                case Index.Up:
                    serializer.Serialize(writer, "UP");
                    return;
            }
            throw new Exception("Cannot marshal type Index");
        }

        public static readonly IndexConverter Singleton = new IndexConverter();
    }

    internal class PubDateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PubDate) || t == typeof(PubDate?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "05.01.05":
                    return PubDate.The050105;
                case "21.02.19":
                    return PubDate.The210219;
                case "31.12.10":
                    return PubDate.The311210;
                case "31.12.13":
                    return PubDate.The311213;
            }
            throw new Exception("Cannot unmarshal type PubDate");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (PubDate)untypedValue;
            switch (value)
            {
                case PubDate.The050105:
                    serializer.Serialize(writer, "05.01.05");
                    return;
                case PubDate.The210219:
                    serializer.Serialize(writer, "21.02.19");
                    return;
                case PubDate.The311210:
                    serializer.Serialize(writer, "31.12.10");
                    return;
                case PubDate.The311213:
                    serializer.Serialize(writer, "31.12.13");
                    return;
            }
            throw new Exception("Cannot marshal type PubDate");
        }

        public static readonly PubDateConverter Singleton = new PubDateConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
