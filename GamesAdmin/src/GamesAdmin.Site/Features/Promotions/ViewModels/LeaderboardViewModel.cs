namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class LeaderboardViewModel
    {
        public LeaderboardViewModel(int promotionId)
        {
            this.PromotionId = promotionId;
            this.PlayerName = string.Empty;
            this.IsTestCurrency = false;
        }

        public int PromotionId { get; set; }
        public string PlayerName { get; set; }
        public bool IsTestCurrency { get; set; }
    }
}