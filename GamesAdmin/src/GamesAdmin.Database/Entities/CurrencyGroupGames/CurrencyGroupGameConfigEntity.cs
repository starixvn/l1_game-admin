using System.Dynamic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("currency_group_game_configs")]
    public class CurrencyGroupGameConfigEntity : Document
    {
        public int GameId { get; set; }

        public string GroupId { get; set; }

        public string Type { get; set; }

        public ExpandoObject Value { get; set; }
    }
}
