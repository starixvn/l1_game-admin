using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    public class HiLoBetLogEntity : BaseBetEntity
    {
        public int Order { get; set; }

        public decimal Amount { get; set; }

        public string Choice { get; set; }

        public IEnumerable<CardEntity> Cards { get; set; }
    }
}