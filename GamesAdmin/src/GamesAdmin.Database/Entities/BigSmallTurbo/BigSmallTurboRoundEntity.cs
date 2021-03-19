using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("txturbo_phiens")]
    public class BigSmallTurboRoundEntity : BaseRoundEntity
    {
        public int? Dice1 { get; set; }

        public int? Dice2 { get; set; }

        public int? Dice3 { get; set; }
        public BigSmallTurboResult Result { get; set; }
    }

    public class BigSmallTurboResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }
}