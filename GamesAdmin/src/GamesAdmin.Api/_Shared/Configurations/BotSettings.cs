namespace GamesAdmin.Api._Shared.Configurations
{
    public interface IBotSettings
    {
        int MinAmount { get; set; }

        int MaxAmount { get; set; }

        int MinInitAmount { get; set; }

        int MaxInitAmount { get; set; }

        int MinAddAmount { get; set; }

        int MaxAddAmount { get; set; }
    }

    public class BotSettings : IBotSettings
    {
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
        public int MinInitAmount { get; set; }
        public int MaxInitAmount { get; set; }
        public int MinAddAmount { get; set; }
        public int MaxAddAmount { get; set; }
    }
}
