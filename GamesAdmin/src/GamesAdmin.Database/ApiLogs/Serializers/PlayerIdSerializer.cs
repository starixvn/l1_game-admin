using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GamesAdmin.Database.ApiLogs.Serializers
{
    public class PlayerIdSerializer : SerializerBase<object>
    {
        public override object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;

            switch (reader.CurrentBsonType)
            {
                case BsonType.String:
                    return reader.ReadString();

                case BsonType.Int32:
                    return reader.ReadInt32();

                case BsonType.Int64:
                    return reader.ReadInt64();

                default:
                    reader.SkipValue();
                    return null;
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var writer = context.Writer;

            if (value == null)
            {
                writer.WriteNull();
            }
            else if (value is int intValue)
            {
                writer.WriteInt32(intValue);
            }
            else if (value is long longValue)
            {
                writer.WriteInt64(longValue);
            }
            else
            {
                writer.WriteString(value.ToString());
            }
        }
    }
}