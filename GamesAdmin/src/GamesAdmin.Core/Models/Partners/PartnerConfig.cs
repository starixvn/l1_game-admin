namespace GamesAdmin.Core.Models.Partners
{
    public class PartnerConfig
    {
        public PartnerConfig(string partnerId, string name, string value, int gameId)
        {
            PartnerId = partnerId;
            Name = name;
            Value = value;
            GameId = gameId;
        }

        public string PartnerId { get; }

        public string Name { get; }

        public string Value { get; }

        public int GameId { get; }
    }
}