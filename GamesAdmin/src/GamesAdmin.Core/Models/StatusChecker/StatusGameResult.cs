namespace GamesAdmin.Core.Models
{
    public class StatusGameResult
    {

        public string Error { get; set; } = string.Empty;

        public string Successful { get; set; }

        public int? RoundId { get; set; }

        public int? RemainingTime { get; set; }


    }
}
