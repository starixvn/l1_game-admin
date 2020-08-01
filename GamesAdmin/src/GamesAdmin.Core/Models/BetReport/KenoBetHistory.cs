using System;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoBetHistory
    {
        public KenoBetHistory(
            long round,
            string name,
            KenoBetChoice choice,
            decimal amount,
            int? memberId,
            string siteId,
            DateTime time)
        {
            Round = round;
            Name = name;
            Choice = choice;
            Amount = amount;
            MemberId = memberId;
            SiteId = siteId;
            Time = time;
        }

        public long Round { get; }

        public string Name { get; }

        public KenoBetChoice Choice { get; }

        public decimal Amount { get; }

        public int? MemberId { get; }

        public string SiteId { get; }

        public DateTime Time { get; }
    }
}
