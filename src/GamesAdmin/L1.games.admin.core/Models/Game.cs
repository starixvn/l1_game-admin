namespace L1.games.admin.core.Models
{
    public class Game
    {
        public Game(int id, string name, bool enabled) 
        {
            Id = id;
            Name = name;
            Enabled = enabled;
        }

        public int Id { get; }

        public string Name { get; }

        public bool Enabled { get; }
    }
}
