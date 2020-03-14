namespace GamesAdmin.Core.Models
{
    public class Game
    {
        public Game(string objectId, int id, string name, bool enabled) 
        {
            ObjectId = objectId;
            Id = id;
            Name = name;
            Enabled = enabled;
        }

        public string ObjectId { get; }

        public int Id { get; }

        public string Name { get; }

        public bool Enabled { get; }
    }
}
