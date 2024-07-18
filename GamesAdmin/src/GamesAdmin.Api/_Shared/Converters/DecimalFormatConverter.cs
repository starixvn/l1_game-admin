using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace GamesAdmin.Api._Shared.Converters
{
    public class DecimalFormatConverter : JsonConverter<decimal>
    {
        public override decimal ReadJson(JsonReader reader, Type objectType, [AllowNull] decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Convert.ToDecimal(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] decimal value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}