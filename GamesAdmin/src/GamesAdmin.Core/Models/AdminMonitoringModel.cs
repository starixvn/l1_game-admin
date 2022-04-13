namespace GamesAdmin.Core.Models
{
    public class AdminMonitoringModel
    {
        public AdminMonitoringModel(string token, string url)
        {
            Token = token;
            Url = url;
        }

        public string Token { get; }

        public string Url { get; }
    }
}