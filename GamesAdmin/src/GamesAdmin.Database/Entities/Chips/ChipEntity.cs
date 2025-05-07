using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Chips
{
    [BsonCollection("chips")]
    public class ChipEntity : MiniGameBaseEntity
    {
        public string Text { get; set; }

        public double Value { get; set; }

        public string Color { get; set; }
    }
}