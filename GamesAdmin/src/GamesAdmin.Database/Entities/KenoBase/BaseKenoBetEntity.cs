using MongoDB.Bson;

namespace GamesAdmin.Database.Entities.Keno
{
    public abstract class BaseKenoBetEntity : BaseBetEntity
    {
        public decimal Big { get; set; }

        public decimal Small { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal Gold { get; set; }

        public decimal Earth { get; set; }

        public decimal Fire { get; set; }

        public decimal Water { get; set; }

        public decimal Wood { get; set; }

        public ObjectId UserId { get; set; }
    }
}