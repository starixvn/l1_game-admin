using System;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("userinfos")]
    public class AccountInfoEntity : MiniGameBaseEntity
    {
        [BsonElement("id")]
        public string UserId { get; set; }

        public string Avatar { get; set; }

        public DateTime JoinedOn { get; set; }

        public string Email { get; set; }

        public string Cmt { get; set; }

        public double Red { get; set; }

        public int KetSat { get; set; }

        public int Xu { get; set; }

        public double RedWin { get; set; }

        public int RedLost { get; set; }

        public int RedPlay { get; set; }

        public double XuWin { get; set; }

        public int XuLost { get; set; }

        public int XuPlay { get; set; }

        public int Thuong { get; set; }

        public int Vip { get; set; }

        public int LastVip { get; set; }

        public int Hu { get; set; }

        public int HuXu { get; set; }

        [BsonElement("type")]
        public bool IsBot { get; set; }

        public bool OtpFirst { get; set; }

        public int AvatarId { get; set; }

        public string Currency { get; set; }

        public string Name { get; set; }

        [BsonElement("UID")]
        public int UID { get; set; }

        public Security Security { get; set; }
    }

    public class Security
    {
        public int Login { get; set; }
    }
}