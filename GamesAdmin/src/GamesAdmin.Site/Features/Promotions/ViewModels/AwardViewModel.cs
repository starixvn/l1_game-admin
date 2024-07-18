using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class AwardViewModel : AwardWinningPlayer
    {
        public OWResult Result { get; set; }
    }
}
