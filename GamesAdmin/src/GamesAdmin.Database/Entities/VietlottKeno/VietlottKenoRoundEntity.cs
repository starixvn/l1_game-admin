using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("kenovietlott_phiens")]
    public class VietlottKenoRoundEntity : BaseRoundEntity
    {
        public IEnumerable<byte> Results { get; set; }
    }
}