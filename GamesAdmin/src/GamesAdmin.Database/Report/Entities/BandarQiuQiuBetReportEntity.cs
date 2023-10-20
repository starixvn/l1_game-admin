using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database.Report.Entities
{
    public class BandarQiuQiuBetReportEntity : BandarQiuQiuBetEntity
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }
    }
}