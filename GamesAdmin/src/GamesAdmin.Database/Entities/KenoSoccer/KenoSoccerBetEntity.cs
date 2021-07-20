using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoSoccerBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }

    [BsonCollection("mydinh_cuocs")]
    public class KenoSoccerMyDinhBetEntity : KenoSoccerBetEntity
    {
    }

    [BsonCollection("hangday_cuocs")]
    public class KenoSoccerHangDayBetEntity : KenoSoccerBetEntity
    {
    }

    [BsonCollection("thongnhat_cuocs")]
    public class KenoSoccerThongNhatBetEntity : KenoSoccerBetEntity
    {
    }
}