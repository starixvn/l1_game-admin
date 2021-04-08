using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("chanleturbo_phiens")]
    public class OddEvenTurboRoundEntity : BaseRoundEntity
    {
        public int? Dice1 { get; set; }

        public int? Dice2 { get; set; }

        public int? Dice3 { get; set; }
        public OddEvenTurboResult Result { get; set; }
    }

    public class OddEvenTurboResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }
}