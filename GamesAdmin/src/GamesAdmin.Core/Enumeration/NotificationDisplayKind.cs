namespace GamesAdmin.Core.Enumeration
{
    public class NotificationDisplayKind : Enumeration
    {
        public const string MarqueeValue = "1";
        public const string DisabledValue = "2";

        public static readonly NotificationDisplayKind Marquee = new NotificationDisplayKind(MarqueeValue, "Marquee");
        public static readonly NotificationDisplayKind DisabledGame = new NotificationDisplayKind(DisabledValue, "DisabledGame");

        public NotificationDisplayKind()
        {
        }

        public NotificationDisplayKind(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
