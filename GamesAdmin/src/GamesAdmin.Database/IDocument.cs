using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GamesAdmin.Database
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("_id")]
        ObjectId id { get; set; }

        DateTime time { get; set; }

        int __v { get; set; }
    }

    public class Document : IDocument
    {
        public ObjectId id { get; set; }

        public DateTime time { get; set; }
        public int __v { get; set; }
    }
}
