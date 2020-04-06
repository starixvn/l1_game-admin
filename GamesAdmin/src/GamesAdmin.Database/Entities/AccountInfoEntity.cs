using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("userinfos")]
    public class AccountInfoEntity : MiniGameBaseEntity
    {
        [BsonElement("id")]
        public string userId { get; set; }

        public string avatar { get; set; }
        public DateTime joinedOn { get; set; }

        public string email { get; set; }

        public string cmt { get; set; }

        public double red { get; set; }

        public int ketSat { get; set; }

        public int xu { get; set; }

        public double redWin { get; set; }

        public int redLost { get; set; }

        public int redPlay { get; set; }

        public double xuWin { get; set; }

        public int xuLost { get; set; }

        public int xuPlay { get; set; }

        public int thuong { get; set; }

        public int vip { get; set; }

        public int lastVip { get; set; }

        public int hu { get; set; }

        public int huXu { get; set; }

        [BsonElement("type")]
        public bool isBot { get; set; }

        public bool otpFirst { get; set; }

        public int avatarId { get; set; }

        public string currency { get; set; }

        public string name { get; set; }

        [BsonElement("UID")]
        public int UID { get; set; }

        public Security security { get; set; }
    }

    public class Security
    {
        public int login { get; set; }
    }
}
