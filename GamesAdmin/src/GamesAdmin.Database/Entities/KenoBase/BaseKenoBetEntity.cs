using MongoDB.Bson;

namespace GamesAdmin.Database.Entities.Keno
{
    public abstract class BaseKenoBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public ObjectId UserId { get; set; }
    }
}