namespace GamesAdmin.Api._Shared.Configurations
{
    public interface IBotSettings
    {
        int DefaultMaxBet { get; set; }
    }

    public class BotSettings : IBotSettings
    {
        public int DefaultMaxBet { get; set; }
    }
}
