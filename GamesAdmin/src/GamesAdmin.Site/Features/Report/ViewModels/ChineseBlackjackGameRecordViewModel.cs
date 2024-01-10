using System.Linq;
using GamesAdmin.Core;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ChineseBlackjackGameRecordViewModel : BlackjackGameRecordViewModel
    {
        public override string GetDealerResult(byte maxCardCount, string highestHand)
        {
            if (DealerCards.All(x => x.Rank == CardLabels.Ace) && DealerCards.Count() == 2)
            {
                return "Ban-ban";
            }

            return base.GetDealerResult(maxCardCount, highestHand);
        }

        public override string GetPlayerResult(byte maxCardCount, string highestHand, bool splitRule = false)
        {
            if (Cards.All(x => x.Rank == CardLabels.Ace) && Cards.Count() == 2)
            {
                return "Ban-ban";
            }

            return base.GetPlayerResult(maxCardCount, highestHand, splitRule);
        }
    }
}