using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sicbo_cuocs")]
    public class SicBoBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}