namespace GamesAdmin.Api._Shared.Configurations
{
    public class ChatBotApiSettings
    {
        public string Token { get; set; }

        public string Url { get; set; }

        public ConversationIds ConversationIds { get; set; }

        public int TimeoutInSeconds { get; set; }

        public string Proxy { get; set; }

        public double HighStakeAmount { get; set; }
    }

    public class ConversationIds
    {
        public string HighStake { get; set; }
        public string Promotion { get; set; }
    }
}