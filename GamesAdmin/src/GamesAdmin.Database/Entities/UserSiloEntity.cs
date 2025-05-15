namespace GamesAdmin.Database.Entities
{
    using GamesAdmin.Database.Attributes;

    [BsonCollection("user_silos")]
    public class UserSiloEntity : Document
    {
        public string Currency { get; set; }

        public string CurrencyCode { get; set; }

        public string NickName { get; set; }

        public string OperatorId { get; set; }

        public object PlayerId { get; set; }

        public string PlayerName { get; set; }

        public string SiteId { get; set; }

        public string SiteName { get; set; }
    }
}
