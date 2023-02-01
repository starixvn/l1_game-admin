namespace GamesAdmin.Core.Models.Lobby
{
    public class GameSettingLog
    {
        public string GameName { get; set; }

        public bool BigSize { get; set; }

        public bool NewTag { get; set; }
    }

    public class BannerLog
    {
        public string GameName { get; set; }

        public int DisplayTime { get; set; }
    }
}
