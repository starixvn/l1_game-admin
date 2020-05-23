namespace GamesAdmin.Core.Enumeration
{
    public class NotificationDisplayKind : Enumeration
    {
        public const string MarqueeValue = "1";

        public static readonly NotificationDisplayKind Marquee = new NotificationDisplayKind(MarqueeValue, "Marquee");

        public NotificationDisplayKind()
        {
        }

        public NotificationDisplayKind(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
