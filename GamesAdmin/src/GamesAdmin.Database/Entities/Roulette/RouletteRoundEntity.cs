using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("roulette_phiens")]
    public class RouletteRoundEntity : BaseRoundEntity
    {
        public byte Result { get; set; }

        public RouletteSettlementResultEntity SettlementResult { get; set; }
    }

    public class RouletteSettlementResultEntity
    {
        public IEnumerable<string> StraightUp { get; set; }

        public IEnumerable<string> Split { get; set; }

        public IEnumerable<string> Street { get; set; }

        public IEnumerable<string> Triangle { get; set; }

        public IEnumerable<string> Corner { get; set; }

        public IEnumerable<string> FourNumbers { get; set; }

        public IEnumerable<string> Line { get; set; }

        public IEnumerable<string> Column { get; set; }

        public IEnumerable<string> Dozen { get; set; }

        public IEnumerable<string> RedBlack { get; set; }

        public IEnumerable<string> OddEven { get; set; }

        public IEnumerable<string> BigSmall { get; set; }
    }
}