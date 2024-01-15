using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BlackjackGameRecordViewModel : BetRecordViewModel
    {
        protected const byte SpecialWinCardCount = 2;

        public const byte MaxPoint = 21;

        public long HandId { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal Bet { get; set; }

        public long? SplitForm { get; set; }

        public long Round { get; set; }

        public IEnumerable<CardViewModel> Cards { get; set; }

        public IEnumerable<string> Actions { get; set; }

        public IEnumerable<CardViewModel> DealerCards { get; set; }

        public bool Ended { get; set; }

        public byte Point { get; set; }

        public byte DealerPoint { get; set; }

        public IEnumerable<byte> SoftPoints { get; set; }

        public bool IsSplittedHand => SplitForm.HasValue || Actions.Any(x => x == BlackjackGameAction.Split);

        public string SplitLabel => IsSplittedHand ? "(Split)" : string.Empty;

        public virtual string GetDealerResult(byte maxCardCount, string highestHand)
        {
            if (DealerPoint == MaxPoint && DealerCards.Count() == 2)
            {
                return highestHand;
            }
            else if (DealerPoint <= MaxPoint && DealerCards.Count() == maxCardCount)
            {
                return "Five-Card";
            }
            else
            {
                return DealerPoint > MaxPoint ? "Bust" : DealerPoint.ToString();
            }
        }

        public virtual string GetPlayerResult(byte maxCardCount, string highestHand, bool splitRule = false)
        {
            if (Point == MaxPoint && Cards.Count() == 2 && !splitRule)
            {
                return highestHand;
            }
            else if (Point <= MaxPoint && Cards.Count() == maxCardCount)
            {
                return "Five-Card";
            }
            else
            {
                var pointValue = Ended ? Point.ToString() : string.Join("/", SoftPoints);
                return Point > MaxPoint ? "Bust" : pointValue;
            }
        }
    }
}