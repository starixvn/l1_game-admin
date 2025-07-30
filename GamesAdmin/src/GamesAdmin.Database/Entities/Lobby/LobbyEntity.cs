using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.Lobby
{
    [BsonCollection("lobbies")]
    public class LobbyEntity : MiniGameBaseEntity
    {
        public string Name { get; set; }
        public string[] CurrencyGroupIds { get; set; }
        public int[] MiniPanelGameIds { get; set; }
        public LobbyCategoryEntity[] Categories { get; set; }
        public BannerEntity[] Banners { get; set; }
        public BannerEntity[] CenterBanners { get; set; }
        public RoundTimerGameEntity RoundTimerGame { get; set; }
    }

    public class LobbyCategoryEntity
    {
        public CategoryNameEntity Name { get; set; }

        public GameSettingEntity[] GameSettings { get; set; }
    }

    public class CategoryNameEntity
    {
        [BsonElement("en-US")]
        public string EN_US { get; set; }

        [BsonElement("vi-VN")]
        public string VI_VN { get; set; }

        [BsonElement("id-ID")]
        public string ID_ID { get; set; }

        [BsonElement("th-TH")]
        public string TH_TH { get; set; }

        [BsonElement("zh-CN")]
        public string ZH_CN { get; set; }

        [BsonElement("zh-TW")]
        public string ZH_TW { get; set; }

        [BsonElement("ja-JP")]
        public string JA_JP { get; set; }

        [BsonElement("es-ES")]
        public string ES_ES { get; set; }
    }

    public class GameSettingEntity
    {
        public int GameId { get; set; }

        public bool BigSize { get; set; }

        public bool NewTag { get; set; }
    }

    public class BannerEntity
    {
        public int GameId { get; set; }

        public int DisplayTime { get; set; }
    }

    public class RoundTimerGameEntity
    {
        public GameEnitity[] Games { get; set; }

        public int Main { get; set; }
    }

    public class GameEnitity
    {
        [BsonElement("id")]
        public int GameId { get; set; }

        public string Name { get; set; }
    }
}
