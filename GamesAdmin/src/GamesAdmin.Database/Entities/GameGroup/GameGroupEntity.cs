using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.GameGroup
{
    [BsonCollection("game_groups")]
    public class GameGroupEntity : MiniGameBaseEntity
    {
        public string Name { get; set; }

        public IEnumerable<int> GameIds { get; set; }
    }
}