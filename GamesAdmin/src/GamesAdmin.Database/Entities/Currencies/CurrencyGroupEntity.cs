using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Currencies
{
    [BsonCollection("currency_groups")]
    public class CurrencyGroupEntity : Document
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool FractionalOddsFormat { get; set; }

        public string PartnerId { get; set; }
    }
}