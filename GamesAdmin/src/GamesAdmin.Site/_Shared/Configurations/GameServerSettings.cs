namespace GamesAdmin.Site._Shared.Configurations
{
    public class GameServerSettings
    {
        public GameServerConfigs Main { get; set; }

        public GameServerConfigs Sedie { get; set; }
    }

    public class GameServerConfigs 
    { 
        public string Api { get; set; }

        public string Socket { get; set; }
    }
}
