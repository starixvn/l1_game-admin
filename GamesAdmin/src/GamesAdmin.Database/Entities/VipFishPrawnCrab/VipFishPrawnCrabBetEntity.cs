using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vipfishprawncrab_cuocs")]
    public class VipFishPrawnCrabBetEntity : BaseBetEntity
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }

        public decimal AnyTriple { get; set; }

        public decimal Mountain { get; set; }

        public decimal Ocean { get; set; }
    }
}