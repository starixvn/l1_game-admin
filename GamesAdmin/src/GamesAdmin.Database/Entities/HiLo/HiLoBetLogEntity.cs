using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("hilo_betlogs")]
    public class HiLoBetLogEntity : BaseBetEntity
    {
        public long RoundId { get; set; }

        public int Order { get; set; }

        public decimal Amount { get; set; }

        public string Choice { get; set; }

        public IEnumerable<CardEntity> Cards { get; set; }
    }
}