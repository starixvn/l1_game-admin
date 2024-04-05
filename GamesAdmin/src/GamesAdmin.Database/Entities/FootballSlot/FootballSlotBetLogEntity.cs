using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("footballslot_betlogs")]
    public class FootballSlotBetLogEntity : BaseBetEntity
    {
        public string Choice { get; set; }
        public IEnumerable<string> SubChoices { get; set; }
        public decimal Amount { get; set; }
    }
}