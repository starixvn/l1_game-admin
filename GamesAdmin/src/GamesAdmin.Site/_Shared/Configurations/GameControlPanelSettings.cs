namespace GamesAdmin.Site._Shared.Configurations
{
    public interface IGameControlPanelSettings
    {
        string Host { get; }

        string SecretKey { get; }

        string User { get; }

        string Hash { get; }
    }

    public class GameControlPanelSettings : IGameControlPanelSettings
    {
        public GameControlPanelSettings(string host, string secretKey, string user, string hash)
        {
            Host = host;
            SecretKey = secretKey;
            User = user;
            Hash = hash;
        }

        public string Host { get; }

        public string SecretKey { get; }

        public string User { get; }

        public string Hash { get; }
    }
}
