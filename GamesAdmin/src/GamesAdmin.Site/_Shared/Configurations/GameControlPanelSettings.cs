namespace GamesAdmin.Site._Shared.Configurations
{
    public interface IGameControlPanelSettings
    {
        string Host { get; }

        string SecretKey { get; }
    }

    public class GameControlPanelSettings : IGameControlPanelSettings
    {
        public GameControlPanelSettings(string host, string secretKey)
        {
            Host = host;
            SecretKey = secretKey;
        }

        public string Host { get; }

        public string SecretKey { get; }
    }
}
