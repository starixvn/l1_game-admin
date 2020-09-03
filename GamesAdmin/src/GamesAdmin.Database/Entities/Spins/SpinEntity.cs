using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Spins
{
    [BsonCollection("spins")]
    public class SpinEntity : Document
    {        
        public int MemberId { get; set; }

        public int AvailSpins { get; set; }

        public int UsedSpins { get; set; }

        public bool Enabled { get; set; }
    }
}
