using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoReportViewModel
    {
        public KenoReportViewModel(
            Game game, 
            long roundId, 
            KenoRoundResult roundResult, 
            IEnumerable<KenoBetRecord> betTickets,
            TimeZoneInfo timeZoneInfo)
        {
            Game = game;
            RoundId = roundId;
            RoundResult = new KenoResultViewModel(
                roundResult?.Results,
                roundResult?.SettlementResult,
                roundResult?.Time);
            BetTickets = betTickets;
            TimeZoneInfo = timeZoneInfo;
        }

        public Game Game { get; }

        public long RoundId { get; }

        public KenoResultViewModel RoundResult { get; }

        public IEnumerable<KenoBetRecord> BetTickets { get; }

        public TimeZoneInfo TimeZoneInfo { get; }

        public int Total => BetTickets?.Count() == 0 ? 0 : BetTickets.Count();
    }
}
