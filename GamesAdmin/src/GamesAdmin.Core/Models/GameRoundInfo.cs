namespace GamesAdmin.Core.Models
{
    public class GameRoundInfo
    {
        public long RoundId { get; set; }

        public int RemainingTime { get; set; }

        public bool IsDisabled { get; set; }
    }
}
