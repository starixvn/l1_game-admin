using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoBetRecord : BetRecord
    {
        public KenoBetRecord(
            Dictionary<string, decimal> amounts,
            Dictionary<string, decimal> odds)
        {
            Amounts = amounts;
            Odds = odds;
        }

        public Dictionary<string, decimal> Amounts { get; }

        public Dictionary<string, decimal> Odds { get; }

        public decimal Total => Amounts.Select(x => x.Value).Sum();
    }
}
