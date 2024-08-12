namespace GamesAdmin.Core.Enumeration
{
    public class NotificationDisplayKind : Enumeration
    {
        public const string MarqueeValue = "1";
        public const string DisabledValue = "2";
        public const string PromotionValue = "3";
        public const string PromotionCampaignValue = "4";

        public static readonly NotificationDisplayKind Marquee = new NotificationDisplayKind(MarqueeValue, "Marquee");
        public static readonly NotificationDisplayKind DisabledGame = new NotificationDisplayKind(DisabledValue, "DisabledGame");
        public static readonly NotificationDisplayKind Promotion = new NotificationDisplayKind(PromotionValue, "Happy Hours");
        public static readonly NotificationDisplayKind PromotionCampaign = new NotificationDisplayKind(PromotionCampaignValue, "Tournament Promotion");

        public NotificationDisplayKind()
        {
        }

        public NotificationDisplayKind(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
