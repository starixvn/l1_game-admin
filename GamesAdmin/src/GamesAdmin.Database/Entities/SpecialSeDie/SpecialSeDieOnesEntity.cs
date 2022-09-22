using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("specialsedie_ones")]
    public class SpecialSeDieOnesEntity : BasePoolBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public bool Win { get; set; }
    }
}