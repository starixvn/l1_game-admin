using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("chanleturbo_phiens")]
    public class OddEvenTurboRoundEntity : BaseRoundEntity
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }
}