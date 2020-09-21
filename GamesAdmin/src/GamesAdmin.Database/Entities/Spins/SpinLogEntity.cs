using System;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.Spins
{
    [BsonCollection("spin_logs")]
    public class SpinLogEntity : Document
    {
        public SpinLogEntity() 
        {
            Time = DateTime.UtcNow;
        }

        public string Key { get; set; }

        public int MemberId { get; set; }

        [BsonElement("spinCnt")]
        public int SpinCount { get; set; }

        public string Source { get; set; }

        public DateTime Time { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
