namespace GamesAdmin.Core.Models
{
    public class GameAccount
    {
        public GameAccount(
            string username,
            int memberId,
            string memberName,
            string language,
            bool isBot,
            string currency,
            string name,
            int uid)
        {
            Username = username;
            MemberId = memberId;
            MemberName = memberName;
            Language = language;
            IsBot = isBot;
            Currency = currency;
            Name = name;
            UID = uid;
        }

        public string Username { get; }

        public int MemberId { get; }

        public string MemberName { get; }

        public string Language { get; }

        public bool IsBot { get; }

        public string Currency { get; }

        public string Name { get; }

        public int UID { get; }

        public bool IsUUS => Currency != null && Currency.Equals("UUS", System.StringComparison.OrdinalIgnoreCase);
    }
}
