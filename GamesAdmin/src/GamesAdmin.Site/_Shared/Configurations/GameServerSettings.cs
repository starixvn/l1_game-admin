namespace GamesAdmin.Site._Shared.Configurations
{
    public class GameServerSettings
    {
        public GameServerConfigs Main { get; set; }

        public GameServerConfigs Sedie { get; set; }

        public GameServerConfigs VipSedie { get; set; }

        public GameServerConfigs SpecialSedie { get; set; }

        public GameServerConfigs VipFishPrawnCrab { get; set; }
    }

    public class GameServerConfigs 
    { 
        public string Api { get; set; }

        public string Socket { get; set; }

        public string SocketRoute { get; set; }
    }
}
