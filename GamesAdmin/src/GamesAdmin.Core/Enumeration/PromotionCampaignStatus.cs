namespace GamesAdmin.Core.Enumeration
{
    public class PromotionCampaignStatus : Enumeration
    {
        public const string AllValue = "all";
        public const string RunningValue = "running";
        public const string CompletedValue = "completed";

        public static readonly PromotionStatus All = new PromotionStatus(AllValue, "All");
        public static readonly PromotionStatus Running = new PromotionStatus(RunningValue, "Running");
        public static readonly PromotionStatus Completed = new PromotionStatus(CompletedValue, "Completed");

        public PromotionCampaignStatus()
        {
        }

        public PromotionCampaignStatus(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
