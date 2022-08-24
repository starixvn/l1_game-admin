namespace GamesAdmin.Core.Enumeration
{
    public class PromotionStatus : Enumeration
    {
        public const string AllValue = "all";
        public const string EnabledValue = "enabled";
        public const string DisabledValue = "disabled";
        public const string DeletedValue = "deleted";

        public static readonly PromotionStatus All = new PromotionStatus(AllValue, "All");
        public static readonly PromotionStatus Enabled = new PromotionStatus(EnabledValue, "Enabled");
        public static readonly PromotionStatus Disabled = new PromotionStatus(DisabledValue, "Disabled");
        public static readonly PromotionStatus Deleted = new PromotionStatus(DeletedValue, "Promotion");

        public PromotionStatus()
        {
        }

        public PromotionStatus(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
