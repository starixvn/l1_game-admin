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

        public GameServerConfigs Baccarat { get; set; }

        public GameServerConfigs FishPrawnCrab { get; set; }

        public GameServerConfigs DragonTiger { get; set; }

        public GameServerConfigs BigSmall { get; set; }

        public GameServerConfigs TurboBigSmall { get; set; }

        public GameServerConfigs TurboOddEven { get; set; }

        public GameServerConfigs Lottery { get; set; }

        public GameServerConfigs ThreeCards { get; set; }

        public GameServerConfigs HeroesSlot { get; set; }
        
        public GameServerConfigs KenoMax { get; set; }

        public GameServerConfigs KenoMax2 { get; set; }

        public GameServerConfigs KenoMini { get; set; }

        public GameServerConfigs KenoMini2 { get; set; }

        public GameServerConfigs KenoEast { get; set; }

        public GameServerConfigs KenoWest { get; set; }

        public GameServerConfigs KenoSouth { get; set; }

        public GameServerConfigs KenoNorth { get; set; }
    }

    public class GameServerConfigs
    {
        public string Api { get; set; }

        public string Socket { get; set; }
    }
}