using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class AnnouncementViewModel : Notification
    {
        public bool IsHappyHours { get; set; } = false;
        public PromotionViewModel Promotion { get; set; }
        public PromotionCampaignViewModel PromotionCampaign { get; set; }
    }
}