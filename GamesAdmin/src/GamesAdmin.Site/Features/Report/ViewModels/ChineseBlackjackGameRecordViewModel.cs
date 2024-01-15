using System.Linq;
using GamesAdmin.Core;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ChineseBlackjackGameRecordViewModel : BlackjackGameRecordViewModel
    {
        private readonly byte minPoint = 16;

        public override string GetDealerResult(byte maxCardCount, string highestHand)
        {
            if (DealerCards.All(x => x.Rank == CardLabels.Ace) && DealerCards.Count() == 2)
            {
                return Point < minPoint ? DealerPoint.ToString() : "Ban-Ban";
            }

            if (DealerPoint == maxPoint && DealerCards.Count() == 2)
            {
                return Point < minPoint ? DealerPoint.ToString() : highestHand;
            }

            return base.GetDealerResult(maxCardCount, highestHand);
        }

        public override string GetPlayerResult(byte maxCardCount, string highestHand, bool splitRule = false)
        {
            if (Point < minPoint)
            {
                return "Unqualified";
            }

            if (Cards.All(x => x.Rank == CardLabels.Ace) && Cards.Count() == 2)
            {
                return "Ban-Ban";
            }

            return base.GetPlayerResult(maxCardCount, highestHand, splitRule);
        }
    }
}