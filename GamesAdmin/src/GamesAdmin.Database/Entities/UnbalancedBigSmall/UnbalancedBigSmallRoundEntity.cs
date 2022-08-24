using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class UnbalancedBigSmallRoundEntity : BigSmallRoundEntity
    {
    }

    [BsonCollection("unbalancedbigsmall1_rounds")]
    public class UnbalancedBigSmall1RoundEntity : UnbalancedBigSmallRoundEntity
    {
    }

    [BsonCollection("unbalancedbigsmall2_rounds")]
    public class UnbalancedBigSmall2RoundEntity : UnbalancedBigSmallRoundEntity
    {
    }

    [BsonCollection("unbalancedbigsmall3_rounds")]
    public class UnbalancedBigSmall3RoundEntity : UnbalancedBigSmallRoundEntity
    {
    }

    [BsonCollection("unbalancedbigsmall4_rounds")]
    public class UnbalancedBigSmall4RoundEntity : UnbalancedBigSmallRoundEntity
    {
    }
}