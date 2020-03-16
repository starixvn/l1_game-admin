using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class Game
    {
        [JsonConstructor]
        public Game(string Id, string name, bool enabled) 
        {
            this.Id = Id;
            Name = name;
            Enabled = enabled;
        }

        public string Id { get; }

        public string Name { get; }

        public bool Enabled { get; }
    }
}
