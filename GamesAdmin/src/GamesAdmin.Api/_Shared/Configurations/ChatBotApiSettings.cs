namespace GamesAdmin.Api._Shared.Configurations
{
    public class ChatBotApiSettings
    {
        public string Token { get; set; }

        public string Url { get; set; }

        public MessageSettings HighStake { get; set; }

        public int TimeoutInSeconds { get; set; }
    }

    public class MessageSettings
    {
        public string ConversationId { get; set; }
    }
}