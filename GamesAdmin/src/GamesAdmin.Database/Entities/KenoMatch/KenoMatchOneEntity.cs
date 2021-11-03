using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoMatchOneEntity : BaseBetEntity
    {
        public decimal FHHome { get; set; }
        public decimal FHRed { get; set; }
        public decimal FHOver { get; set; }
        public decimal FHUnder { get; set; }
        public decimal FHOdd { get; set; }
        public decimal FHEven { get; set; }

        public decimal FTHome { get; set; }
        public decimal FTRed { get; set; }
        public decimal FTOver { get; set; }
        public decimal FTUnder { get; set; }
        public decimal FTOdd { get; set; }
        public decimal FTEven { get; set; }

        public decimal LiveFTHome { get; set; }
        public decimal LiveFTRed { get; set; }
        public decimal LiveFTOver { get; set; }
        public decimal LiveFTUnder { get; set; }
        public decimal LiveFTOdd { get; set; }
        public decimal LiveFTEven { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal Refund { get; set; }

        public Dictionary<string, bool> Odds { get; set; }
    }

    [BsonCollection("kenomatch1_ones")]
    public class KenoMatch1OneEntity : KenoMatchOneEntity
    {
    }

    [BsonCollection("kenomatch2_ones")]
    public class KenoMatch2OneEntity : KenoMatchOneEntity
    {

    }
}