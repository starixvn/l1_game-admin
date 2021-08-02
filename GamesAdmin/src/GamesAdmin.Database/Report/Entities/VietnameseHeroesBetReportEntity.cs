using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database.Report.Entities
{
    public class VietnameseHeroesBetReportEntity : VietnameseHeroesRoundEntity
    {
        public string Username { get; set; }

        public string Nickname { get; set; }

        public string UserId { get; set; }
    }
}