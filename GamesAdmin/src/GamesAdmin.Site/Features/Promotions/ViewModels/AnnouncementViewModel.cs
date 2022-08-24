using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class AnnouncementViewModel : Notification
    {
        public PromotionViewModel Promotion { get; set; }
    }
}