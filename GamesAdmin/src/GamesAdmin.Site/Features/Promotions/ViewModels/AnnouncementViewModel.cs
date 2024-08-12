using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class AnnouncementViewModel : Notification
    {
        public bool IsHappyHours => DisplayKind.Value == NotificationDisplayKind.Promotion.Value;
        public PromotionViewModel Promotion { get; set; }
        public PromotionCampaignViewModel PromotionCampaign { get; set; }
    }
}