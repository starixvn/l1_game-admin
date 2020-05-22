using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("kenovietlott_phiens")]
    public class VietlottKenoRoundEntity : MiniGameBaseEntity
    {
        public string RoundNo { get; set; }

        public IEnumerable<byte> Results { get; set; }
    }
}