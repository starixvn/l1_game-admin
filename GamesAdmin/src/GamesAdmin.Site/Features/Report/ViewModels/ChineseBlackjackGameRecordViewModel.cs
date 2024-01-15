using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ChineseBlackjackGameRecordViewModel : BlackjackGameRecordViewModel
    {
        private readonly byte minPoint = 16;



        public override string GetDealerResult(byte maxCardCount, string highestHand)
        {
            if (DealerCards.All(x => x.Rank == CardLabels.Ace) && DealerCards.Count() == SpecialWinCardCount)
            {
                return "Ban-Ban";
            }

            if (DealerPoint == MaxPoint && DealerCards.Count() == SpecialWinCardCount)
            {
                return highestHand;
            }

            return base.GetDealerResult(maxCardCount, highestHand);
        }

        public override string GetPlayerResult(byte maxCardCount, string highestHand, bool splitRule = false)
        {
            if (IsPlayerHandUnqualified(maxCardCount))
            {
                return "Unqualified";
            }

            if (Cards.All(x => x.Rank == CardLabels.Ace) && Cards.Count() == SpecialWinCardCount)
            {
                return "Ban-Ban";
            }

            return base.GetPlayerResult(maxCardCount, highestHand, splitRule);
        }

        private bool IsPlayerHandUnqualified(byte maxCardCount) => 
            Point < minPoint && Cards.Count() < maxCardCount && Actions.Any(x => x == BlackjackGameAction.Stand)
            && !HandGetSpecialWin(DealerCards, DealerPoint);

        private bool HandGetSpecialWin(IEnumerable<CardViewModel> cards, byte point) =>
            cards.Count() == SpecialWinCardCount && point == MaxPoint;
    }
}