using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoSoccerOneEntity : BaseBetEntity
    {
        public decimal PreHTHome { get; set; }
        public decimal PreHTAway { get; set; }
        public decimal PreHTOver { get; set; }
        public decimal PreHTUnder { get; set; }
        public decimal PreHTOdd { get; set; }
        public decimal PreHTEven { get; set; }

        public decimal PreFTHome { get; set; }
        public decimal PreFTAway { get; set; }
        public decimal PreFTOver { get; set; }
        public decimal PreFTUnder { get; set; }
        public decimal PreFTOdd { get; set; }
        public decimal PreFTEven { get; set; }

        public decimal LiveFTHome { get; set; }
        public decimal LiveFTAway { get; set; }
        public decimal LiveFTOver { get; set; }
        public decimal LiveFTUnder { get; set; }
        public decimal LiveFTOdd { get; set; }
        public decimal LiveFTEven { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal Refund { get; set; }

        public Dictionary<string, bool> Odds { get; set; }
    }

    [BsonCollection("mydinh_ones")]
    public class KenoSoccerMyDinhOneEntity : KenoSoccerOneEntity
    {
    }

    [BsonCollection("hangday_ones")]
    public class KenoSoccerHangDayOneEntity : KenoSoccerOneEntity
    {
    }

    [BsonCollection("thongnhat_ones")]
    public class KenoSoccerThongNhatOneEntity : KenoSoccerOneEntity
    {
    }
}