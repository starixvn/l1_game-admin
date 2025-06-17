using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("footballcointoss_matches")]
    public class FootballCoinTossMatchEntity : Document
    {
        public string Home { get; set; }

        public string Away { get; set; }

        public DateTime StartTime { get; set; }
    }
}