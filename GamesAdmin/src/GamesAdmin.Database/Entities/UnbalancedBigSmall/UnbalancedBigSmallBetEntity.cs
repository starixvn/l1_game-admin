using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class UnbalancedBigSmallBetEntity : BaseBetEntity
    {
        public decimal Big { get; set; }

        public decimal Small { get; set; }

        public decimal TotalAmount { get; set; }
    }

    [BsonCollection("unbalancedbigsmall1_bets")]
    public class UnbalancedBigSmall1BetEntity : UnbalancedBigSmallBetEntity
    {
    }

    [BsonCollection("unbalancedbigsmall2_bets")]
    public class UnbalancedBigSmall2BetEntity : UnbalancedBigSmallBetEntity
    {
    }

    [BsonCollection("unbalancedbigsmall3_bets")]
    public class UnbalancedBigSmall3BetEntity : UnbalancedBigSmallBetEntity
    {
    }

    [BsonCollection("unbalancedbigsmall4_bets")]
    public class UnbalancedBigSmall4BetEntity : UnbalancedBigSmallBetEntity
    {
    }
}