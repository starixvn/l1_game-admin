using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("lottery_cuocs")]
    public class LotteryBetEntity : BaseBetEntity
    {
        public LotteryChoicesEntity Choices { get; set; }
    }
}