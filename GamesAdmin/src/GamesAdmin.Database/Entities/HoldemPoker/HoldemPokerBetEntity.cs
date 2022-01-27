using GamesAdmin.Core.Models;
using System;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    public class HoldemPokerBetEntity : BaseBetEntity
    {
        public decimal Ante { get; set; }

        public decimal AnteOdds { get; set; }

        public decimal Call { get; set; }

        public decimal CallOdds { get; set; }

        public bool IsFold { get; set; }

        public DateTime CallTime { get; set; }

        public IEnumerable<PokerCard> Cards { get; set; }

        public CombinationCard Result { get; set; }
    }
}
