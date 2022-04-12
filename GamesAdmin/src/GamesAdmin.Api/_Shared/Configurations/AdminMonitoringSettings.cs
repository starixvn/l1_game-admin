namespace GamesAdmin.Api._Shared.Configurations
{
    public class AdminMonitoringSettings
    {
        public string Url { get; set; }

        public string JwtSecretKey { get; set; }

        public int ExpiredTokenByMinutes { get; set; }
    }
}