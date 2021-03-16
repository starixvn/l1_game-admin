namespace GamesAdmin.Api._Shared.Configurations
{
    public class GameServerSettings
    {
        public GameServerConfigs Main { get; set; }

        public GameServerConfigs Sedie { get; set; }

        public GameServerConfigs VipSedie { get; set; }

        public GameServerConfigs SpecialSedie { get; set; }

        public GameServerConfigs Blackjack { get; set; }

        public GameServerConfigs VipFishPrawnCrab { get; set; }

        public GameServerConfigs SicBo { get; set; }
        
        public GameServerConfigs VipBaccarat { get; set; }

        public GameServerConfigs VipSedieWar { get; set; }
        
        public GameServerConfigs FourGuardians { get; set; }

        public GameServerConfigs KenoProMax { get; set; }

        public GameServerConfigs MiniSeDie { get; set; }

        public GameServerConfigs Roulette { get; set; }

    }

    public class GameServerConfigs
    {
        public string Api { get; set; }

        public string Socket { get; set; }
    }
}
