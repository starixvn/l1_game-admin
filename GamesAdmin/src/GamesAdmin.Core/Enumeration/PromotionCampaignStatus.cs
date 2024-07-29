namespace GamesAdmin.Core.Enumeration
{
    public class PromotionCampaignStatus : Enumeration
    {
        public const string AllValue = "all";
        public const string RunningValue = "running";
        public const string CompletedValue = "completed";
        public const string NotStartedValue = "new";
        public const string PaidValue = "paid";
        public const string DeletedValue = "deleted";

        public static readonly PromotionCampaignStatus All = new PromotionCampaignStatus(AllValue, "All");
        public static readonly PromotionCampaignStatus Running = new PromotionCampaignStatus(RunningValue, "Running");
        public static readonly PromotionCampaignStatus Completed = new PromotionCampaignStatus(CompletedValue, "Completed");
        public static readonly PromotionCampaignStatus NotStarted = new PromotionCampaignStatus(NotStartedValue, "Hasn't Started");

        public PromotionCampaignStatus()
        {
        }

        public PromotionCampaignStatus(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
