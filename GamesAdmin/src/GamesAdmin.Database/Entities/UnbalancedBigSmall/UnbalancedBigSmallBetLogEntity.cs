using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class UnbalancedBigSmallBetLogEntity : BaseBetEntity
    {
        public decimal Amount { get; set; }

        public string Choice { get; set; }
    }

    [BsonCollection("unbalancedbigsmall1_betlogs")]
    public class UnbalancedBigSmall1BetLogEntity : UnbalancedBigSmallBetLogEntity
    {
    }

    [BsonCollection("unbalancedbigsmall2_betlogs")]
    public class UnbalancedBigSmall2BetLogEntity : UnbalancedBigSmallBetLogEntity
    {
    }

    [BsonCollection("unbalancedbigsmall3_betlogs")]
    public class UnbalancedBigSmall3BetLogEntity : UnbalancedBigSmallBetLogEntity
    {
    }

    [BsonCollection("unbalancedbigsmall4_betlogs")]
    public class UnbalancedBigSmall4BetLogEntity : UnbalancedBigSmallBetLogEntity
    {
    }
}