using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("_id")]
        ObjectId Id { get; set; }
    }

    public class Document : IDocument
    {
        public ObjectId Id { get; set; }
    }
}