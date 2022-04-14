namespace GamesAdmin.Site._Shared.Configurations
{
    public class GameServerSettings
    {
        public GameServerConfigs Main { get; set; }
        public GameServerConfigs Sedie { get; set; }
        public GameServerConfigs VipSedie { get; set; }
        public GameServerConfigs SpecialSedie { get; set; }
        public GameServerConfigs VipFishPrawnCrab { get; set; }
        public GameServerConfigs SicBo { get; set; }
        public GameServerConfigs VipSeDieWar { get; set; }
        public GameServerConfigs VipBaccarat { get; set; }
        public GameServerConfigs KenoProMax { get; set; }
        public GameServerConfigs MiniSeDie { get; set; }
        public GameServerConfigs FishPrawnCrab { get; set; }
        public GameServerConfigs BigSmall { get; set; }
        public GameServerConfigs TurboBigSmall { get; set; }
        public GameServerConfigs DragonTiger { get; set; }
        public GameServerConfigs TurboOddEven { get; set; }
        public GameServerConfigs Roulette { get; set; }
        public GameServerConfigs Baccarat { get; set; }
        public GameServerConfigs Lottery { get; set; }
        public GameServerConfigs ThreeCards { get; set; }
        public GameServerConfigs MiniKeno { get; set; }
        public GameServerConfigs BitcoinMaster { get; set; }
        public GameServerConfigs KenoMatch1 { get; set; }
        public GameServerConfigs KenoMatch2 { get; set; }
        public GameServerConfigs KenoPower1 { get; set; }
        public GameServerConfigs KenoPower2 { get; set; }
        public GameServerConfigs CasinoHoldemPoker { get; set; }
        public GameServerConfigs TexasHoldemPoker { get; set; }
    }

    public class GameServerConfigs
    {
        public string Api { get; set; }

        public string Socket { get; set; }

        public string SocketRoute { get; set; }
    }
}