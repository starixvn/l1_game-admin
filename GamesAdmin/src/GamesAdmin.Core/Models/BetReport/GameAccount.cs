namespace GamesAdmin.Core.Models.BetReport
{
    public class GameAccount
    {
        public GameAccount(
            string username,
            int memberId,
            string memberName,
            int siteId,
            string language,
            bool? isBot,
            string currency,
            string name,
            int? uid)
        {
            Username = username;
            MemberId = memberId;
            MemberName = memberName;
            SiteId = siteId;
            Language = language;
            IsBot = isBot;
            Currency = currency;
            Name = name;
            UID = uid;
        }

        public string Username { get; }

        public int MemberId { get; }

        public string MemberName { get; }

        public int SiteId { get; }

        public string Language { get; }

        public bool? IsBot { get; }

        public string Currency { get; }

        public string Name { get; }

        public int? UID { get; }

        public bool IsUUS => Currency.Equals("UUS", System.StringComparison.OrdinalIgnoreCase);
    }
}
