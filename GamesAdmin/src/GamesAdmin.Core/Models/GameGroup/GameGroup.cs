using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class GameGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> GameIds { get; set; }
    }
}