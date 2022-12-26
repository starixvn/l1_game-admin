namespace GamesAdmin.Core.Models.Lobby
{
    public class LobbyModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string[] CurrencyGroupIds { get; set; }
        public int[] MiniPanelGameIds { get; set; }
        public LobbyCategory[] Categories { get; set; }
        public Banner[] Banners { get; set; }
    }

    public class LobbyCategory
    {
        public CategoryName Name { get; set; }

        public GameSetting[] GameSettings { get; set; }
    }

    public class CategoryName
    {
        public string EN_US { get; set; }

        public string VI_VN { get; set; }

        public string ID_ID { get; set; }

        public string TH_TH { get; set; }

        public string ZH_CN { get; set; }

        public string ZH_TW { get; set; }
    }

    public class GameSetting
    {
        public int GameId { get; set; }

        public bool BigSize { get; set; }

        public bool NewTag { get; set; }
    }

    public class Banner
    {
        public int GameId { get; set; }

        public int DisplayTime { get; set; }
    }
}
