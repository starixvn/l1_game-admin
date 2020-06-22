using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.ProVietKeno
{
    [BsonCollection("kenoproviet_phiens_log")]
    public class ProvietKenoRoundLogEntity : BaseRoundEntity
    {
        public string Source { get; set; }
    }
}